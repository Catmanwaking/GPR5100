using Photon.Pun;
using UnityEngine;

public class RPCLinker : MonoBehaviour
{
    [SerializeField] private WeaponController weapon;

    [PunRPC]
    private void ShootRpc()
    {
        weapon.ShootRpcLink();
    }

    [PunRPC]
    private void ReloadRpc()
    {
        weapon.ReloadRpcLink();
    }
}
