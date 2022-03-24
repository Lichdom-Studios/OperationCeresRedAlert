using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PhotonBlast : MonoBehaviour
{
    public float startSpeed = 10f;
    [SerializeField] float maxDuration = 3f;
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

        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        animationSequence = DOTween.Sequence().Pause();
        animationSequence.Insert(0, transform.DOScale(75f, 3f).SetAutoKill(false));
        animationSequence.Insert(0, coreMaterial.DOFade(0f, 1.5f).SetAutoKill(false));
        animationSequence.Insert(0, material.DOFade(0f, 3f).SetAutoKill(false).OnComplete(ResetPhotonBlast));

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

        float duration = maxDuration;

        float speed = startSpeed + PlayerController.instance.movementSpeed;
        while (duration > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * speed);

            duration -= Time.deltaTime;

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
