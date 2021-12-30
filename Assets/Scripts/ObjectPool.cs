using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instace;

    void Awake()
    {
        if (!instace)
            instace = this;
        else
            Destroy(gameObject);
    }
}
