using UnityEngine;

public class SoundControl : MonoBehaviour
{
    private bool lowerMusicVolume;
    private AudioSource currentMusic;
    public void LowerVolume()
    {
        lowerMusicVolume = true;
        currentMusic = SoundManager.GetCurrentMusic();
    }

    void Update()
    {
        if(lowerMusicVolume)
        {
            currentMusic.volume -= .1f * Time.deltaTime;
            if(currentMusic.volume <= 0) lowerMusicVolume = false;
        }
    }
}
