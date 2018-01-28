using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBehaviour : MonoBehaviour {
    public Image m_LoadingImg;
    public float m_IncreaseAlphaSpeed = 2.0f;

    private void OnEnable()
    {
        SubscriptionSystem.Instance.SubscribeEvent("Finished Loading", BeginUnload);
    }

    void OnDisable()
    {
        SubscriptionSystem.Instance.UnsubscribeEvent("Finished Loading", BeginUnload);
    }

    // Use this for initialization
    IEnumerator Start () {
		if (!m_LoadingImg)
        {
            m_LoadingImg = GetComponent<Image>();
        }
        while (m_LoadingImg.color.a < 1.0f)
        {
            Color newLoadingColor = m_LoadingImg.color;
            newLoadingColor.a += Time.deltaTime * m_IncreaseAlphaSpeed;
            m_LoadingImg.color = newLoadingColor;
            yield return null;
        }
        SubscriptionSystem.Instance.TriggerEvent("Started Loading");
        yield break;
	}

    void BeginUnload()
    {
        StartCoroutine(UnloadingUpdate());
    }

    IEnumerator UnloadingUpdate()
    {
        while (m_LoadingImg.color.a > 0)
        {
            Color newLoadingColor = m_LoadingImg.color;
            newLoadingColor.a -= Time.deltaTime * m_IncreaseAlphaSpeed;
            m_LoadingImg.color = newLoadingColor;
            yield return null;
        }
        SceneManager.UnloadSceneAsync("Loading Screen");
        yield break;
    }
}
