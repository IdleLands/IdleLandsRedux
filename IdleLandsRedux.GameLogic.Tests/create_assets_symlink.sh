#!/bin/bash
if [ ! -d "bin/Debug/assets" ]; then
  ln -sv ../../../assets bin/Debug/assets
  exit $?
else
  exit 0
fi
