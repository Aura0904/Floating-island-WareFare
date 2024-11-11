using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunInfo : ItemInfo
{
    public float damage;
    public float firerate = 0.1f;
    
    public float spread = 0.001f; //a spread of 0.5, for example, means the bullets can hit anywhere on the screen.
    public int pelletsPerAttack = 1;
    public bool automatic;
    



    protected GunInfo gunInfo;

    
    




}
