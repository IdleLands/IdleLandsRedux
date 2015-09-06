function GetStaticBonusObject() {
  var bonusObject = new Object();

  //stats
  bonusObject.staticHitPoints = 0;
  bonusObject.staticMagicPoints = 0;
  bonusObject.staticStrength = 0;
  bonusObject.staticConstitution = 0;
  bonusObject.staticDexterity = 0;
  bonusObject.staticAgility = 0;
  bonusObject.staticIntelligence = 0;
  bonusObject.staticWisdom = 0;
  bonusObject.staticLuck = 0;
  bonusObject.percentageHitPoints = 0;
  bonusObject.percentageMagicPoints = 0;
  bonusObject.percentageStrength = 0;
  bonusObject.percentageConstitution = 0;
  bonusObject.percentageDexterity = 0;
  bonusObject.percentageAgility = 0;
  bonusObject.percentageIntelligence = 0;
  bonusObject.percentageWisdom = 0;
  bonusObject.percentageLuck = 0;

  //various
  bonusObject.staticExperienceGain = 0;
  bonusObject.staticGoldGain = 0;
  bonusObject.staticPhysicalAttackChance = 0;
  bonusObject.staticCriticalChance = 0;
  bonusObject.staticMinimalDamage = 0;

  return bonusObject;
}