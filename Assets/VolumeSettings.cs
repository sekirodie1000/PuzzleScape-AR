using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings_2 : MonoBehaviour
{
   [SerializeField] private AudioMixer myMixer;
   [SerializeField] private Slider musicSlider;
   public void SetMusicVolume() 
   {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", volume);
   }

}
