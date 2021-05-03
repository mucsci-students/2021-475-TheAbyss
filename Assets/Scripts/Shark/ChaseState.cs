using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public StateManager sm;
    public KillState killState;

    public MoveTowardsPointState moveTowardsPointState;

    public RunAwayState runAwayState;

    public override State RunCurrentState()
    {
        if(sm.canAttackPlayer)
        {
            return killState;
        }
        else if(!sm.canSeePlayer)
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
