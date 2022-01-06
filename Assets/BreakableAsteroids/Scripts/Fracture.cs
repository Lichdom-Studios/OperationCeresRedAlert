using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [Tooltip("\"Fractured\" is the object that this will break into")]
    public GameObject fractured;

    public void FractureObject()
    {
        GameObject obj = Instantiate(fractured, transform.position, transform.rotation); //Spawn in the broken version

        foreach (Transform child in obj.transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(200f, 500f), transform.position, Random.Range(8f, 10f));
            //Destroy(gameObject, 3.5f);
        }

        gameObject.SetActive(false);

    }
}
