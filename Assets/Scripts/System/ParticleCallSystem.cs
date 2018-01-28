using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Based on the game event trigger system, it will play the particle system when it is requested!
/// </summary>
public class ParticleCallSystem : MonoBehaviour {
    [System.Serializable]
    protected class ParticleEventNames
    {
        public ParticleSystem m_ParticlePrefab;
        public string m_EventName;
    }
    [SerializeField, Tooltip("Array of the particle event names")]
    ParticleEventNames[] m_ArrayOfParticleEventNames;

    Dictionary<string, ParticleSystem> m_NameParticleSysDict = new Dictionary<string, ParticleSystem>();

    public static ParticleCallSystem Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            foreach (ParticleEventNames eventParticle in m_ArrayOfParticleEventNames)
            {
                GameObject instantiatedParticleGO = Instantiate(eventParticle.m_ParticlePrefab.gameObject);
                m_NameParticleSysDict.Add(eventParticle.m_EventName, instantiatedParticleGO.GetComponent<ParticleSystem>());
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayParticle(Vector2 _pos, string _particleName)
    {
        ParticleSystem requestedParticle;
        if (m_NameParticleSysDict.TryGetValue(_particleName, out requestedParticle))
        {
            requestedParticle.transform.position = new Vector3(_pos.x, _pos.y, requestedParticle.transform.position.z);
            requestedParticle.Play();
        }
    }
}
