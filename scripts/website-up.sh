#!/usr/bin/env bash

docker build -f ./src/Dockerfile -t html-static-webserver ./src
docker run -l app=static -d -p 80:8000 html-static-webserver
