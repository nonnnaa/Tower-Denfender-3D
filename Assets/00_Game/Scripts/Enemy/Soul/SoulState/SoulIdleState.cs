using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulIdleState : FSMState
{
    private SoulControl soulControl;

    public SoulIdleState(SoulControl soulControl)
    {
        this.soulControl = soulControl;
    }
    public override void EnterState()
    {
        base.EnterState();
    }
}
