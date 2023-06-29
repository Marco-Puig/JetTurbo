using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBtn : MonoBehaviour
{
    public void ClickLeave()
    {
        SceneManager.LoadScene("menu");
    }
    public void ClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
