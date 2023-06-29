using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider; // Reference to the UI slider
    public Text progressText; // Reference to the UI text element
    public float minimumLoadTime = 2f;

    private AsyncOperation asyncOperation;

    public GameObject LoadingScreenUI;

    bool startLoading = false;

    private float startTime;

    public GameObject menuUI;

    public GameObject[] others;

    public CustomNetworkManager manager;

    // Start is called before the first frame update
    public void LoadLevel(string level)
    {
        // Start loading the target scene asynchronously
        asyncOperation = SceneManager.LoadSceneAsync(level);

        // Disable scene activation to prevent it from loading immediately
        asyncOperation.allowSceneActivation = false;

        startLoading = true;

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startLoading)
        {
            if (others != null)
            {
                for (int i = 0; i < others.Length; i++)
                {
                    others[i].SetActive(false);
                }                
            }

            LoadingScreenUI.SetActive(true);
            menuUI.SetActive(false);
            // Update the progress value based on the async operation
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingSlider.value = progress;
            //progressText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";

            // Check if the loading is complete and activate the scene
            if (asyncOperation.progress >= 0.9f && Time.time - startTime >= minimumLoadTime)
            {
                asyncOperation.allowSceneActivation = true;
            }
        }
    }
}