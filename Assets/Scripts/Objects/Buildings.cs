using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : BaseObject
{
    protected override void Awake()
    {
        base.Awake();
        objectType = OBJECT_TYPE.BUILDING;
    }
}
