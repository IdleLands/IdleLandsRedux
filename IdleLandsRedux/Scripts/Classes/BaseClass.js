  var baseHp = 10
  var baseHpPerLevel = 15
  var baseHpPerCon = 5
  var baseHpPerInt = 0
  var baseHpPerDex = 0
  var baseHpPerWis = 0
  var baseHpPerStr = 0
  var baseHpPerAgi = 0
  var baseHpPerLuck = 0

  var baseMp = 5
  var baseMpPerLevel = 2
  var baseMpPerInt = 3
  var baseMpPerCon = 0
  var baseMpPerDex = 0
  var baseMpPerWis = 0
  var baseMpPerStr = 0
  var baseMpPerAgi = 0
  var baseMpPerLuck = 0

  var battleXpGainPercent = 10

  var baseXpGainPerCombat = 100
  var baseXpGainPerOpponentLevel = 50

  var baseGoldGainPerCombat = 0
  var baseGoldGainPerOpponentLevel = 0

  var baseXpLossPerCombat = 10
  var baseXpLossPerOpponentLevel = 5

  var baseGoldLossPerCombat = 0
  var baseGoldLossPerOpponentLevel = 0

  var baseConPerLevel = 0
  var baseDexPerLevel = 0
  var baseAgiPerLevel = 0
  var baseStrPerLevel = 0
  var baseIntPerLevel = 0
  var baseWisPerLevel = 0
  var baseLuckPerLevel = 0

  var events = {}

  function hp(player) {
    baseHp +
      (baseHpPerLevel * player.level.getValue()) +
      (baseHpPerCon   * player.calc.stat 'con') +
      (baseHpPerDex   * player.calc.stat 'dex') +
      (baseHpPerStr   * player.calc.stat 'str') +
      (baseHpPerWis   * player.calc.stat 'wis') +
      (baseHpPerAgi   * player.calc.stat 'agi') +
      (baseHpPerInt   * player.calc.stat 'int') +
      (baseHpPerLuck  * player.calc.stat 'luck')
  }

  function mp(player) {
    baseMp +
      (baseMpPerLevel * player.level.getValue()) +
      (baseMpPerInt   * player.calc.stat 'int') +
      (baseMpPerCon   * player.calc.stat 'con') +
      (baseMpPerDex   * player.calc.stat 'dex') +
      (baseMpPerStr   * player.calc.stat 'str') +
      (baseMpPerWis   * player.calc.stat 'wis') +
      (baseMpPerAgi   * player.calc.stat 'agi') +
      (baseMpPerLuck  * player.calc.stat 'luck')
  }

  function con(player) {
    baseConPerLevel*player.level.getValue()
  }

  function dex(player) {
    baseDexPerLevel*player.level.getValue()
  }

  function agi(player) {
    baseAgiPerLevel*player.level.getValue()
  }

  function str(player) {
    baseStrPerLevel*player.level.getValue()
  }

  function int(player) {
    baseIntPerLevel*player.level.getValue()
  }

  function wis(player) {
    baseWisPerLevel*player.level.getValue()
  }

  function luck(player) {
    baseLuckPerLevel*player.level.getValue()
  }

  ###
    deadVariables contains:
      deadPlayers
      numDead
      deadPlayerTotalXp
      deadPlayerAverageXp
  ###

  function combatEndXpGain(player, deadVariables) {
    baseXpGainPerCombat + _.reduce (_.pluck (_.pluck deadVariables.deadPlayers, 'level'), '__current'),
      ((prevVal, level) => prevVal + (level * baseXpGainPerOpponentLevel))
    , 0
  }

  function combatEndXpLoss(player, deadVariables) {
    baseXpLossPerCombat + _.reduce (_.pluck (_.pluck deadVariables.winningParty.players, 'level'), '__current'),
      ((prevVal, level) => prevVal + (level * baseXpLossPerOpponentLevel))
    , 0
  }

  function combatEndGoldGain(player, deadVariables) {
    baseGoldGainPerCombat + _.reduce (_.pluck (_.pluck deadVariables.deadPlayers, 'level'), '__current'),
      ((prevVal, level) => prevVal + (level * baseGoldGainPerOpponentLevel))
    , 0
  }

  function combatEndGoldLoss(player, deadVariables) {
    baseGoldLossPerCombat + _.reduce (_.pluck (_.pluck deadVariables.winningParty.players, 'level'), '__current'),
      ((prevVal, level) => prevVal + (level * baseGoldLossPerOpponentLevel))
    , 0
  }

  function eventModifier(event) {
    event.min
  }

