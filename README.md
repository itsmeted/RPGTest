# RPGTest

Some assumptions here... we randomly choose actions both for enemies and player agents.
There is no AI..

Lower speed value actually means faster attacking. It specifies how many game ticks to wait
until attacking again.  A speed value of 0 still means one
update before starting the next action (actions don't trigger immediately one after another).
Health range is from 0 to 100.

The calculation for damage is:
(sourceAgent.Attack - targetAgent.Defense) / 10.0f * strength of attack action.
If Attack is less than the Defense then no health is subtracted

- If sourceAgent Attack is 10, and targetAgent Defense is 20, then the attack will do nothing.
- If sourceAgent Attack is 20, and targetAgent Defense is 20, then the attack will do nothing.
- If sourceAgent Attack is 20, and targetAgent Defense is 10, then the attack will do the attack action healthdelta change
- If sourceAgent Attack is 30, and targetAgent Defense is 10, then the attack will do twice the attack action healthdelta change

Actions are specified on the RPGTestConfiguration component on the RPGTestGame.

Every action has a duration.  A duration of 0 for an action means it triggers immediately and then the
speed delay is triggered to wait until another action is triggered.

Only 100 actions are shown in the actions list in the game due to slowdown when the list gets too large... ideally this would be a recycle list instead.
I did not spend much time tuning this except to make sure that it finishes most of the time instead of neverending simulation.
