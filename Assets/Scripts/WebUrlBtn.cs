using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebUrlBtn : MonoBehaviour
{
    public void url(string link)
    {
        Application.OpenURL(link);
    }
}
