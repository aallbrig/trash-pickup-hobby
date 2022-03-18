#!/usr/bin/env bash

docker build -f ./src/Dockerfile -t html-static-webserver ./src
# TODO: Get puppeteer script working inside k8s container
# docker build -f ./acceptance-tests/Dockerfile -t acceptance-tests ./acceptance-tests

docker run -l app=static -d -p 80:8000 html-static-webserver

# TODO: Get puppeteer script working inside k8s container
# docker run acceptance-tests
HEADLESS_MODE=false LANDING_PAGE="http://localhost:8000" npm --prefix ./acceptance-tests test

docker stop "$(docker ps --filter label=app=static --format "{{ .ID }}")"
