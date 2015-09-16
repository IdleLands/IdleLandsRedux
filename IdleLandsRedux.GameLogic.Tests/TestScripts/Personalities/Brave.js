/**
  * Test to see if calling functions with arguments works.
  */

function Brave_fleePercent() {
       return -100;
}

function Brave_strPercent() {
       return 5;
}

function Brave_canUse(player) {
       return player.statistics["combat self flee"] > 0;
}
