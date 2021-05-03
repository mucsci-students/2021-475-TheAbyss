using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPointState : State
{
    public StateManager sm;
    public PatrolState patrolState;
    public RunAwayState runAwayState;

    public ChaseState chaseState;

    public override State RunCurrentState()
    {
        if(sm.hasArrivedAtPoint)
        {
            return patrolState;
        }
        else if(sm.canSeePlayer)
        {
            return chaseState;
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
