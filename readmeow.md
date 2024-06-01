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

