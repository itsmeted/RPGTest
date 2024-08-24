# RPGTest

Some assumptions here... we randomly choose actions both for enemies and player agents.
There is no AI..

Lower speed value actually means faster attacking.  It specifies how many game ticks to wait
until attacking again.
Health range is from 0 to 100.

The calculation for damage is:
(sourceAgent.Attack - targetAgent.Defense) / 10.0f * strength of attack action...

If sourceAgent Attack is 10, and targetAgent Defense is 20, then the attack will do nothing.
If sourceAgent Attack is 20, and targetAgent Defense is 20, then the attack will do nothing.
If sourceAgent Attack is 20, and targetAgent Defense is 10, then the attack will do the attack action healthdelta change
If sourceAgent Attack is 30, and targetAgent Defense is 10, then the attack will do twice the attack action healthdelta change

Actions are specified on the RPGTestConfiguration component on the RPGTestGame.

There are some hardcoded ranges for various things...
Health range is from 0 to 100.
