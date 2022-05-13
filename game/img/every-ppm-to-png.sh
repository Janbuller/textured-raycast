#!/bin/bash

shopt -s globstar
for i in ./**/*ppm ; do
    magick "$i" -transparent black "${i/.ppm}.png"
done
