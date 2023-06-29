using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SteamUserDisplay : MonoBehaviour
{
    public Text nameText;
    public Image iconPlayer;

    void Start()
    {
        if(!SteamManager.Initialized)
        {
            nameText.enabled = false;
            iconPlayer.enabled = false;
            return;
        }
        
        nameText.text = SteamFriends.GetPersonaName();
        
        StartCoroutine(LoadAvatarIcon());
    }


    int avatarInt;
    uint width, height;
    Texture2D downloadedAvatar;
    Rect rect = new Rect(0,0,184,184);
    Vector2 pivot = new Vector2(0.5f, 0.5f);

    IEnumerator LoadAvatarIcon()
    {
        avatarInt = SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID());
        
        while (avatarInt == -1)
        {
            yield return null;
        }

        if (avatarInt > 0)
        {
            SteamUtils.GetImageSize(avatarInt, out width, out height);

                if(width > 0 && height > 0)
                {
                    byte[] avatarStream = new byte[4 * (int)width * (int)height];

                    SteamUtils.GetImageRGBA(avatarInt, avatarStream, 4 * (int)width * (int)height);

                    downloadedAvatar = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);

                    downloadedAvatar.LoadRawTextureData(avatarStream);

                    downloadedAvatar.Apply();

                    iconPlayer.sprite = Sprite.Create(downloadedAvatar, rect, pivot);

                }
        }
    }
}