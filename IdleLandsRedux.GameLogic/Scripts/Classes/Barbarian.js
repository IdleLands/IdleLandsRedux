/**
  * This class is a physically powerful class. Their Rage is a powerful force that drastically
  * increases their STR. Rage is accumulated by taking damage and watching allies die. Each point of rage
  * adds to the Barbarians damage multiplier. They have an overall reduction in dex and agi to make up for their
  * bulky nature. They also take half damage on physical attacks.
  *
  * @name Barbarian
  * @special Rage (The Barbarian gets Rage so they can power themselves up while they get beaten up.)
  * @physical
  * @tank
  * @hp 200+[level*25]+[con*10]
  * @mp 0+[level*-10]+[int*-5]
  * @itemScore con*2 + str*2 - wis - int
  * @statPerLevel {str} 6
  * @statPerLevel {dex} 2
  * @statPerLevel {con} 6
  * @statPerLevel {int} -3
  * @statPerLevel {wis} -3
  * @statPerLevel {agi} 0
  * @minDamage 40%
  * @hpregen 5%
  * @category Classes
  * @package Player
*/


function Barbarian_OnStaticBonus(basePlayerObject) {
	log('Barbarian_OnStaticBonus: ');
	var bonusObject = GetDefaultBonusObject();
	bonusObject.StaticHitPoints = 200 + basePlayerObject.Stats.Level.Current * 25 + basePlayerObject.Stats.Constitution.Current * 10
	bonusObject.StaticMagicPoints = 0 + basePlayerObject.Stats.Level.Current * -10 + basePlayerObject.Stats.Intelligence.Current * -5
	bonusObject.PercentageMinimalDamage = 40;
	bonusObject.PercentageHpRegen = 5;
	return bonusObject;
}

function Barbarian_OnDependentBonus(playerObject, cumulativeStatsObject) {
    log('Barbarian_OnDependentBonus: ');
	return GetDefaultBonusObject();
}

function Barbarian_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
	log('Barbarian_OnOverrulingBonus: ');
	return GetDefaultBonusObject();
}

function Barbarian_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Barbarian_OnShouldModifyStaticBonusScriptFor: ' + script);
	return null;
}

function Barbarian_OnShouldModifyDependentBonusScriptFor(script, basePlayerObject) {
	log('Barbarian_OnShouldModifyDependentBonusScriptFor: ' + script);
	return GetDefaultMultiplyBonusObject();
}