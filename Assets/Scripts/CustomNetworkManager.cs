using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Steamworks;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField] private PlayerObjectController GamePlayerPrefab;
    public List<PlayerObjectController> GamePlayers {get;} = new List<PlayerObjectController>();
    string LevelName;

    ///loading stuff:
    public Slider loadingSlider; // Reference to the UI slider
    public Text progressText; // Reference to the UI text element
    public float minimumLoadTime = 2f;
    private AsyncOperation asyncOperation;
    public GameObject LoadingScreenUI;
    bool startLoading = false;
    private float startTime;
    public GameObject menuUI;


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().name == "menu")
        {
            PlayerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);
            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)SteamMatchmaking.GetLobbyMemberByIndex(
                (CSteamID)SteamLobby.Instance.CurrentLobbyID, GamePlayers.Count);
            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
        }
    }

    public void StartGame(string SceneName)
    {
        LevelName = SceneName;
        startLoading = true;
        startTime = Time.time;
    }

    void Update()
    {    
        float num = 0;
        if (startLoading)
        {
            LoadingScreenUI.SetActive(true);
            menuUI.SetActive(false);
            // Update the progress value based on the async operation
            float progress = Mathf.Clamp01(num + 0.01f / 0.9f);
            loadingSlider.value = progress;
            //progressText.text = "Loading: " + (progress * 100f).ToString("F0") + "%";

            // Check if the loading is complete and activate the scene
            if (Time.time - startTime >= minimumLoadTime)
            {
                //asyncOperation.allowSceneActivation = true;
                // Start loading the target scene asynchronously
                ServerChangeScene(LevelName);
                startLoading = false;
            }
        }
    }

    public void CallShutdown()
    {
        Shutdown();        
    }
    
    public void CallStart()
    {
        Start();        
    }

}
