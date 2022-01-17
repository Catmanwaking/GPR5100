using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomName_InputField;
    [SerializeField] private TMP_InputField nickName_InputField;
    [SerializeField] private TMP_Text info_Text;

    public void CreateRoom()
    {
        if (!TrySetName())
            return;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.CreateRoom(roomName_InputField.text, roomOptions);
    }

    public void JoinRoom()
    {
        if (!TrySetName())
            return;
        PhotonNetwork.JoinRoom(roomName_InputField.text);
    }

    public bool TrySetName()
    {
        string name = nickName_InputField.text;
        if(string.IsNullOrWhiteSpace(name))
        {
            info_Text.text = $"\"{nickName_InputField.text}\" is not a valid name";
            return false;
        }
        PhotonNetwork.LocalPlayer.NickName = name;
        return true;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        info_Text.text = message;
    }
}
