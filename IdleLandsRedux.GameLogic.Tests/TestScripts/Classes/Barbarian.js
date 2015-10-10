/**
  * Test to see if the entire chain of stats increasing works.
  */


function Barbarian_OnStaticBonus(basePlayerObject) {
	log('Barbarian_OnStaticBonus: ');
	var bonusObject = GetDefaultBonusObject();
	bonusObject.HitPoints.Value = 200
	bonusObject.MagicPoints.Percent = 20
	return bonusObject;
}

function Barbarian_OnDependentBonus(playerObject, cumulativeStatsObject) {
    log('Barbarian_OnDependentBonus: ' + cumulativeStatsObject.ToString());
	var bonusObject = GetDefaultBonusObject();
	bonusObject.HitPoints.Value = cumulativeStatsObject.HitPoints.Value + 200
	bonusObject.MagicPoints.Percent = cumulativeStatsObject.MagicPoints.Percent + 20
	return bonusObject;
}

function Barbarian_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
	log('Barbarian_OnOverrulingBonus: ' + cumulativeStatsObject.ToString());
	var bonusObject = GetDefaultBonusObject();
	bonusObject.HitPoints.Value = cumulativeStatsObject.HitPoints.Value + 200
	bonusObject.MagicPoints.Percent = cumulativeStatsObject.MagicPoints.Percent * 2
	return bonusObject;
}

function Barbarian_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Barbarian_OnShouldModifyStaticBonusScriptFor: ' + script);
	return null;
}

function Barbarian_OnShouldModifyDependentBonusScriptFor(script, basePlayerObject) {
	log('Barbarian_OnShouldModifyDependentBonusScriptFor: ' + script);
	return GetDefaultMultiplyBonusObject();
}