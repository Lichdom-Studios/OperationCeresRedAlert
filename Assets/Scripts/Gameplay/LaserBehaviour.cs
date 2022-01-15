using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public float speed = 100f;
    [SerializeField] float maxRange = 100f;
    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= maxRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);

            yield return null;
        }

        gameObject.SetActive(false);

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WholeAsteroid")
        {
            gameObject.SetActive(false);
        }
    }
}
