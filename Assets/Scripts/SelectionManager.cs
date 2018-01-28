using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField]
    GameObject playerSelect;
    [SerializeField]
    GameObject enemySelect;

    void Awake()
    {
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("SelectPlayer", SelectPlayer);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("SelectBuilding", SelectBuilding);
        SubscriptionSystem.Instance.SubscribeEvent("DeselectPlayer", DeselectPlayer);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("SelectEnemy", SelectEnemy);
        SubscriptionSystem.Instance.SubscribeEvent("DeselectEnemy", DeselectEnemy);
    }

    void SelectPlayer(GameObject go)
    {
        playerSelect.transform.SetParent(go.transform);
        playerSelect.transform.localPosition = new Vector3();
        playerSelect.gameObject.SetActive(true);
        Units unit = go.GetComponent<Units>();
        if(unit.target != null)
        {
            SubscriptionSystem.Instance.TriggerEvent<GameObject>("SelectEnemy", unit.target.gameObject);
        }
    }

    void SelectBuilding(GameObject go)
    {
        playerSelect.transform.SetParent(go.transform);
        playerSelect.transform.localPosition = new Vector3(1.25f, 1.25f);
        playerSelect.gameObject.SetActive(true);
    }

    void DeselectPlayer()
    {
        playerSelect.transform.SetParent(transform);
        playerSelect.gameObject.SetActive(false);
        SubscriptionSystem.Instance.TriggerEvent("DeselectEnemy");
    }

    void SelectEnemy(GameObject go)
    {
        enemySelect.transform.SetParent(go.transform);
        enemySelect.transform.localPosition = new Vector3();
        enemySelect.gameObject.SetActive(true);
    }

    void DeselectEnemy()
    {
        enemySelect.transform.SetParent(transform);
        enemySelect.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("SelectPlayer", SelectPlayer);
        SubscriptionSystem.Instance.SubscribeEvent("DeselectPlayer", DeselectPlayer);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("SelectEnemy", SelectEnemy);
        SubscriptionSystem.Instance.SubscribeEvent("DeselectEnemy", DeselectEnemy);
    }

    void OnDisable()
    {
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("SelectPlayer", SelectPlayer);
        SubscriptionSystem.Instance.UnsubscribeEvent("DeselectPlayer", DeselectPlayer);
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("SelectEnemy", SelectEnemy);
        SubscriptionSystem.Instance.UnsubscribeEvent("DeselectEnemy", DeselectEnemy);
    }
}
