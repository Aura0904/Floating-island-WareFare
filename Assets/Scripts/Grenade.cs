using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : MonoBehaviourPunCallbacks
{
    public float delay = 3;
    public float Radius = 5f;
    public float Force = 700f;
    public float damage;
    

    public GameObject explosionEffect;

    float countDown;
    bool hasExploded = false;

    PhotonView pv;


    void Start()
    {
        countDown = delay;
    }

    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0f && hasExploded == false)
        {
            Exploded();
        }
    }

    void Exploded()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, Radius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(Force, transform.position, Radius, damage);
                IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);               

                }

            }

        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, Radius);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(Force, transform.position, Radius);
                
            }
        }

        Destroy(gameObject);

    }

}





