using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillState : State
{
    public StateManager sm;
    public override State RunCurrentState()
    {
        return this;
    }
}
