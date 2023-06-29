using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quit : MonoBehaviour
{
    public void Quit(){
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
        SceneManager.LoadScene("menu");
     #endif
    }
}
