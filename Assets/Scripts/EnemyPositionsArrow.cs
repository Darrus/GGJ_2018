using UnityEngine;
using System.Collections.Generic;

public class EnemyPositionsArrow : MonoBehaviour
{
    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    GameObject player;

    List<GameObject> arrows = new List<GameObject>();

    private void Update()
    {
        
    }
}
