/**
  * Test to see if accessing player stats + returning static bonus works
  */

function Archer_OnStaticBonus(basePlayerObject) {
	log('Archer_OnStaticBonus: ');
	var bonusObject = GetDefaultBonusObject();
	bonusObject.HitPoints.Value = 70 + basePlayerObject.Stats.Level.Current * 10 + basePlayerObject.Stats.Constitution.Current * 6
	bonusObject.MagicPoints.Value = 30 + basePlayerObject.Stats.Level.Current * 2 + basePlayerObject.Stats.Intelligence.Current
	return bonusObject;
}

function Archer_OnDependentBonus(playerObject, cumulativeStatsObject) {
    log('Archer_OnDependentBonus: ');
	return GetDefaultBonusObject();
}

function Archer_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
	log('Archer_OnOverrulingBonus: ');
	return GetDefaultBonusObject();
}

function Archer_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Archer_OnShouldModifyStaticBonusScriptFor: ' + script);
	return null;
}

function Archer_OnShouldModifyDependentBonusScriptFor(script, basePlayerObject) {
	log('Archer_OnShouldModifyDependentBonusScriptFor: ' + script);
	return GetDefaultMultiplyBonusObject();
}