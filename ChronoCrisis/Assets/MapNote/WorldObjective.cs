// World 1 : Defeat 69 enemy
// world 2 : find info(shop) (available in loop > 5, bayar coin)
// world 3 : find broken staff
// world 4 : find key door to dungeon (cari npc)
// world 5 : find Guide and choose boss room after grinding


// scaling level : +5 stats point / Level
// INT SCALING : for every increase in INT, maxMP += 5 , magicPower += 2.5
// AGI SCALING : movementSPD = baseSPD + baseSPD*agi/200 , atkSPD = baseAtkSPD + baseAtkSPD*agi/200
// VIT SCALING : for every increase in VIT, HP += 5, def += 1

// LEVEL SCALING MILESTONE :
// baseMilestone = 100
// nextMilestone = baseMilestone
// nextMilestone += 15*(level-1)

// lvl 1 100
// lvl 2 115
// lvl 3 145
// lvl 4 190
// lvl 5 250

// EXP given to player
// baseExp = 25
// newExp = baseExp + 5*loopCount

// loop 0 = 25
// loop 1 = 30
// loop 2 = 35
// loop 3 = 40

//coin drop
// baseDrop = 1-2 (random)
// newBaseDrop = baseDrop + loopCount + 2*(worldLevel - 1)

// world 1 = 1-2 + loopCount
// world 2 = 3-4 + loopCount
// world 3 = 5-6 + loopCount