using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ButtonController3 : MonoBehaviour
{
    [SerializeField] private Button CanvasButton;
    [SerializeField] private float disableTime = 20f;
    [SerializeField] private GameObject toastPnl;
    [SerializeField] private float toastDur = 2f;

    private string ButtonStateKey;
    private string ButtonDisableEndTimeKey;

    void Start()
    {
        string buttonName = CanvasButton.name;
        ButtonStateKey = buttonName + "_ButtonState";
        ButtonDisableEndTimeKey = buttonName + "_ButtonDisableEndTime";
        LoadButtonState();
        CanvasButton.onClick.AddListener(OnButtonClick);
        toastPnl.SetActive(false);
    }

    void OnButtonClick()
    {
        StartCoroutine(DisableButtonTemporarily());
        StartCoroutine(ShowToastMessage());
    }

    IEnumerator DisableButtonTemporarily()
    {
        CanvasButton.interactable = false;

        SaveButtonState(false);
        SaveDisableEndTime();

        yield return new WaitForSeconds(disableTime);

        CanvasButton.interactable = true;
        SaveButtonState(true);
    }

    IEnumerator ShowToastMessage()
    {
        toastPnl.SetActive(true);
        yield return new WaitForSeconds(toastDur);
        toastPnl.SetActive(false);
    }

    private void SaveButtonState(bool isActive)
    {
        PlayerPrefs.SetInt(ButtonStateKey, isActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SaveDisableEndTime()
    {
        DateTime currentTime = DateTime.UtcNow;
        DateTime endTime = currentTime.AddSeconds(disableTime);
        PlayerPrefs.SetString(ButtonDisableEndTimeKey, endTime.ToString("o"));
        PlayerPrefs.Save();
    }

    private void LoadButtonState()
    {
        if (PlayerPrefs.HasKey(ButtonStateKey) && PlayerPrefs.HasKey(ButtonDisableEndTimeKey))
        {
            int savedState = PlayerPrefs.GetInt(ButtonStateKey);
            string savedEndTimeString = PlayerPrefs.GetString(ButtonDisableEndTimeKey);
            DateTime savedEndTime;

            if (DateTime.TryParse(savedEndTimeString, null, System.Globalization.DateTimeStyles.RoundtripKind, out savedEndTime))
            {
                DateTime currentTime = DateTime.UtcNow;

                if (currentTime < savedEndTime)
                {
                    CanvasButton.interactable = false;
                    StartCoroutine(UpdateButtonState(savedEndTime));
                }
                else
                {
                    CanvasButton.interactable = true;
                    SaveButtonState(true);
                    PlayerPrefs.DeleteKey(ButtonDisableEndTimeKey);
                }
            }
            else
            {
                CanvasButton.interactable = true;
            }
        }
        else
        {
            CanvasButton.interactable = true;
        }
    }

    private IEnumerator UpdateButtonState(DateTime endTime)
    {
        while (DateTime.UtcNow < endTime)
        {
            yield return null;
        }
        CanvasButton.interactable = true;
        SaveButtonState(true);
        PlayerPrefs.DeleteKey(ButtonDisableEndTimeKey);
    }
}