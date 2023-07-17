using System;
using System.Collections;
using System.Collections.Generic;
using Dan.Enums;
using Dan.Models;
using UnityEngine;
using UnityEngine.Networking;

using static Dan.ConstantVariables;

namespace Dan.Main
{
    public class LeaderboardCreatorBehaviour : MonoBehaviour
    {
        [Serializable]
        private struct EntryResponse
        {
            public Entry[] entries;
        }
        
        internal void Authorize(Action<string> callback)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerPrefsGuidKey, "")))
            {
                callback?.Invoke(PlayerPrefs.GetString(PlayerPrefsGuidKey));
                return;
            }
            
            var request = UnityWebRequest.Get(GetServerURL(Routes.Authorize));
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(null);
                    return;
                }

                var guid = request.downloadHandler.text;
                PlayerPrefs.SetString(PlayerPrefsGuidKey, guid);
                callback?.Invoke(guid);
            }));
        }
        
        internal void ResetAndAuthorize(Action<string> callback, Action onFinish)
        {
            callback += guid =>
            {
                if (string.IsNullOrEmpty(guid)) return;
                onFinish?.Invoke();
            };
            PlayerPrefs.DeleteKey(PlayerPrefsGuidKey);
            Authorize(callback);
        }
        
        internal void SendGetRequest(string url, Action<bool> callback = null)
        {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(false);
                    return;
                }
                callback?.Invoke(true);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }
        
        internal void SendGetRequest(string url, Action<Entry[]> callback = null)
        {
            var request = UnityWebRequest.Get(url);
            StartCoroutine(HandleRequest(request, isSuccessful =>
            {
                if (!isSuccessful)
                {
                    HandleError(request);
                    callback?.Invoke(Array.Empty<Entry>());
                    return;
                }
                var tmp = "{\"entries\":" + request.downloadHandler.text + "}";
                var response = JsonUtility.FromJson<EntryResponse>(tmp);
                callback?.Invoke(response.entries);
                LeaderboardCreator.Log("Successfully retrieved leaderboard data!");
            }));
        }
        
        internal void SendPostRequest(string url, List<IMultipartFormSection> form, Action<bool> callback = null, Action<string> errorCallback = null)
        {
            var request = UnityWebRequest.Post(url, form);
            StartCoroutine(HandleRequest(request, callback, errorCallback));
        }
        
#if UNITY_ANDROID
        private class ForceAcceptAll : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData) => true;
        }
#endif
        private static IEnumerator HandleRequest(UnityWebRequest request, Action<bool> onComplete, Action<string> errorCallback = null)
        {
#if UNITY_ANDROID
            request.certificateHandler = new ForceAcceptAll();
#endif
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                onComplete.Invoke(false);
                errorCallback?.Invoke(request.responseCode + ": " + request.downloadHandler.text);
                request.downloadHandler.Dispose();
                request.Dispose();
                yield break;
            }

            onComplete.Invoke(true);
            request.downloadHandler.Dispose();
            request.Dispose();
        }
        
        private static void HandleError(UnityWebRequest request)
        {
            var message = Enum.GetName(typeof(StatusCode), (StatusCode)request.responseCode).SplitByUppercase();
                
            var downloadHandler = request.downloadHandler;
            var text = downloadHandler.text;
            if (!string.IsNullOrEmpty(text))
                message = $"{message}: {text}";
            LeaderboardCreator.LogError(message);
        }
    }
}