#!/usr/bin/env bash

docker build -f ./src/Dockerfile -t html-static-webserver -l./src
docker build -f ./acceptance-tests/Dockerfile -t acceptance-tests ./acceptance-tests

docker run -l app=static -d -p 80:8000 html-static-webserver

docker run acceptance-tests

docker stop "$(docker ps --filter label=app=static --format "{{ .ID }}")"
