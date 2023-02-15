using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayer : BasicEntity
{
    protected enum ANIMSTATE
    {
        IDLE,
        WALK,
        JUMP,
        ATTACK,

    }
    protected override void OnDeath()
    {
        
    }
}
