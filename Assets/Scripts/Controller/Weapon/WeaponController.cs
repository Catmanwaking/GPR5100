//Author: Dominik Dohmeier

using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public abstract class WeaponController : MonoBehaviourPun
{
    [Header("WeaponStats")]
    [SerializeField] protected bool isAutoFire;
    [SerializeField] protected int ammoCapacity;
    [SerializeField] protected int ammoUsePerShot;
    [SerializeField] protected int bulletsPerShot;
    [SerializeField] protected float shotsPerSecond;
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField]
    [Tooltip("A spread value of 1.0f is equal to a spreadcone of 90°")]
    protected Deviation deviation;
    [SerializeField] protected LayerMask hitLayers;

    [Header("Sounds")]
    [SerializeField] protected AudioClip gunShotClip;
    [SerializeField] protected AudioClip reloadClip;

    [Header("MuzzleFlash")]
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Transform muzzleFlashTransform;

    protected bool isFiring;
    protected bool isReloading;
    protected bool canFire;
    protected int currentAmmo;
    protected float fireRate;
    protected float nextShotTime;

    protected int fireHash;
    protected int reloadHash;

    protected Transform shotOrigin;
    protected Animator animator;
    protected AudioController audioController;

    private bool displayGizmos = false;

    //[HideInInspector] public ObjectPool<ParticleSystem> sparkObjectPool;

    public System.Action<int> OnAmmoChange;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioController = GetComponentInParent<AudioController>();
        displayGizmos = true;
    }

    private void Start()
    {
        fireHash = Animator.StringToHash("Fire");
        reloadHash = Animator.StringToHash("Reload");
        if(photonView.IsMine)
        {
            isFiring = false;
            canFire = true;
            currentAmmo = ammoCapacity;
            fireRate = 1 / shotsPerSecond;

            shotOrigin = GetComponentInParent<Camera>().transform;
            OnAmmoChange?.Invoke(currentAmmo);

            MethodLinker.Instance.LinkToHudAmmo(ref OnAmmoChange);
            MethodLinker.Instance.LinkToHudCrosshair(ref deviation.OnDeviationChange);
            deviation.ResetDeviation();
        }
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            ApplyAutoFire();
            deviation.UpdateDeviation();
        }
    }

    /// <summary>
    /// Applies autofire if the player is holding the fire button.
    /// </summary>
    private void ApplyAutoFire()
    {
        if (isFiring && Time.time > nextShotTime && canFire)
        {
            if (currentAmmo <= 0)
                return;
            animator.SetTrigger(fireHash);
            Shoot();
            photonView.RPC("ShootRpc", RpcTarget.Others);
            OnAmmoChange?.Invoke(currentAmmo);
        }
    }

    public void SetReloaded()
    {
        currentAmmo = ammoCapacity;
        canFire = true;
        audioController.PlaySound(reloadClip, 0.4f);
        OnAmmoChange?.Invoke(currentAmmo);
    }

    protected void PlayParticleSystem(RaycastHit hit)
    {
        //ParticleSystem pSystem = sparkObjectPool.GetPooledObject();
        //pSystem.transform.position = hit.point;
        //pSystem.transform.rotation = Quaternion.LookRotation(hit.normal);
        //pSystem.gameObject.SetActive(true);
        //pSystem.Play();
    }

    protected void RandomMuzzleRotation()
    {
        muzzleFlashTransform.localRotation = Quaternion.Euler(0, 0, Random.value * 360);
    }

    #region Inputs

    private void OnFire(InputValue value)
    {       
        isFiring = value.Get<float>() == 1.0f && (Time.time > nextShotTime || isAutoFire);
    }

    private void OnReload()
    {
        if(currentAmmo < ammoCapacity)
        {
            isReloading = true;
            photonView.RPC("ReloadRpc", RpcTarget.Others);
            animator.SetTrigger(reloadHash);
        }
    }

    #endregion

    /// <summary>
    /// Applies shot logic.
    /// </summary>
    protected abstract void Shoot();

    public void ShootRpcLink()
    {
        RandomMuzzleRotation();
        animator.SetTrigger(fireHash);
        audioController.PlaySound(gunShotClip, 0.4f);
    }

    public void ReloadRpcLink()
    {
        animator.SetTrigger(reloadHash);
    }

    public void ResetWeapon()
    {
        currentAmmo = ammoCapacity;
        isFiring = false;
        canFire = true;
    }

    private void OnDrawGizmos()
    {
        if (displayGizmos)
        {
            //for spread testing purposes
            Gizmos.color = Color.green;
            Gizmos.DrawRay(shotOrigin.position, shotOrigin.rotation * (Vector3.forward + Vector3.up * deviation) * 3);
            Gizmos.DrawRay(shotOrigin.position, shotOrigin.rotation * (Vector3.forward + Vector3.down * deviation) * 3);
            Gizmos.DrawRay(shotOrigin.position, shotOrigin.rotation * (Vector3.forward + Vector3.left * deviation) * 3);
            Gizmos.DrawRay(shotOrigin.position, shotOrigin.rotation * (Vector3.forward + Vector3.right * deviation) * 3);
        }
    }
}
