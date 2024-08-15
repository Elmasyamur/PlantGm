using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonController : MonoBehaviour {
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundClip;
    [SerializeField] private Button button;

    private void Start(){
        if (button != null){
            button.onClick.AddListener(OnButtonClick);
        }
        if (objectToShow != null){
            objectToShow.SetActive(false);
        }
    }

    private void OnButtonClick(){
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);
            if (audioSource != null && soundClip != null) {
                audioSource.PlayOneShot(soundClip);
            }
            StartCoroutine(HideObjectAfterDelay(2f));
        }
    }

    private IEnumerator HideObjectAfterDelay(float delay){
        yield return new WaitForSeconds(delay);
        if (objectToShow != null){
            objectToShow.SetActive(false);
        }
    }
}