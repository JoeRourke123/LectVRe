class Lecture(object):
    def __init__(self, **kwargs):
        self.id: int = kwargs.get("id", 0)
        self.lecturer: Lecturer = kwargs.get("lecturer", None)
        self.students: list = []
        self.slideURL: str = kwargs.get("slideURL", "")
        self.slide: int = 1

class Lecturer(object):
    def __init__(self, **kwargs):
        self.id: int = kwargs.get("id", 0)
        self.username: str = kwargs.get("name", "Jimmy Johnjohn")
        self.lecture: Lecture = kwargs.get("lecture", None)
        self.minifig: dict = kwargs.get("minifig", {
            "hair": None,
            "face": None,
            "body": None,
            "legs": None,
        })


class Student(Lecturer):
    def __init__(self, **kwargs):
        Lecturer.__init__(self, **kwargs)
        self.notes: list = []
        self.hand_up: bool = False
        self.depressed: bool = True
        self.seat: int = kwargs.get("seat", 0)
