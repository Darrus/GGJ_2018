﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour {
    [SerializeField]
    GameObject fullBar;
    [SerializeField]
    TextMeshPro textMesh;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateHealthBar(float health, float maxHealth)
    {
        float ratio = health / maxHealth;
        ratio = 1.0f - ratio;
        Vector3 position = new Vector3(-spriteRenderer.size.x * ratio, 0.0f);
        fullBar.transform.localPosition = position;
    }

    public void UpdateGather(int gather, int maxGather)
    {
        textMesh.text = gather.ToString() + "/" + maxGather.ToString();
    }
}