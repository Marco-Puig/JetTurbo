using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.Demo
{
    public class LeaderboardShowcase : MonoBehaviour
    {
        [SerializeField] private string _leaderboardPublicKey;
        
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI[] _entryFields;
        
        [SerializeField] private Text _playerUsernameInput;

        private int _playerScore;
        
        private void Start()
        {
            Load();
            Submit();
        }

        public void Update()
        {
            _playerScore = (int)(PlayerPrefs.GetFloat("HighScore") * 100);
            _playerScoreText.text = "Best Time: " + (_playerScore / 100) + ":" + (_playerScore - _playerScore / 100 * 100);    
        }
        
        public void Load() => LeaderboardCreator.GetLeaderboard(_leaderboardPublicKey, OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            foreach (var entryField in _entryFields)
            {
                entryField.text = "";
            }
            
            for (int i = 0; i < entries.Length; i++)
            {
                _entryFields[i].text = $"{entries[i].RankSuffix()}. {entries[i].Username} : {entries[i].Score / 100}" +":"+ $"{entries[i].Score - entries[i].Score / 100 * 100}";
            }
        }

        public void Submit()
        {
            LeaderboardCreator.UploadNewEntry(_leaderboardPublicKey, _playerUsernameInput.text, _playerScore, Callback, ErrorCallback);
        }
        
        public void DeleteEntry()
        {
            LeaderboardCreator.DeleteEntry(_leaderboardPublicKey, Callback, ErrorCallback);
        }

        public void ResetPlayer()
        {
            LeaderboardCreator.ResetPlayer();
        }
        
        private void Callback(bool success)
        {
            if (success)
                Load();
        }
        
        private void ErrorCallback(string error)
        {
            Debug.LogError(error);
        }
    }
}
