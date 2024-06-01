# Thinking


## Event Triggers

### PREREQS
0. Should be attached to object like Barrel, Cup, etc. with area of trigger defined by each object type.
1.  When a player enters the trigger zone of an object, an signal should be sent from the object to the player. The player should then get notified of the event and have a function for its type.
2. The HUD should mimic this behavior, ideally though, this behavior should be dictated by which player controller owns which camera....
    but this is just godot... singleplayer
        
### Espresso Machine
0. On player enter should display a Notification on the PlayerHud and when the interact input button (F) is held/pressed triggers an event.

----

## Core Game Loop

0. There is timer and at 0 the levels ends and goes to a new level.



# Godot GameJam 2024

## Todo List

0. Player Class
    - Movement X
    - Interact 
        a. cup, steamer, shot puller, coffee jar
        b. shrink ray (small) (big)
        c. Item shop
        d. drink drop zone
    - attack (when small) stab stab

1. Customer Class
    - Move (static)
    - 2D bubble order
    - timer
    - reciper/order request
    - state completed/waiting/failed
    - on state change event for giving money

2. Game Loop Class
    - Queue of Customers
    - Rat Horde
    - shop status
    - has player
    - level of wave

3. Rat Horde Class
    - Move (static) from enter point, to hide point.
    - state alive/dead
    - should be formed as an array of rat objects with start locations, and hide locations.

4. Shop Class
    - health score card (at 0 game ends)
    - money amount earned per customer completed event
    - list of upgrades

5. Upgrade Class
    - name
    - description
    - modifier to game object(s)
        a. kill rat
        b. slow rats etc.

## Assets
1. weapon small mode
2. Cups
3. syrup flavor icons
4. syrup bottle colors
5. Milk Steamer

## Game Loop
- Wave starts
    - Take Customer Orders
        1. Little person comes in with icon of order. Have certain amount of time to complete.
        2. Combine ingredients to complete order.
        3. Drop off order. (drop zone)
        (Notes: coffee cup station, player cannot set cup down until drink is finished.)
    - Watch out for rats!
        1. Health bar (HealthScore)
            Depending on % of rats you did not exterminate, healthscore goes down
        2. At 0 healthscore game ends
        3+ Would be nice to show visual effects in shop as score goes down.
    - Kill the rats!
        1. Go to shrink ray, now your small.
        2. poky sharpie stick to stab the rats
        3. special locations to 
    - Upgrade shop
        1. just statically either kills extra rats, adds modifiers (slower)
        2+ show stuff around map change (metal barrels, colored holes etc.)

## MVP
1. Make Drinks -> Money
2. Kill Rats via Shrink -> Stay open
3. Upgrade shop (static) -> easier to kill rats

## Recipes
    - Coffee
    - Late (esspresso + steamed milk [if hot])
    - Capucino (extra time steaming)
    - machiato (shot goes on top)