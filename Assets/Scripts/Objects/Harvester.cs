using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : Units
{
    protected override void Awake()
    {
        base.Awake();
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Select(GameObject go)
    {
        if (go == this.gameObject)
        {
            Debug.Log(go);
            SubscriptionSystem.Instance.UnsubcribeEvent<GameObject>("LeftClick", Select);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", InteractSelected);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("RightClick", Deselect);
        }
    }

    void Deselect(GameObject go)
    {
        SubscriptionSystem.Instance.UnsubcribeEvent<GameObject>("LeftClick", InteractSelected);
        SubscriptionSystem.Instance.UnsubcribeEvent<GameObject>("RightClick", Deselect);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void InteractSelected(GameObject go)
    {
        BaseObject baseObject = go.GetComponent<BaseObject>();
        if (baseObject != null)
        {
            switch (baseObject.objectType)
            {
                case OBJECT_TYPE.RESOURCE:
                    AttackTarget(baseObject);
                    state = UNIT_STATE.ATTACK;
                    break;
                case OBJECT_TYPE.UNITS:
                    break;
            }
        }
        else
            MoveTo(go.transform.position);
    }
}
