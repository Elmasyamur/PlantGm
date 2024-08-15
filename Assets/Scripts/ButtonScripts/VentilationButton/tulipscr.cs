using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class VentilationButtonTulip: MonoBehaviour
{
    public Button CanvasButton;
    public float disableTime = 20f; // Butonun ka� saniye boyunca devre d��� kalaca��n� belirleyin
    public GameObject toastPnl; // Toast mesaj� i�in panel objesi
    public float toastDur = 2f; // Toast mesaj�n�n ekranda kalma s�resi

    private string ButtonStateKey; // PlayerPrefs anahtarlar� i�in de�i�kenler
    private string ButtonDisableEndTimeKey;

    void Start()
    {
        // Buton i�in benzersiz anahtarlar olu�tur
        string buttonName = CanvasButton.name;
        ButtonStateKey = buttonName + "_ButtonState";
        ButtonDisableEndTimeKey = buttonName + "_ButtonDisableEndTime";

        // Butonun durumunu y�kle
        LoadButtonState();

        CanvasButton.onClick.AddListener(OnButtonClick);
        toastPnl.SetActive(false); // Ba�lang��ta toast panelini gizle
    }

    void OnButtonClick()
    {
        StartCoroutine(DisableButtonTemporarily());
        StartCoroutine(ShowToastMessage());
    }

    IEnumerator DisableButtonTemporarily()
    {
        CanvasButton.interactable = false; // Butonun t�klanabilirli�ini kapat

        // Devre d��� kalma s�resini ve son zaman�n� kaydet
        SaveButtonState(false);
        SaveDisableEndTime();

        yield return new WaitForSeconds(disableTime);

        CanvasButton.interactable = true; // Belirli bir s�re sonra butonun t�klanabilirli�ini geri a�
        SaveButtonState(true);
    }

    IEnumerator ShowToastMessage()
    {
        toastPnl.SetActive(true); // Toast panelini g�ster
        yield return new WaitForSeconds(toastDur);
        toastPnl.SetActive(false); // Belirli bir s�re sonra toast panelini gizle
    }

    private void SaveButtonState(bool isActive)
    {
        PlayerPrefs.SetInt(ButtonStateKey, isActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SaveDisableEndTime()
    {
        DateTime currentTime = DateTime.UtcNow; // UTC zaman�n� al
        DateTime endTime = currentTime.AddSeconds(disableTime); // Devre d��� kalma s�resi sonunda biti� zaman� hesapla
        PlayerPrefs.SetString(ButtonDisableEndTimeKey, endTime.ToString("o")); // ISO 8601 format�nda sakla
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
                    // Buton devre d��� ve s�re dolmam��sa
                    CanvasButton.interactable = false;
                    StartCoroutine(UpdateButtonState(savedEndTime));
                }
                else
                {
                    // S�re dolmu� veya buton aktifse
                    CanvasButton.interactable = true;
                    SaveButtonState(true); // Butonu aktif yap ve durumu kaydet
                    PlayerPrefs.DeleteKey(ButtonDisableEndTimeKey); // Devre d��� kalma s�resini temizle
                }
            }
            else
            {
                // Tarih format� ge�ersizse varsay�lan olarak butonu aktif yap
                CanvasButton.interactable = true;
            }
        }
        else
        {
            // �lk ba�latma, varsay�lan olarak butonu aktif yap
            CanvasButton.interactable = true;
        }
    }

    private IEnumerator UpdateButtonState(DateTime endTime)
    {
        while (DateTime.UtcNow < endTime)
        {
            yield return null; // S�re dolana kadar bekle
        }
        CanvasButton.interactable = true;
        SaveButtonState(true); // Butonu tekrar aktif yap ve durumu kaydet
        PlayerPrefs.DeleteKey(ButtonDisableEndTimeKey); // Devre d��� kalma s�resini temizle
    }
}