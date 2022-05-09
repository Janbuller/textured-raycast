#!/bin/bash

for door in *png ; do
    mkdir -p "../${door/.png}"
    for wall in ../ownwalls/*png ; do
	WALL_FILENAME="${wall/..\/}";
	WALL_FILENAME="${WALL_FILENAME/ownwalls\/}"
	magick "$wall" "$door" -gravity center -composite "../${door/.png}/${WALL_FILENAME/.png}-${door/.png}.png";
    done
done
