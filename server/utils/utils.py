from models.models import Lecture
from random import choice


def get_free_seat(lecture: Lecture):
    seats = [i for i in range(9)]

    for student in lecture.students:
        print(student.seat)
        seats.remove(student.seat)

    chosen_seat = choice(seats)
    location = (0,0,0,0)

    return chosen_seat, location
