using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArrowSystem : MonoBehaviour {
    [Header("Variables required")]
    [SerializeField]
    protected GameObject m_ArrowUnitPrefab;

    private void OnEnable()
    {
        SubscriptionSystem.Instance.SubscribeEvent<Transform>("SpawnArrow", SpawnUnitArrow);
    }

    private void OnDisable()
    {
        SubscriptionSystem.Instance.UnsubscribeEvent<Transform>("SpawnArrow", SpawnUnitArrow);
    }

    protected void SpawnUnitArrow(Transform _trackedUnitTransform)
    {
        ArrowPointToUnit arrowPointer = Instantiate(m_ArrowUnitPrefab, transform, false).GetComponent<ArrowPointToUnit>();
        arrowPointer.TargetTransform = _trackedUnitTransform;
    }
}
