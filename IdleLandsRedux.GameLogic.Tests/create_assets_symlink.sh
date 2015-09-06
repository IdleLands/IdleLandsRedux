#!/bin/bash
if [ -d "bin/Debug" ]; then
  if [ ! -d "bin/Debug/assets" ]; then
    ln -sv ../../../assets bin/Debug/assets
  fi
fi

if [ -d "bin/Release" ]; then
  if [ ! -d "bin/Release/assets" ]; then
    ln -sv ../../../assets bin/Release/assets
  fi
fi

exit 0
