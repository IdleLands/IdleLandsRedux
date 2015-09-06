/**
  * This personality attempts to prevent you from fleeing from combat.
  *
  * @name Brave
  * @prerequisite Flee combat once
  * @effect -100 fleePercent
  * @effect +5% STR
  * @effect +10% XP loss at end of combat
  * @category Personalities
  * @package Player
*/

function OnStaticBonus(basePlayerObject) {
	var bonusObject = GetStaticBonusObject();

	if(!canUse(basePlayerObject)) {
		return basePlayerObject;
	}

	bonusObject.percentageStrength = 5;
	return bonusObject;
}

function canUse(basePlayerObject) {
	return basePlayerObject.statistics["combat self flee"] > 0;
}
