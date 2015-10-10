/**
  * Test to see if stacked bonusses work.
*/

function Brave1_OnStaticBonus(basePlayerObject) {
	var bonusObject = GetDefaultBonusObject();
	bonusObject.Strength.Percent = 5;
	bonusObject.HitPoints.Value = 50;
	return bonusObject;
}

function Brave1_OnDependentBonus(playerObject, cumulativeStatsObject) {
	log('Brave1_OnDependentBonus: ');
	return GetDefaultBonusObject();
}

function Brave1_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
	log('Brave1_OnOverrulingBonus: ');
	return GetDefaultBonusObject();
}

function Brave1_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Brave1_OnShouldModifyStaticBonusScriptFor: ' + script);
	return GetDefaultMultiplyBonusObject();
}

function Brave1_OnShouldModifyDependentBonusScriptFor(script, basePlayerObject) {
	log('Brave1_OnShouldModifyDependentBonusScriptFor: ' + script);
	return GetDefaultMultiplyBonusObject();
}