using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour {
    public void ReloadSceneButton()
    {
        SubscriptionSystem.Instance.TriggerEvent("Scene Change");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
