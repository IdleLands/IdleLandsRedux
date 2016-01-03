#!/bin/bash
GENDARME_EXE="./gendarme/gendarme.exe"

if [ ! -e "$GENDARME_EXE" ]; then
  GENDARME_EXE="../gendarme/gendarme.exe"
  if [ ! -e "$GENDARME_EXE"  ]; then
    echo "Gendarme not found."
    exit 1
  fi
fi

echo "Gendarme found at $GENDARME_EXE"

mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.Common/bin/Debug/IdleLandsRedux.Common.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.Common.Tests/bin/Debug/IdleLandsRedux.Common.Tests.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.Contracts/bin/Debug/IdleLandsRedux.Contracts.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.Contracts.Tests/bin/Debug/IdleLandsRedux.Contracts.Tests.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.Core/bin/Debug/IdleLandsRedux.exe
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.DataAccess/bin/Debug/IdleLandsRedux.DataAccess.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.GameLogic/bin/Debug/IdleLandsRedux.GameLogic.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.GameLogic.DataEntities/bin/Debug/IdleLandsRedux.GameLogic.DataEntities.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.GameLogic.DataEntities.Tests/bin/Debug/IdleLandsRedux.GameLogic.DataEntities.Tests.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.GameLogic.Tests/bin/Debug/IdleLandsRedux.GameLogic.Tests.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.InteropPlugins/bin/Debug/IdleLandsRedux.InteropPlugins.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.InteropPlugins.Tests/bin/Debug/IdleLandsRedux.InteropPlugins.Tests.dll
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.WebService/bin/Debug/IdleLandsRedux.WebService.exe
mono $GENDARME_EXE -config ./rules.xml IdleLandsRedux.WebService.Tests/bin/Debug/IdleLandsRedux.WebService.Tests.dll

exit 0
