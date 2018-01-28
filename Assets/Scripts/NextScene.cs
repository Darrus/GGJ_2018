using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        SceneManagerHelper.Instance.TransitWithLoading(sceneName);
        SubscriptionSystem.Instance.TriggerEvent("Scene Change");
    }
}
