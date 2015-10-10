/**
  * Test to see if hooks work
  */

function Archer1_OnStaticBonus(basePlayerObject) {
	log('Archer1_OnStaticBonus: ');
	var bonusObject = GetDefaultBonusObject();
	bonusObject.HitPoints.Value = 70 + basePlayerObject.Stats.Level.Current * 10 + basePlayerObject.Stats.Constitution.Current * 6
	bonusObject.MagicPoints.Value = 30 + basePlayerObject.Stats.Level.Current * 2 + basePlayerObject.Stats.Intelligence.Current
	return bonusObject;
}

function Archer1_OnDependentBonus(playerObject, cumulativeStatsObject) {
	log('Archer1_OnDependentBonus: ');
	return GetDefaultBonusObject();
}

function Archer1_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
	log('Archer1_OnOverrulingBonus: ');
	return GetDefaultBonusObject();
}

function Archer1_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Archer1_OnShouldModifyStaticBonusScriptFor: ' + script);
	if(script == "Brave1") {
		var obj = GetDefaultMultiplyBonusObject();
		obj.HitPoints.Value = 2;
		return obj;
	}
	return GetDefaultMultiplyBonusObject();
}

function Archer1_OnShouldModifyDependentBonusScriptFor(script, basePlayerObject) {
	log('Archer1_OnShouldModifyDependentBonusScriptFor: ' + script);
	return GetDefaultMultiplyBonusObject();
}