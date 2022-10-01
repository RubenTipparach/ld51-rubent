LD 51 tools 

Engine:
Unity
Visual studio


Art: 
Blender
Aseprite

Music: https://www.beepbox.co/#9n31s0k0l00e0ft2ma7g0fj07r3i0o432T3v0uaef0q0x10p71d23Sp99f9c9Vppbaa9gE1b9T0v1ug3f0q0y10l52d39w5h5E1b5T1v1u73f10o9q011d03A3F0B2Q5d01Pc52cE3619622636T2v1u15f10w4qw02d03w0E0b4h4h4h4h00014h4h4h4h4g014h40004g4h4h4h4h4h4p21tKr5RFUCzU2pJj5o6r5QVjsvS0FEYcPhZllellARlellARlellARlek0FEY5h0QlM97bQmQ4tBxu0AqqcOwLFduwLFE8M0

https://onlinesequencer.net/#t1

more tools maybe: 
https://github.com/kobitoko/Game-Jam-Tools-Resources



Game Design Ideas:

Card game with each turn is 10 seconds?

Space game where each interval is 10 seconds
    - plan battles inbetween each turn, fast forward enabled.
    - ships have different hull facing
    - ships also have shields

Card game about collecting spaceships...
... or weapons on board a ship
    - how do weapons fire? 10 seconds will be an interval for all power to recharge?



Game name: SCAVERS

Game Design
    - Camera: r/f up and down
        - Free movement areound a pivot
        - lock on a ship
    - Ship movement
        - yaw, pitch roll
        - translational movement only.
    - 10 second moanuver intervals
        - show ship time slices.
        - each manuever allows for limited travel distance
        - each manuever allows for limited rotation of ship.
        - each second time slice will aloow a player to plan their weapons firing.
    - Progression: aim for 4-5 levels
        - each level the fleet is persistent
        - TODO: save fleet data in user prefs for laziness
        - allow user to reset progress
        - replaying a mission allows you to use your current fleet progress
        - disabling a ship allows you to salvage it for future missions.
    - ships have hull stuff 
        - weapon types:
            - disruptors - damages weapon systems only, can be blocked by obstacles
                - can be fired 3 times
            - lasers - same as missiles 
                - can only be fired once per round.
            - missiles - damages hull and weapons, can track, but are limited, and dont avoid collision
                - launched in barages dpending on ship size
            - torpedos - fires in a straight line, does massive damage to hull, can be obstructed
                - only found on large ships, and can only carry 1 but nuke a large area.
                - can destroy space debris?
        - subsystems
            - shields, recharges to take 1 missile damage or diruptor damage
                - torpedos ignore shields
            - armor, protects all subsystems, takes 1/4 damage of all attack types, disruptors don't affect it.
    - Cover/debris
        - Asteroids
        - station and large freighter derlicts