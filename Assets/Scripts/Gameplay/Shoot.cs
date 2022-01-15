using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    ObjectPool objectPool;


    void Start()
    {
        objectPool = ObjectPool.Instance;
    }

    public void FireWeapon()
    {
        objectPool.SpawnObject("Laser", transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
    }
}
