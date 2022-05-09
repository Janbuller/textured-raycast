#!/bin/bash

shopt -s globstar
for i in ./**/*png ; do
    magick "$i" -compress none -background "#000000" -alpha remove -alpha off "${i/.png}.ppm"
done
