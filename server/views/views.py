from django.http import HttpResponse
from django.shortcuts import render

from selenium import webdriver

from querystring_parser import parser
from time import sleep as wake


def index_page(request):
    return render(request, "index.html")


def get_str_between(str1, str2, full: str):
    if str2 in full:
        s2i = full.index(str2)
    else:
        s2i = len(full)

    if str1 in full:
        s1i = full.index(str1)
    else:
        s1i = len(full)

    return full[s1i + len(str1) : s2i]


def view_page(request, *args, **kwargs):
    print(kwargs)
    params = (request.scope['query_string']).decode('utf-8')

    parsed_params = parser.parse(params)
    print(parsed_params)
    url = parsed_params.get("toget", "")
    slide = parsed_params.get("slide", 1)

    browser = webdriver.Firefox()

    if "docs.google.com" in url:
        split_url = url.split("/")
        split_url[-1] = "present#" + slide
        split_url[0] += "/"
        url = "/".join(split_url)

    browser.get(url)
    wake(2)

    image_bytes = browser.get_screenshot_as_png()

    browser.quit()

    return HttpResponse(image_bytes, content_type="image/png")