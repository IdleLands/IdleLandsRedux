/**
  * Test to see if stacked bonusses work.
*/

function Brave1_OnStaticBonus(basePlayerObject) {
	var bonusObject = GetStaticBonusObject();
	bonusObject.PercentageStrength = 5;
	bonusObject.StaticHitPoints = 50;
	return bonusObject;
}

function Brave1_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Brave1_OnShouldModifyStaticBonusScriptFor: ' + script);
	return null;
}