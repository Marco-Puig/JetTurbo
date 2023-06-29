using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public GameObject ui_menu;
    public GameObject ui_multi;
    public GameObject ui_map;
    public GameObject ui_news;
    public GameObject ui_set;

    public void Quit(){
    #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
    }
    public void Play(){
        SceneManager.LoadScene("Dev_Tst_Offline");
    }
    public void Multiplayer(){
        ui_multi.SetActive(true);
        ui_menu.SetActive(false);
        ui_map.SetActive(false);
        ui_news.SetActive(false);
        ui_set.SetActive(false);
    }
    public void Multiplayer_Back(){
        ui_multi.SetActive(false);
        ui_menu.SetActive(true);
        ui_map.SetActive(false);
        ui_news.SetActive(true);
        ui_set.SetActive(false);
    }
    public void Maps(){
        ui_multi.SetActive(false);
        ui_menu.SetActive(false);
        ui_map.SetActive(true);
        ui_news.SetActive(false);
        ui_set.SetActive(false);
    }
    public void Settings(){
        ui_multi.SetActive(false);
        ui_menu.SetActive(false);
        ui_map.SetActive(false);
        ui_news.SetActive(false);
        ui_set.SetActive(true);
    }
}
