using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PhotonBlast : MonoBehaviour
{
    public float startSpeed = 10f;
    [SerializeField] float maxRange = 100f;
    [SerializeField] int points = 3;
    Sequence animationSequence;
    [SerializeField] Material material, coreMaterial;    
    DOTweenAnimation cameraAnimation;

    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip fireSound, explosionSound;
    private void OnEnable()
    {
        Color photonColor = material.color;
        photonColor.a = 1f;
        Color coreColor = coreMaterial.color;
        coreColor.a = 1f;

        material.color = photonColor;
        coreMaterial.color = coreColor;

        animationSequence = DOTween.Sequence().Pause();
        animationSequence.Insert(0, transform.DOScale(100f, 2f));
        animationSequence.Insert(0, coreMaterial.DOFade(0f, 2f));
        animationSequence.Insert(0, material.DOFade(0f, 2f).OnComplete(ResetPhotonBlast));

        if (!cameraAnimation)
            cameraAnimation = Camera.main.GetComponent<DOTweenAnimation>();

        if (!audio)
            audio = GetComponent<AudioSource>();

        if(GameManager.instance.GetGameState() == GameState.PLAY)
            StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        if(audio && fireSound)
        {
            audio.clip = fireSound;
            audio.Play();
        }

        float speed = startSpeed + PlayerController.instance.movementSpeed;
        while (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= maxRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);

            yield return null;
        }

        PhotonExplosion();

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
 

    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }

        if (collision.gameObject.tag == "WholeAsteroid" && GameManager.instance.GetGameState() == GameState.PLAY)
        {
            PhotonExplosion();
            Player.instance.AddToScore(points);
        }
    }

    void PhotonExplosion()
    {
        if (!animationSequence.IsPlaying())
        {
            if (audio && explosionSound)
            {
                audio.clip = explosionSound;
                audio.Play();
            }

            animationSequence.Play();
            cameraAnimation.DOPlayForward();
        }
        
    }

    void ResetPhotonBlast()
    {         
        gameObject.SetActive(false);
    }
}
