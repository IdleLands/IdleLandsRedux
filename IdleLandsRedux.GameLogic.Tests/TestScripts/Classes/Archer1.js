/**
  * Test to see if hooks work
  */

function Archer1_OnStaticBonus(basePlayerObject) {
	log('Archer1_OnStaticBonus: ');
	var bonusObject = GetStaticBonusObject();
	bonusObject.StaticHitPoints = 70 + basePlayerObject.Stats.Level.Current * 10 + basePlayerObject.Stats.Constitution.Current * 6
	bonusObject.StaticMagicPoints = 30 + basePlayerObject.Stats.Level.Current * 2 + basePlayerObject.Stats.Intelligence.Current
	return bonusObject;
}

function Archer1_OnDependentBonus(playerObject, cumulativeStatsObject) {
}

function Archer1_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
}

function Archer1_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Archer1_OnShouldModifyStaticBonusScriptFor: ' + script);
	if(script == "Brave1") {
		var obj = GetDefaultMultiplyOtherScriptBonusObject();
		obj.StaticHitPoints = 2;
		return obj;
	}
	return null;
}