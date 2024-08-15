using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3; 
    public AudioSource audioSource4;
    public AudioSource audioSource5;
    public AudioSource audioSource6; 
    public Image soundIcon;          
    public Sprite soundOnSprite;     
    public Sprite soundOffSprite;   

    private bool isMuted = false; 

    void Start(){
        UpdateSoundIcon(); 
        MuteAllSources(isMuted); 
    }

    public void ToggleSound(){
        isMuted = !isMuted; 
        MuteAllSources(isMuted);
        UpdateSoundIcon(); 
    }

    private void MuteAllSources(bool mute){
        audioSource1.mute = mute;
        audioSource2.mute = mute;
        audioSource3.mute = mute;
        audioSource4.mute = mute;
        audioSource5.mute = mute;
        audioSource6.mute = mute;
    }

    private void UpdateSoundIcon(){
        soundIcon.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
}