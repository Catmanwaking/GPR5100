using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomnameInputField;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomnameInputField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomnameInputField.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
