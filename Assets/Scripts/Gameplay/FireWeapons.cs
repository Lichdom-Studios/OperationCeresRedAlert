using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireWeapons : MonoBehaviour
{
    [SerializeField] List<Shoot> weapons;
    [SerializeField] float fireRate = 0.5f;
    WaitForSeconds delay;
    int index = 0;

    [SerializeField] Slider overheatBar;
    [Range(0f, 1f)]
    [SerializeField] float heatValue = 0.05f, cooldownRate = 0.5f;
    float heatLevel = 0f;

    [SerializeField] AudioSource audio;

    bool overheated = false, cooldown = false;

#if UNITY_STANDALONE_WIN
    public static FireWeapons instance;
    void Start()
    {
        delay = new WaitForSeconds(fireRate);

        if (!audio)
            audio = GetComponent<AudioSource>();

        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
#else
  void Start()
    {
        delay = new WaitForSeconds(fireRate);

        if (!audio)
            audio = GetComponent<AudioSource>();
    }
#endif


    public void BeginFire()
    {
        if (!overheated && GameManager.instance.GetGameState() == GameState.PLAY)
        {
            cooldown = false;
            StopCoroutine("Cooldown");
            StartCoroutine("Fire");
        }
    }

    public void EndFire()
    {
        if (!overheated)
        {
            StopCoroutine("Fire");
            cooldown = true;
            StartCoroutine("Cooldown");
        }
    }

    void IncreaseHeat()
    {
        heatLevel += heatValue;
        overheatBar.value = heatLevel;

        if(overheatBar.value >= overheatBar.maxValue)
        {
            overheated = true;            
        }
    }
  
    IEnumerator Fire()
    {
        while(!overheated)
        {
            weapons[index].FireWeapon();

            float pitch = 0.9f + Random.Range(-0.1f, 0f);
            audio.pitch = pitch;
            audio.Play();
            
            ++index;

            if (index >= weapons.Count)
                index = 0;

            IncreaseHeat();

            yield return delay;
            yield return null;
        }

        StartCoroutine("Cooldown");

        yield break;
    }

    IEnumerator Cooldown()
    {
        while (overheatBar.value > overheatBar.minValue)
        {
            heatLevel = Mathf.Lerp(heatLevel, -0.1f, Time.deltaTime * cooldownRate);
            overheatBar.value = heatLevel;

            yield return null;
        }

        overheated = false;
        yield break;
    }
}
