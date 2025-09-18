using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDeadState : FSMState
{
    private SoulControl soulControl;

    public SoulDeadState(SoulControl soulControl)
    {
        this.soulControl = soulControl;
    }
}
