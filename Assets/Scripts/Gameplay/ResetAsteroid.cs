using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAsteroid : MonoBehaviour
{
    [SerializeField] float maxDistance = 2;
    [SerializeField] bool hide = true;
    void Update()
    {
        //if (hide)
        //{
            if (PlayerController.instance.transform.position.z - transform.position.z >= maxDistance)
            {
                gameObject.SetActive(false);
            }
        //}
        //else
        //{
        //    if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= maxDistance)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }
}
