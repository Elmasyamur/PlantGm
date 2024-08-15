using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ParticleCactus : MonoBehaviour
{
    public Button button;
    public GameObject particle;
    public GameObject vuforiaTarget;
    public float destroyDelay = 10f;
    private Coroutine hideCoroutine;
    private ObserverBehaviour observerBehaviour;
    private bool isTargetTracked = false;
    void Start()
    {
        button.onClick.AddListener(ShowWateringCan);
        particle.SetActive(false);

        observerBehaviour = vuforiaTarget.GetComponent<ObserverBehaviour>();
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED ||
            targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            isTargetTracked = true;
        }
        else
        {
            isTargetTracked = false;
        }
    }
    void ShowWateringCan()
    {
        if (isTargetTracked)
        {
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }
            particle.SetActive(true);
            hideCoroutine = StartCoroutine(HideWateringCanAfterDelay());
        }
    }
    IEnumerator HideWateringCanAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        particle.SetActive(false);
    }
}