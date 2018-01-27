using UnityEngine;

public class Player : Units {
    private void Awake()
    {
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Select(GameObject go)
    {
        if(go == this.gameObject)
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
        //BaseObject baseObject = go.GetComponent<BaseObject>();
        //if (baseObject != null)
        //{
        //    switch (baseObject.objectType)
        //    {
        //        case OBJECT_TYPE.RESOURCE:
        //            break;
        //        case OBJECT_TYPE.UNITS:
        //            target = baseObject;
        //            break;
        //    }
        //}
        //else
            MoveTo(go.transform.position);
    }
}
