using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GrowthCactus : MonoBehaviour
{
    [SerializeField] private GameObject plant;
    [SerializeField] private float maxSize = 2f;
    [SerializeField] private float growthPerClick = 0.50f;
    [SerializeField] private bool kaydet = true;
    [SerializeField] private Button ResetButton;
    [SerializeField] private Button sulamabuton;
    private Vector3 initialScale;
    private float growthDelay = 3f;
    private float growthDuration = 10f;

    void Start()
    {
        initialScale = plant.transform.localScale;

        if (kaydet)
        {
            float savedScaleX = PlayerPrefs.GetFloat("PlantScaleX", plant.transform.localScale.x);
            float savedScaleY = PlayerPrefs.GetFloat("PlantScaleY", plant.transform.localScale.y);
            float savedScaleZ = PlayerPrefs.GetFloat("PlantScaleZ", plant.transform.localScale.z);
            plant.transform.localScale = new Vector3(savedScaleX, savedScaleY, savedScaleZ);
        }
        sulamabuton.onClick.AddListener(OnButtonClick);
        ResetButton.onClick.AddListener(RstScale);
    }

    public void OnButtonClick()
    {
        if (plant.transform.localScale.x < initialScale.x * maxSize)
        {
            Vector3 targetScale = plant.transform.localScale + initialScale * growthPerClick;
            StartCoroutine(DelayedGrowth(targetScale));
        }
    }

    private IEnumerator DelayedGrowth(Vector3 targetScale)
    {
        yield return new WaitForSeconds(growthDelay);
        yield return StartCoroutine(GrowPlantOverTime(targetScale));
    }

    private IEnumerator GrowPlantOverTime(Vector3 targetScale)
    {
        Vector3 startScale = plant.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < growthDuration)
        {
            plant.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / growthDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        plant.transform.localScale = targetScale;
        PlayerPrefs.SetFloat("PlantScaleX", plant.transform.localScale.x);
        PlayerPrefs.SetFloat("PlantScaleY", plant.transform.localScale.y);
        PlayerPrefs.SetFloat("PlantScaleZ", plant.transform.localScale.z);
        PlayerPrefs.Save();
    }

    public void RstScale()
    {
        plant.transform.localScale = initialScale;
        PlayerPrefs.SetFloat("PlantScaleX", initialScale.x);
        PlayerPrefs.SetFloat("PlantScaleY", initialScale.y);
        PlayerPrefs.SetFloat("PlantScaleZ", initialScale.z);
        PlayerPrefs.Save();
    }
}