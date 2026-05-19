using UnityEngine;

public class WeatherEffectManager : MonoBehaviour
{

    // ✅ Singleton global
    public static WeatherEffectManager Instance;

    public WeatherAudio weatherAudio;
    public WeatherEffectSettings[] settings;

    private GameObject currentEffect;

    private WeatherBar currentBar;

    

    void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;

    Debug.Log("WeatherManager READY");
}

 public void ToggleEffect(
        WeatherType effect,
        Transform barTransform
    )
    {
        // 🔥 même barre = OFF
        if (currentBar != null &&
            currentBar.transform == barTransform)
        {
            StopEffect();
            return;
        }

        StopEffect();

        // 🔍 trouver config météo
        WeatherEffectSettings config = null;

        foreach (var s in settings)
        {
            if (s.type == effect)
            {
                config = s;
                break;
            }
        }

        if (config == null)
        {
            Debug.LogWarning(
                "No config found for weather type: " + effect
            );
            return;
        }

        // ⭐ spawn DIRECTEMENT en enfant
        currentEffect = Instantiate(
            config.prefab,
            barTransform
        );

        // ⭐ transform local
        currentEffect.transform.localPosition =
            config.localOffset;

        currentEffect.transform.localRotation =
            Quaternion.Euler(config.localRotation);

        currentEffect.transform.localScale =
            config.localScale;

        // ⭐ particles en local
        var ps = currentEffect.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            var main = ps.main;

            main.simulationSpace =
                ParticleSystemSimulationSpace.Local;
        }

        // ⭐ audio
        if (weatherAudio != null)
        {
            weatherAudio.Play(effect);
        }

        currentBar =
            barTransform.GetComponent<WeatherBar>();
    }

    public void StopEffect()
    {
        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }

        if (weatherAudio != null)
        {
            weatherAudio.Stop();
        }

        currentEffect = null;
        currentBar = null;
    }
}

[System.Serializable]
public class WeatherEffectSettings
{
    public WeatherType type;

    public GameObject prefab;

    public Vector3 localOffset;
    public Vector3 localRotation;
    public Vector3 localScale;
}
