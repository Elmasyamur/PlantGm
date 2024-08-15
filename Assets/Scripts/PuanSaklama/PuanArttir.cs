using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons; 
    public TMP_Text scoreText; 
    public int[] pointValues; 

    private int currentScore = 0; 
    private const string ScoreKey = "CurrentScore"; 

    void Start(){
        if (buttons.Length != pointValues.Length){
            Debug.LogError("Buton sayýsý ve puan deðerleri dizisi eþleþmiyor!");
            return;
        }
        LoadScore();

        for (int i = 0; i < buttons.Length; i++){
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }

    void OnButtonClick(int index){
        if (index >= 0 && index < pointValues.Length){
            currentScore += pointValues[index];
            if (scoreText != null){
                scoreText.text = currentScore.ToString();
            }
            SaveScore();
        }
    }

    void SaveScore(){
        PlayerPrefs.SetInt(ScoreKey, currentScore);
        PlayerPrefs.Save(); 
    }
    void LoadScore(){
        if (PlayerPrefs.HasKey(ScoreKey)){
            currentScore = PlayerPrefs.GetInt(ScoreKey);
            if (scoreText != null){
                scoreText.text = currentScore.ToString();
            }
        }
    }
}