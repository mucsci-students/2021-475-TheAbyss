using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : State
{
    public StateManager sm;

    public MoveTowardsPointState moveTowardsPointState;

    public override State RunCurrentState()
    {
        if(!sm.isAfraidOfPlayer)
        {
            sm.hasArrivedAtPoint = false;
            return moveTowardsPointState;
        }
        else
        {
            return this;
        }
    }
}
