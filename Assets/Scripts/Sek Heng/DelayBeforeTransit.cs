using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayBeforeTransit : MonoBehaviour {
    public float m_DelayTime = 3.0f;
    public string m_TransitSceneName;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(m_DelayTime);
        SceneManagerHelper.Instance.TransitWithLoading(m_TransitSceneName);
        yield break;
    }
}
