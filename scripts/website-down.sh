#!/usr/bin/env bash

docker stop "$(docker ps --filter label=app=static --format "{{ .ID }}")"
