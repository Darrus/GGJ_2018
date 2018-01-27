using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class ObjectManager : Singleton<ObjectManager>
{
    [SerializeField]
    GameObject[] prefabs;

    List<BaseObject>[] objectList;

    private void Awake()
    {
        objectList = new List<BaseObject>[(int)BaseObject.OBJECT_TYPE.MAX];
        for(int i = 0; i < objectList.Length; ++i)
        {
            objectList[i] = new List<BaseObject>();
        }
    }

    public void AddObject(BaseObject.OBJECT_TYPE type, BaseObject baseObject)
    {
        objectList[(int)type].Add(baseObject);
    }

    public void RemoveObject(BaseObject.OBJECT_TYPE type, BaseObject baseObject)
    {
        objectList[(int)type].Remove(baseObject);
    }

    public List<BaseObject> GetList(BaseObject.OBJECT_TYPE type)
    {
        return objectList[(int)type];
    }

    public void Spawn(BaseObject.OBJECT_TYPE type, Vector3 position)
    {
        GameObject go = Instantiate<GameObject>(prefabs[(int)type]);
        go.transform.SetParent(transform);
        go.transform.position = position;
    }
}
