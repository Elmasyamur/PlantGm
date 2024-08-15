using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    float a;
    void Start() {
        a = 0;  
    }
    void Update(){
        a += 1 * Time.deltaTime;
        if (a > 6){
            SceneManager.LoadScene(1);
        }
    }
}
