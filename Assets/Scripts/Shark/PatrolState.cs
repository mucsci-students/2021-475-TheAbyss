using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public StateManager sm;
    public ChaseState chaseState;
    public MoveTowardsPointState moveTowardsPointState;
    public RunAwayState runAwayState;

    public override State RunCurrentState()
    {
        if(sm.canSeePlayer)
        {
            return chaseState;
        }
        else if(!sm.hasArrivedAtPoint)
        {
            return moveTowardsPointState;
        }
        else if(sm.isAfraidOfPlayer)
        {
            return runAwayState;
        }
        else
        {
            return this;
        }
    }

}
