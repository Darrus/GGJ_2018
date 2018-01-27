using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayBGM("GGJ18 - Nomad MX_1");
	}

    void OnDestroy()
    {
        SoundManager.Instance.StopAllSounds();
    }
}
