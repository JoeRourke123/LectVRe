# chat/consumers.py
import ast
import json
import uuid

from channels.generic.websocket import AsyncWebsocketConsumer
from django.contrib.auth.models import AnonymousUser

from models.models import Student, Lecture, Lecturer
from utils.utils import get_free_seat


class LectvreConsumer(AsyncWebsocketConsumer):
    rooms = {}
    users = {}
    user_room = {}

    async def connect(self):

        scope_user = None
        for header in self.scope['headers']:
            if header[0] == b'sec-websocket-key':
                scope_user = str(header[1].decode('utf-8'))

        if "db1f5" not in self.rooms:
            self.rooms["db1f5"] = Lecture(id="db1f5", lecturer=Lecturer(id=self.gen_user_id(), name="Name"))

        self.users[scope_user]: Student = Student(id=self.gen_user_id())

        await self.accept()

    def gen_user_id(self):
        u = str(uuid.uuid4())
        while u in self.users:
            u = str(uuid.uuid4())

        return u

    async def add_to_room(self, user, room):
        self.groups.append(room.id)
        self.groups.append(user.id)
        await self.channel_layer.group_add(
            room if type(room) == str else room.id,
            self.channel_name
        )
        await self.channel_layer.group_add(
            user.id,
            self.channel_name
        )

    async def disconnect(self, close_code):
        # Leave room group
        scope_user = None
        for header in self.scope['headers']:
            if header[0] == b'sec-websocket-key':
                scope_user = str(header[1].decode('utf-8'))

        user = self.users[scope_user]

        if user.lecture:
            await self.channel_layer.group_send(
                user.lecture.id,
                {
                    'type': 'update',
                    'message': {
                        "type": "left_room",
                        "user_id": user.id,
                    }
                }
            )
            await self.channel_layer.group_discard(
                user.lecture.id,
                self.channel_name
            )
            for i in range(len(self.rooms[user.lecture.id].students)):
                if self.rooms[user.lecture.id].students[i].id == user.id:
                    self.rooms[user.lecture.id].students.pop(i)

        await self.channel_layer.group_discard(
            user.id,
            self.channel_name
        )

        if self.scope['user'] in self.users:
            del self.users[self.scope['user']]

        if user.id in self.user_room:
            del self.user_room[user.id]

    # Receive message from WebSocket
    async def receive(self, text_data: str):
        scope_user = None
        for header in self.scope['headers']:
            if header[0] == b'sec-websocket-key':
                scope_user = str(header[1].decode('utf-8'))

        message = ast.literal_eval(str(text_data).replace('\x00', ''))
        message['message']['type'] = message['type']
        message = message['message']

        user: Student = self.users[scope_user]

        if "type" not in message:
            return

        room = self.user_room.get(user.id, None)

        return_data = {}

        if message['type'] == "join":
            message['room'] = "db1f5"

            if message['room'] not in self.rooms:
                return
            else:
                room: Lecture = self.rooms[message['room']]
                self.user_room[user.id] = room

                user_obj: Student = Student(id=user.id, seat=get_free_seat(room), lecture=room, **message)
                self.users[scope_user] = user_obj

                self.rooms[message['room']].students.append(user_obj)

                await self.add_to_room(user_obj, room)

                await self.channel_layer.group_send(
                    room.id,
                    {
                        'type': 'update',
                        'message': {
                            "type": "new_user",
                            "message": {
                                "user": user.id,
                                "name": user_obj.name,
                                "seat": user_obj.seat,
                                "minifig": user_obj.minifig,
                            }
                        }
                    }
                )

                return_data = {
                    "type": "position",
                    "message": {
                        "user": user_obj.id,
                        "name": user_obj.name,
                        "x": user_obj.position[0],
                        "y": user_obj.position[1],
                        "z": user_obj.position[2],
                        "r": user_obj.position[3],
                        "minifig": user_obj.minifig,
                    }
                }

                print(return_data)
        elif message['type'] == "create_room":
            user_obj: Lecturer = Lecturer(id=user.id, **message)
            room: Lecture = Lecture(id=self.gen_user_id()[0:5], lecturer=user_obj)
            user_obj.lecture = room

            self.rooms[room.id] = room
            self.user_room[user.id] = room.id

            return_data = {
                "type": "create_room",
                "message": {
                    "room": room.id,
                    "user": user.id,
                }
            }

            await self.add_to_room(user_obj, room)
        elif message['type'] == "leave":
            pass
        elif message['type'] == "handup":
            room: Lecture = self.rooms[user.lecture.id]

            for student in room.students:
                if student.id == user.id:
                    user.hand_up = not user.hand_up
                    student.hand_up = not student.hand_up

            return_data = {
                "type": "hand_up",
                "message": {
                    "user": user.id,
                    "hand_up": user.hand_up
                }
            }

        elif message['type'] == "note_change":
            pass
        elif message['type'] == "position":
            user_obj = self.users[scope_user]
            return_data = {**message, "user": user.id, "minifig": user_obj.minifig}

        print("Response: " + str(return_data))

        # Send message to room group
        await self.channel_layer.group_send(
            room if room.__class__ is str else room.id,
            {
                'type': 'update',
                'message': return_data
            }
        )

    # Receive message from room group
    async def update(self, event):
        message = event['message']

        # Send message to WebSocket
        await self.send(text_data=json.dumps(message))
