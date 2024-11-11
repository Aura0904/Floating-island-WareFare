using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : Item
{
    const float defaultFOV = 75f; //set this to whatever your game's base FOV is; it's used (along with cam.fieldOfView) to adjust spread so that lower FOVs do not result in increase accuracy.
    [SerializeField] Camera cam;
    [SerializeField] GameObject bulletImpactPrefab;
    GunInfo gun;
    float lastShot;
    public float range = 100f;

    public TextMeshProUGUI ammoInfoText;


    public int maxAmmo = 30;
    public int currentAmmo;
    public int MagSize = 30;
    

    public int reserveAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    public Animator animator;



    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        gun = itemInfo as GunInfo;



    }



    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Update()
    {


        if (isReloading)
            return;


        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    public override void Use()
    {
        Shoot();
    }

    public override void UseRepeating()
    {
        if (!(gun.automatic))
            return;
        Shoot();
    }

    protected virtual void Shoot()
    {
        if (currentAmmo <= 0)
            return;
        currentAmmo--;
        ammoInfoText.text = currentAmmo + " / " + MagSize;


        if (Time.time < lastShot + gun.firerate)
            return;

        for (int i = 0; i < gun.pelletsPerAttack; i++)
        {
            Vector2 spread = Random.insideUnitCircle * GetSpread();
            Ray ray = cam.ViewportPointToRay(new Vector2(0.5f, 0.5f) + spread / (cam.fieldOfView / defaultFOV));
            ray.origin = cam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
            }
        }
        lastShot = Time.time;


    }

IEnumerator Reload()
{
    isReloading = true;
    animator.SetBool("Reloading", true);

    yield return new WaitForSeconds(reloadTime);

    int ammoNeeded = maxAmmo - currentAmmo;

    if (MagSize >= ammoNeeded)
    {
        currentAmmo += ammoNeeded;
        MagSize -= ammoNeeded;
    }
    else
    {
        currentAmmo += MagSize;
        MagSize = 0;
    }

    isReloading = false;
    animator.SetBool("Reloading", false);

    // Update ammo text UI
    ammoInfoText.text = currentAmmo + " / " + MagSize;
}

    protected virtual float GetSpread() //this function is here in case you want to override it to modify spread
    {
        return gun.spread;
    }



    [PunRPC]
    protected virtual void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 5f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }


}
