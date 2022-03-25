#!/usr/bin/env bash

./scripts/website-up.sh
# TODO: Get puppeteer script working inside k8s container
# docker build -f ./acceptance-tests/Dockerfile -t acceptance-tests ./acceptance-tests

# TODO: Get puppeteer script working inside k8s container
# docker run acceptance-tests
HEADLESS_MODE=false LANDING_PAGE="http://localhost:8888" GAME_URL="http://localhost:8888/WebGL/index.html" npm --prefix ./acceptance-tests test
# HEADLESS_MODE=false LANDING_PAGE="https://trashpickuphobby.com" GAME_URL="https://trashpickuphobby.com/WebGL/index.html" npm --prefix ./acceptance-tests test

./scripts/website-down.sh
