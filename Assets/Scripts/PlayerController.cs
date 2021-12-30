using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;

    void Start()
    {
        GameManager.OnGameStateChange += CheckGameState;
    }
    void CheckGameState(GameState state)
    {

        if (state == GameState.PLAY)
            StartCoroutine(MoveForward());
        if (state == GameState.GAMEOVER)
            GameManager.OnGameStateChange -= CheckGameState;
    }
    IEnumerator MoveForward()
    {
        while (GameManager.instance.GetGameState() == GameState.PLAY)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, movementSpeed * Time.deltaTime);
            yield return null;
        }

        yield break;
    }
}
