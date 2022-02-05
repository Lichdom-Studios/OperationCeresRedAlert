using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    public float startSpeed = 100f;
    [SerializeField] float maxRange = 100f;
    [SerializeField] int points = 3;
    [SerializeField] int photonCharge = 5;
    private void OnEnable()
    {
        if(GameManager.instance.GetGameState() == GameState.PLAY)
            StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float speed = startSpeed + PlayerController.instance.movementSpeed;
        while (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= maxRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);

            yield return null;
        }

        gameObject.SetActive(false);

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WholeAsteroid" && GameManager.instance.GetGameState() == GameState.PLAY)
        {
            Player.instance.AddToScore(points);
            FireWeapons.instance.ChargePhotonCannon(photonCharge);
            gameObject.SetActive(false);
        }

    }
}
