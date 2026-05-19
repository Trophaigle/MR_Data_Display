using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeatherBar : MonoBehaviour
{
    public WeatherType weatherType;

    private bool active = false;
     private InteractableUnityEventWrapper wrapper;

      void Awake()
    {
        wrapper = transform.GetChild(0).GetComponent<InteractableUnityEventWrapper>();

        wrapper.WhenSelect.AddListener(OnSelected);
    }

    public void SetEffect(WeatherType newEffect)
    {
        weatherType = newEffect;
    }

    private void OnSelected()
{
    Debug.Log("🔥 BAR CLICKED");

    active = !active;

    Debug.Log("Active state = " + active);

    if (active)
    {
        Debug.Log("SPAWN EFFECT: " + weatherType);

        WeatherEffectManager.Instance.ToggleEffect(weatherType, transform);
    }
    else
    {
        Debug.Log("STOP EFFECT");

        WeatherEffectManager.Instance.StopEffect();
    }
}
}
