#!/bin/bash

for door in *png ; do
    mkdir -p "${door/.png}"
    for wall in ../*png ; do
	WALL_FILENAME="${wall/..\/}";
	magick "$wall" "$door" -gravity center -composite "${door/.png}/${WALL_FILENAME/.png}-${door/.png}.png";
    done
done
