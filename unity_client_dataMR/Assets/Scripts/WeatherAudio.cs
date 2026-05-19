using UnityEngine;

public class WeatherAudio : MonoBehaviour
{
     public AudioSource audioSource;

    public AudioClip rainSound;
    public AudioClip sunnySound;
    public AudioClip stormSound;
    public AudioClip cloudSound;

    public float fadeSpeed = 2f;

    private float targetVolume = 0f;

    void Update()
    {
        audioSource.volume = Mathf.Lerp(
            audioSource.volume,
            targetVolume,
            Time.deltaTime * fadeSpeed
        );

        // stop totalement quand presque inaudible
        if (audioSource.volume < 0.01f &&
            targetVolume == 0f &&
            audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void Play(WeatherType weatherType)
    {
        switch(weatherType)
        {
            case WeatherType.Rain:
                audioSource.clip = rainSound;
                break;

            case WeatherType.Sunny:
                audioSource.clip = sunnySound;
                break;

            case WeatherType.Storm:
                audioSource.clip = stormSound;
                break;

            case WeatherType.Cloudy:
                audioSource.clip = cloudSound;
                break;
        }

        // évite restart audio si déjà joué
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        targetVolume = 1f;
    }

    public void Stop()
    {
        targetVolume = 0f;
    }
  
}
