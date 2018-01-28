using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayUI : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI steelText;
    [SerializeField]
    TextMeshProUGUI woodText;

    void Update()
    {
        if (ResourceManager.Instance == null)
            return;

        if (player != null)
        {
            healthText.text = player.health.ToString() + "/" + player.maxHealth;
        }

        steelText.text = ResourceManager.Instance.GetResourceCount(ResourceManager.RESOURCE_TYPE.STEEL).ToString();
        woodText.text = ResourceManager.Instance.GetResourceCount(ResourceManager.RESOURCE_TYPE.WOOD).ToString();
    }
}
