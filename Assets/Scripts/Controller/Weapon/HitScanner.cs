//Author: Dominik Dohmeier

using UnityEngine;

class HitScanner : WeaponController
{
    /// <summary>
    /// Fires a single raycast to damage enemies.
    /// Manages spread and values for the next shot.
    /// </summary>
    protected override void Shoot()
    {
        SetShotValues();        
        //audioHandler.PlaySound(gunShotClip, 0.4f);
        RandomMuzzleRotation();
        HitScan();       
        deviation.IncreaseDeviation();
    }

    private void SetShotValues()
    {
        currentAmmo -= ammoUsePerShot; //TODO add logic here
        nextShotTime = Time.time + fireRate;
        isFiring = isAutoFire;
        isReloading = false;
    }

    private void HitScan()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            //Calculate Spread
            Vector3 shotDirection = shotOrigin.rotation * deviation.GetRandomSpreadVector();

            Debug.DrawRay(shotOrigin.position, shotDirection * 50, Color.red, 5); //for spread testing purposes

            if (Physics.Raycast(shotOrigin.position, shotDirection, out RaycastHit hit, range, hitLayers, QueryTriggerInteraction.Ignore))
            {
                PlayParticleSystem(hit);
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHealth enemy = hit.collider.GetComponent<PlayerHealth>();
                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}
