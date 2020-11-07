# chat/consumers.py
import ast
import json
import uuid

from channels.auth import UserLazyObject
from channels.generic.websocket import AsyncWebsocketConsumer
from django.contrib.auth.models import AnonymousUser

from models.models import Student, Lecture, Lecturer
from utils.utils import get_free_seat


class LectvreConsumer(AsyncWebsocketConsumer):
    rooms = {}
    users = {}
    user_room = {}

    async def connect(self):
        user: AnonymousUser = self.scope['user']

        self.users[user]: Student = Student(id=self.gen_user_id())

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

    # async def disconnect(self, close_code):
    #     # Leave room group
    #     await self.channel_layer.group_discard(
    #         self.room_group_name,
    #         self.channel_name
    #     )

    # Receive message from WebSocket
    async def receive(self, text_data: str):
        print(text_data)
        message = ast.literal_eval(text_data)

        user: Student = self.users[self.scope['user']]

        if "type" not in message:
            return

        room = self.user_room.get(user.id, None)

        return_data = {}

        if message['type'] == "join":
            if message['room'] not in self.rooms:
                return
            else:
                room: Lecture = self.rooms[message['room']]
                self.user_room[user.id] = room

                user_obj: Student = Student(id=user.id, seat=get_free_seat(room), lecture=room, **message)
                self.users[user.id] = user_obj

                self.rooms[message['room']].students.append(user_obj)

                await self.add_to_room(user_obj, room)

                await self.channel_layer.group_send(
                    room.id,
                    {
                        'type': 'update',
                        'message': {
                            "type": "new_user",
                            "user_id": user.id,
                            "name": user_obj.name,
                            "minifig": user_obj.minifig,
                        }
                    }
                )

                return_data = {
                    "type": "position",
                    "user_id": user.id,
                    "x": user_obj.position[0],
                    "y": user_obj.position[1],
                    "z": user_obj.position[2],
                    "r": user_obj.position[3]
                }
        elif message['type'] == "create_group":
            user_obj: Lecturer = Lecturer(id=user.id, **message)
            room: Lecture = Lecture(id=self.gen_user_id(), lecturer=user_obj)
            user_obj.lecture = room

            self.rooms[room.id] = room
            self.user_room[user.id] = room.id

            return_data = {
                "room_id": room.id,
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
                "user_id": user.id,
                "hand_up": user.hand_up
            }

        elif message['type'] == "note_change":
            pass
        elif message['type'] == "position":
            print(message)
            return_data = {**message, "user_id": user.id}

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
