using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField] float speed = .15f;
    [SerializeField] float rotationSpeeds = .25f;
    [SerializeField] Fracture fracture;
    void OnEnable()
    {
        if (!fracture)
            fracture = GetComponent<Fracture>();


        if(Random.value > 0.5f)
            StartCoroutine(MoveAsteroid());

        StartCoroutine(RotateAsteroid());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator MoveAsteroid()
    {
        Vector3 randomSpeed = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        while(isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + randomSpeed, Time.deltaTime * speed);

            yield return null;
        }

        yield break;
    }

    IEnumerator RotateAsteroid()
    {
        Vector3 randomSpeed = new Vector3(Random.Range(-rotationSpeeds, rotationSpeeds), Random.Range(-rotationSpeeds, rotationSpeeds), Random.Range(-rotationSpeeds, rotationSpeeds));

        while (isActiveAndEnabled)
        {
            transform.Rotate(randomSpeed);

            yield return null;
        }

        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && fracture)
        {
            fracture.FractureObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && fracture)
        {
            fracture.FractureObject();
        }
    }

}
