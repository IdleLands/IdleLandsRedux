/**
  * The Archer is a physical debuff/dps class. Their Focus stat increases critical chance by up to 50%,
  * and allows access to powerful skills. Focus increases every turn and with use of the Take Aim skill,
  * and decreases when the Archer is damaged as well as when it is spent.
  *
  * @name Archer
  * @physical
  * @dps
  * @hp 70+[level*10]+[con*6]
  * @mp 30+[level*2]+[int*1]
  * @special Focus (The Archer builds focus over time, resulting in devastating attacks if unchecked.)
  * @itemScore agi*1.2 + dex*1.6 + str*0.3 - int
  * @statPerLevel {str} 2
  * @statPerLevel {dex} 4
  * @statPerLevel {con} 2
  * @statPerLevel {int} 1
  * @statPerLevel {wis} 1
  * @statPerLevel {agi} 3
  * @minDamage 50%
  * @category Classes
  * @package Player
*/