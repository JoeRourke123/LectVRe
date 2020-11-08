from models.models import Lecture
from random import choice


def get_free_seat(lecture: Lecture):
    seats = []

    for i in range(9):
        if not any(i == s.seat for s in lecture.students):
            seats.append(i)

    chosen_seat = choice(seats)

    return chosen_seat