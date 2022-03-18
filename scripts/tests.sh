#!/usr/bin/env bash

./scripts/website-up.sh
# TODO: Get puppeteer script working inside k8s container
# docker build -f ./acceptance-tests/Dockerfile -t acceptance-tests ./acceptance-tests

# TODO: Get puppeteer script working inside k8s container
# docker run acceptance-tests
HEADLESS_MODE=false LANDING_PAGE="http://localhost:8000" npm --prefix ./acceptance-tests test

docker stop "$(docker ps --filter label=app=static --format "{{ .ID }}")"
