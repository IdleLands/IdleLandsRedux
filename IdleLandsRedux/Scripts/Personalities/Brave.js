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

function fleePercent() {
	return -100;
}

function strPercent() {
	return 5;
}

function combatEndXpLoss(player, baseCombatEndXpLoss) {
	return parseInt(baseCombatEndXpLoss)*0.1;
}

function canUse(player) {
	return player.statistics["combat self flee"] > 0;
}

function desc() {
	return "Flee combat once";
}