# COMP313-A2

Basic outline of game:  
* Maze-like level which the player has to reach the end of
* Patrolling bots with cone of light representing field of vision
* If the player enters the cone of light, the bots shoot the player. They will also stop patrolling and begin to follow the user (quite slowly)
- If you sneak behind a bot you can disable it.
- Potentially some kind of trail that the player leaves behind for the bots to follow  
This boils down to these basic game mechanics for the player:
- Movement (walk, run, crouch, jump)
- Stealth (avoiding the cone of light)
- Attack (disabling bots from behind)
