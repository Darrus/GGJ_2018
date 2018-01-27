using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement : Buildings
{
    public override bool UpgradeBuilding()
    {
        // make sure it is upgradable
        if (m_StateOfBuilding == STATE_OF_BUILDING.COMPLETE && m_CurrentUpgradeIndex + 1 < m_ListOfUpgrades.Count)
        {
            BuildingManager.Instance.SetBuildingTile(m_ListOfUpgrades[m_CurrentUpgradeIndex].m_UpgradedTile, transform.position);
            // need to check amount of resources and what sprites to be used!
            m_ListOfUpgrades.RemoveAt(0);
        }
        return false;
    }
}
