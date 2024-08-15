using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class VentilationButtonTulip: MonoBehaviour
{
    public Button CanvasButton;
    public float disableTime = 20f; // Butonun kaç saniye boyunca devre dýþý kalacaðýný belirleyin
    public GameObject toastPnl; // Toast mesajý için panel objesi
    public float toastDur = 2f; // Toast mesajýnýn ekranda kalma süresi

    private string ButtonStateKey; // PlayerPrefs anahtarlarý için deðiþkenler
    private string ButtonDisableEndTimeKey;

    void Start()
    {
        // Buton için benzersiz anahtarlar oluþtur
        string buttonName = CanvasButton.name;
        ButtonStateKey = buttonName + "_ButtonState";
        ButtonDisableEndTimeKey = buttonName + "_ButtonDisableEndTime";

        // Butonun durumunu yükle
        LoadButtonState();

        CanvasButton.onClick.AddListener(OnButtonClick);
        toastPnl.SetActive(false); // Baþlangýçta toast panelini gizle
    }

    void OnButtonClick()
    {
        StartCoroutine(DisableButtonTemporarily());
        StartCoroutine(ShowToastMessage());
    }

    IEnumerator DisableButtonTemporarily()
    {
        CanvasButton.interactable = false; // Butonun týklanabilirliðini kapat

        // Devre dýþý kalma süresini ve son zamanýný kaydet
        SaveButtonState(false);
        SaveDisableEndTime();

        yield return new WaitForSeconds(disableTime);

        CanvasButton.interactable = true; // Belirli bir süre sonra butonun týklanabilirliðini geri aç
        SaveButtonState(true);
    }

    IEnumerator ShowToastMessage()
    {
        toastPnl.SetActive(true); // Toast panelini göster
        yield return new WaitForSeconds(toastDur);
        toastPnl.SetActive(false); // Belirli bir süre sonra toast panelini gizle
    }

    private void SaveButtonState(bool isActive)
    {
        PlayerPrefs.SetInt(ButtonStateKey, isActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SaveDisableEndTime()
    {
        DateTime currentTime = DateTime.UtcNow; // UTC zamanýný al
        DateTime endTime = currentTime.AddSeconds(disableTime); // Devre dýþý kalma süresi sonunda bitiþ zamaný hesapla
        PlayerPrefs.SetString(ButtonDisableEndTimeKey, endTime.ToString("o")); // ISO 8601 formatýnda sakla
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
                    // Buton devre dýþý ve süre dolmamýþsa
                    CanvasButton.interactable = false;
                    StartCoroutine(UpdateButtonState(savedEndTime));
                }
                else
                {
                    // Süre dolmuþ veya buton aktifse
                    CanvasButton.interactable = true;
                    SaveButtonState(true); // Butonu aktif yap ve durumu kaydet
                    PlayerPrefs.DeleteKey(ButtonDisableEndTimeKey); // Devre dýþý kalma süresini temizle
                }
            }
            else
            {
                // Tarih formatý geçersizse varsayýlan olarak butonu aktif yap
                CanvasButton.interactable = true;
            }
        }
        else
        {
            // Ýlk baþlatma, varsayýlan olarak butonu aktif yap
            CanvasButton.interactable = true;
        }
    }

    private IEnumerator UpdateButtonState(DateTime endTime)
    {
        while (DateTime.UtcNow < endTime)
        {
            yield return null; // Süre dolana kadar bekle
        }
        CanvasButton.interactable = true;
        SaveButtonState(true); // Butonu tekrar aktif yap ve durumu kaydet
        PlayerPrefs.DeleteKey(ButtonDisableEndTimeKey); // Devre dýþý kalma süresini temizle
    }
}