using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

class PingUpdater : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Hashtable props = photonView.Controller.CustomProperties;
        props["Ping"] = PhotonNetwork.GetPing();
        photonView.Controller.SetCustomProperties(props);
    }
}
