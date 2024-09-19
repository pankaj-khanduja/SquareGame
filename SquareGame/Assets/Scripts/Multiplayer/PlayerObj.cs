using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerObj : MonoBehaviourPun, IPunObservable
{

    public int score;

    [PunRPC]
    public void OnGameStart()
    {
        SquareController.Instance.Action_OnMultiplayerStart?.Invoke();
        SquareController.Instance.StartGame2();
    }

    private void Update()
    {
        if(this.photonView.IsMine)
        {
            score = SquareController.Instance.PlayerIQScore;
        }
        else
        {
            SquareController.Instance.opponentScore = score;
        }
    }

    // Implementing OnPhotonSerializeView to sync data
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // We are the one sending data to others
        if (stream.IsWriting)
        {
            // Send playerHealth value to other players
            stream.SendNext(score);
        }
        else
        {
            // We are receiving data
            score = (int)stream.ReceiveNext();
        }
    }
}
