#!/usr/bin/env python
from threading import Lock
from flask import Flask, render_template, session, request, \
    copy_current_request_context, g
from flask_socketio import SocketIO, send, join_room, leave_room, \
    close_room, rooms, disconnect

# Set this variable to "threading", "eventlet" or "gevent" to test the
# different async modes, or leave it set to None for the application to choose
# the best option based on installed packages.
from models import Lecture, Student, Lecturer
from utils import get_free_seat

async_mode = "threading"

app = Flask(__name__)
app.config['SECRET_KEY'] = 'secret!'
socketio = SocketIO(app, async_mode=async_mode)
thread = None
thread_lock = Lock()


@app.route('/')
def index():
    return render_template('index.html', async_mode=socketio.async_mode)


@socketio.on('json')
def handle_json(message):
    if "type" not in message:
        return

    if message['type'] == "join":
        join_room(message['room'])

        room: Lecture = g.rooms[message['room']]
        session['user'] = Student(id=request.sid, seat=get_free_seat(room), lecture=room, **message)

        g.rooms[message['room']].students.append(session['user'])

        send({
            "type": "init",
            "user_id": request.sid,
        }, json=True)
        send({
            "type": "user_join",
            "user_id": request.sid,
            "name": session['user'].name,
            "body": {
                "head": None,
                "face": None,
                "body": None,
                "legs": None,
            },
        }, json=True, broadcast=True)
    elif message['type'] == "create_room":
        print("Create a room bing bong")
        session['user'] = Lecturer(id=request.sid, location=(0,0,0), **message)
        room: Lecture = Lecture(id=request.sid, lecturer=session['user'], **message)

        if "rooms" not in g:
            g.rooms = {}

        g.rooms[request.sid] = room

        send({
            "room_id": request.sid,
        }, json=True)
        send({
            "type": "init",
            "user_id": request.sid,
        }, json=True)
    elif message['type'] == "leave":
        # Leave room code
        pass
    elif message['type'] == "position":
        update_movement(message)
    elif message['type'] == "note_change":
        pass


@socketio.on('movement_event')
def update_movement(message):
    send({**message, "user_id": request.sid}, json=True, broadcast=True)


@socketio.on('disconnect_request')
def disconnect_request():
    @copy_current_request_context
    def can_disconnect():
        disconnect()

    send({'data': 'Disconnected'},
         json=True,
         broadcast=True,
         callback=can_disconnect)


@socketio.on('connect')
def connect_user():
    print("Connected")


@socketio.on('disconnect')
def test_disconnect():
    print('Client disconnected')


if __name__ == '__main__':
    socketio.run(app, debug=True)
