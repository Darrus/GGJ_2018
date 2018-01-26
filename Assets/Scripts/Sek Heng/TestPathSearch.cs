using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathSearch : MonoBehaviour {
    [SerializeField]
    protected TileSystem m_TileSystem;
    [SerializeField]
    protected List<TileScript> m_PathFoundList;
    [Header("Put the tile that u want it to find from start to end!")]
    [SerializeField]
    protected TileScript m_StartTile;
    [SerializeField]
    protected TileScript m_EndTile;
    [SerializeField]
    protected bool m_BeginPathFound = false;
	
	// Update is called once per frame
	void Update () {
		if (m_BeginPathFound)
        {
            m_PathFoundList = m_TileSystem.GetFoundPath(m_StartTile, m_EndTile);
            m_BeginPathFound = false;
        }
	}
}
