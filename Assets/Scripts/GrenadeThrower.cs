using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrenadeThrower : MonoBehaviourPunCallbacks
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;
   public PhotonView PV;

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
       GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}


