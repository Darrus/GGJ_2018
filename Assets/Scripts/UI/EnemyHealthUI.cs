using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthUI : MonoBehaviour {
    [SerializeField]
    GameObject fullBar;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateHealthBar(float health, float maxHealth)
    {
        float ratio = health / maxHealth;
        ratio = 1.0f - ratio;
        Debug.Log(spriteRenderer.size.x * ratio);
        Vector3 position = new Vector3(-spriteRenderer.size.x * ratio, 0.0f);
        fullBar.transform.localPosition = position;
    }
}
