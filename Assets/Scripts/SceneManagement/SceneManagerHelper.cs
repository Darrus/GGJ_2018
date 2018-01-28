using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerHelper : MonoBehaviour {

    static SceneManagerHelper m_Instance;
    public static SceneManagerHelper Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject sceneManagerGO = new GameObject();
                sceneManagerGO.AddComponent<SceneManagerHelper>();
            }
            return m_Instance;
        }
    }

    [SerializeField] string m_SceneToLoad;

    private void Awake()
    {
        if (!m_Instance)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitWithLoading(string _sceneName)
    {
        m_SceneToLoad = _sceneName;
        SceneManager.LoadSceneAsync("Loading Screen", LoadSceneMode.Additive);
        SubscriptionSystem.Instance.SubscribeEvent("Started Loading", StartedLoading);
    }

    void StartedLoading()
    {
        SubscriptionSystem.Instance.UnsubscribeEvent("Started Loading", StartedLoading);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
        StartCoroutine(BeginLoadingOtherScene(m_SceneToLoad));
    }

    IEnumerator BeginLoadingOtherScene(string _sceneName)
    {
        yield return SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
        SubscriptionSystem.Instance.TriggerEvent("Finished Loading");
        yield break;
    }
}
