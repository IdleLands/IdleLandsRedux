/**
  * Test to see if accessing player stats + returning static bonus works
  */

function Archer_OnStaticBonus(basePlayerObject) {
	log('Archer_OnStaticBonus: ');
	var bonusObject = GetStaticBonusObject();
	bonusObject.StaticHitPoints = 70 + basePlayerObject.Stats.Level.Current * 10 + basePlayerObject.Stats.Constitution.Current * 6
	bonusObject.StaticMagicPoints = 30 + basePlayerObject.Stats.Level.Current * 2 + basePlayerObject.Stats.Intelligence.Current
	return bonusObject;
}

function Archer_OnDependentBonus(playerObject, cumulativeStatsObject) {
    
}

function Archer_OnOverrulingBonus(playerObject, cumulativeStatsObject) {
}

function Archer_OnShouldModifyStaticBonusScriptFor(script, basePlayerObject) {
	log('Archer_OnShouldModifyStaticBonusScriptFor: ' + script);
	return null;
}