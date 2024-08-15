using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CactusPart : MonoBehaviour
{
    public Button button;
    public GameObject sack;
    public GameObject vuforiaTarget;
    public float destroyDelay = 10f;
    private Coroutine hideCoroutine;
    private ObserverBehaviour observerBehaviour;
    private bool isTargetTracked = false;
    void Start()
    {
        button.onClick.AddListener(ShowWateringCan);
        sack.SetActive(false);

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
            sack.SetActive(true);
            hideCoroutine = StartCoroutine(HideWateringCanAfterDelay());
        }
    }
    IEnumerator HideWateringCanAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        sack.SetActive(false);
    }
}