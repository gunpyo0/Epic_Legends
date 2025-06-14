using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [System.Serializable]
    public class ParticleEntry
    {
        public string key;
        public GameObject prefab;
    }

    [SerializeField]
    public ParticleEntry[] particleEntries;

    private Dictionary<string, GameObject> particleDict;
    private static ParticleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            particleDict = new Dictionary<string, GameObject>();
            foreach (var entry in particleEntries)
            {
                if (entry != null && !string.IsNullOrEmpty(entry.key) && entry.prefab != null)
                {
                    particleDict[entry.key] = entry.prefab;
                }
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawn a particle prefab registered under the given key at position.
    /// </summary>
    /// <param name="key">Name of the entry in inspector.</param>
    /// <param name="position">World position to spawn at.</param>
    public static void Play(string key, Vector3 position)
    {
        if (instance == null)
        {
            Debug.LogError("ParticleManager not initialized. Make sure a ParticleManager exists in the scene.");
            return;
        }
        instance.Spawn(key, position);
    }

    private void Spawn(string key, Vector3 position)
    {
        if (!particleDict.ContainsKey(key))
        {
            Debug.LogWarning($"No particle registered under key '{key}'");
            return;
        }

        GameObject prefab = particleDict[key];
        GameObject go = Instantiate(prefab, position, Quaternion.identity);

        // Automatically play any ParticleSystem components
        var systems = go.GetComponentsInChildren<ParticleSystem>();
        float maxLifetime = 0f;
        foreach (var ps in systems)
        {
            var main = ps.main;
            ps.Play();
            float lifetime = main.duration;
            if (main.startLifetime.mode == ParticleSystemCurveMode.Constant)
            {
                lifetime += main.startLifetime.constant;
            }
            else
            {
                lifetime += main.startLifetime.constantMax;
            }
            if (lifetime > maxLifetime)
                maxLifetime = lifetime;
        }

        if (maxLifetime > 0f)
            Destroy(go, maxLifetime);
    }
}
