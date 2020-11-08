from django.http import HttpResponse
from django.shortcuts import render


def index_page(request):
    return render(request, "index.html")


def view_page(request):
    return HttpResponse("<h1>Go to https://lectvre.space/djhit5/0 to make your notes and see them appear here!</h1>")