using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

#if UNITY_EDITOR
    [SerializeField] bool m_DestroyIt = false;
    [SerializeField] bool m_RestoreFullHealth = false;

    private void Update()
    {
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
    }
#endif

    protected override void Awake()
    {
        base.Awake();
        objectType = OBJECT_TYPE.BUILDING;
        m_OriginalMaxHP = maxHealth;
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

    public abstract bool UpgradeBuilding();
}
