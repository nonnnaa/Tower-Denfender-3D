using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHitState : FSMState
{
    private SoulControl soulControl;

    public SoulHitState(SoulControl soulControl)
    {
        this.soulControl = soulControl;
    }
}
