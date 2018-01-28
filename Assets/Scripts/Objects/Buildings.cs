using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public abstract class Buildings : BaseObject
{
    [System.Serializable]
    public class UpgradeRequirement
    {
        public ResourceObject.OBJECT_TYPE m_RequiredType;
        public int m_AmountOfResource;
        public Tile m_UpgradedTile;
    }
    [SerializeField, Tooltip("List of Upgrade Requirement")]
    protected List<UpgradeRequirement> m_ListOfUpgrades = new List<UpgradeRequirement>();
    [SerializeField] protected int m_CurrentUpgradeIndex = 0;
    public enum STATE_OF_BUILDING
    {
        COMPLETE,
        RUINED,
        TOTAL_STATE,
    }
    [SerializeField, Tooltip("Ruined Sprite")]
    protected Tile m_RuinedBuildingTile;
    [SerializeField, Tooltip("State of building")]
    protected STATE_OF_BUILDING m_StateOfBuilding = STATE_OF_BUILDING.COMPLETE;
    [SerializeField] int m_OriginalMaxHP;
    public bool isRuined = false;

    [SerializeField]
    GameObject repairSymbol;
    [SerializeField]
    TextMeshPro textMesh;

    public float repairDuration;
    public int repairCost;
    float repairTimer;
    bool repair = false;

#if UNITY_EDITOR
    [SerializeField] bool m_DestroyIt = false;
    [SerializeField] bool m_RestoreFullHealth = false;
#endif

    protected override void Awake()
    {
        base.Awake();
        objectType = OBJECT_TYPE.BUILDING;
        m_OriginalMaxHP = maxHealth;
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
        repairSymbol.transform.position = new Vector3(1.2f, 1.7f);
        repairSymbol.gameObject.SetActive(false);
    }

    void Update()
    {
 #if UNITY_EDITOR
        if (m_DestroyIt)
        {
            TakeDamage(maxHealth);
            m_DestroyIt = false;
        }
        if (m_RestoreFullHealth)
        {
            RestoreHealth(maxHealth);
            m_RestoreFullHealth = false;
        }
#endif
        if (repair)
        {
            repairTimer -= Time.deltaTime;
            textMesh.text = ((int)repairTimer).ToString();
            if(repairTimer <= 0.0f)
            {
                RestoreHealth(m_OriginalMaxHP);
                repair = false;
            }
        }
    }

    protected override void OnDestroy()
    {
        //base.OnDestroy();
        //BuildingManager.Instance.RemoveBuildingTile(transform.position);
    }

    public override void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health == 0 && m_StateOfBuilding != STATE_OF_BUILDING.RUINED)
        {
            m_CurrentUpgradeIndex = 0;
            m_StateOfBuilding = STATE_OF_BUILDING.RUINED;
            BuildingManager.Instance.SetBuildingTile(m_RuinedBuildingTile, transform.position);
            maxHealth = m_OriginalMaxHP;
            ObjectManager.Instance.RemoveObject(objectType, this);
            isRuined = true;
            // trigger that it is being destroyed
            SubscriptionSystem.Instance.TriggerEvent<BaseObject.OBJECT_TYPE>("HouseDestroyed", objectType);
        }
    }

    public override void RestoreHealth(int amount)
    {
        base.RestoreHealth(amount);
        if (m_StateOfBuilding == STATE_OF_BUILDING.RUINED)
        {
            m_StateOfBuilding = STATE_OF_BUILDING.COMPLETE;
            BuildingManager.Instance.SetBuildingTile(m_ListOfUpgrades[m_CurrentUpgradeIndex].m_UpgradedTile, transform.position);
        }
    }

    void Select(GameObject go)
    {
        if(go == this.gameObject)
        {
            SubscriptionSystem.Instance.TriggerEvent<GameObject>("SelectBuilding", this.gameObject);
            SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", Select);
            if (m_StateOfBuilding == STATE_OF_BUILDING.RUINED)
            {
                SubscriptionSystem.Instance.SubscribeEvent("Repair", Repair);
            }
        }
    }

    void Repair()
    {
        if(ResourceManager.Instance.GetResourceCount(ResourceManager.RESOURCE_TYPE.WOOD) > repairCost && !repair)
        {
            SoundManager.Instance.PlaySFX("NomadStrategyRepairBuilding");
            repairSymbol.gameObject.SetActive(true);
            ResourceManager.Instance.UseResource(ResourceManager.RESOURCE_TYPE.WOOD, repairCost);
            repair = true;
            repairTimer = repairDuration;
            SubscriptionSystem.Instance.UnsubscribeEvent("Repair", Repair);
        }
    }

    public abstract bool UpgradeBuilding();
}
