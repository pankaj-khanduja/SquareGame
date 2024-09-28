using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SquareOne;
using System;
public class EditPlayerProfile : MonoBehaviour
{

    public TMP_InputField _playerNameField;
    public Image selectedSprite;
    public Sprite[] PicCollections;
    public GameObject ErrorObj;
    private void OnEnable()
    {
        if(Constant.PlayerLogin)
        {
            Texture2D tex = MyProfile.Instance._PlayerData._PlayerDataContainer.userPic;
            Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            selectedSprite.sprite = newSprite;
            _playerNameField.text = MyProfile.Instance._PlayerData._PlayerDataContainer.playerName;
        }
    }

    public void SaveBnClicked()
    {
        if(_playerNameField.text.Equals(string.Empty))
        {
            ErrorObj.SetActive(true);
            Invoke("DisableError", 2);
            return;
        }
        // Make a readable copy if the texture is compressed
        Texture2D readableTexture = MakeTextureReadable(selectedSprite.sprite.texture);
        // Encode to PNG
        byte[] pngData = readableTexture.EncodeToPNG();

        if (Constant.PlayerLogin) SaveData(pngData);
        else LoginWithID(pngData);




    }

    void LoginWithID(byte[] pngData)
    {
        Dictionary<string, string> customData = new Dictionary<string, string>();
        customData.Add(Constant.userName, _playerNameField.text);
        customData.Add(Constant.customID, SystemInfo.deviceUniqueIdentifier);
        customData.Add(Constant.picBase64, Convert.ToBase64String(pngData));
        LoginController.Instance?.LoginWithCustomID(customData);
    }

    void SaveData(byte[] pngData)
    {
        Dictionary<string, string> customData = new Dictionary<string, string>();
        customData.Add(Constant.userName, _playerNameField.text);
        customData.Add(Constant.picBase64, Convert.ToBase64String(pngData));
        MyProfile.Instance.SaveProfileData(customData);
        this.gameObject.SetActive(false);

    }

    private Texture2D MakeTextureReadable(Texture2D texture)
    {
        // Create a new Texture2D with the same dimensions and format
        Texture2D readableTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);

        // Copy pixel data from the original texture
        readableTexture.SetPixels(texture.GetPixels());
        readableTexture.Apply();

        return readableTexture;
    }

    void DisableError()
    {
        ErrorObj.SetActive(false);
    }

    public void PicSelect(int index)
    {
        selectedSprite.sprite = PicCollections[index];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
