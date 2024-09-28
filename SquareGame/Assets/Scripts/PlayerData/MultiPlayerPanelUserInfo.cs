using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MultiPlayerPanelUserInfo : MonoBehaviour
{

    public Image userPicImage;
    public TextMeshProUGUI playerNameText , gameOverPanelText;
    public bool isLocalPlayer;


    private void OnEnable()
    {
        if (isLocalPlayer)
        {
            SquareController.Instance.Action_OnLocalPlayerDataReceived += OnPlayerDataLoaded;
        }
        else
        {
            SquareController.Instance.Action_OnOpponentDataReceived += OnPlayerDataLoaded;
        }
    }

    private void OnDisable()
    {
        if (isLocalPlayer)
        {
            SquareController.Instance.Action_OnLocalPlayerDataReceived -= OnPlayerDataLoaded;
        }
        else
        {
            SquareController.Instance.Action_OnOpponentDataReceived -= OnPlayerDataLoaded;
        }
    }


    void OnPlayerDataLoaded(string playerName , Texture2D picTex)
    {
        Texture2D tex = picTex;
        Sprite newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        userPicImage.sprite = newSprite;
        gameOverPanelText.text =  playerNameText.text = playerName;
    }
}
