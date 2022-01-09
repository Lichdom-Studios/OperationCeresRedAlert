using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float maxSpeed = 15f;
    public float movementSpeed = 5f;
    public float rotationSpeed = 10f;
    public float speedIncreaseAmount = 0.1f;
    public float difficultyChangeRate = 10f;

    WaitForSeconds difficultyDuration;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        GameManager.OnGameStateChange += CheckGameState;

        difficultyDuration = new WaitForSeconds(difficultyChangeRate);

        Input.gyro.enabled = true;
    }
    void CheckGameState(GameState state)
    {

        if (state == GameState.PLAY)
        {
            StartCoroutine(MoveForward());
            StartCoroutine(RotatePlayer());
            StartCoroutine(DifficultyChange());
        }
        if (state == GameState.GAMEOVER)
            GameManager.OnGameStateChange -= CheckGameState;
    }
    IEnumerator MoveForward()
    {
        while (GameManager.instance.GetGameState() == GameState.PLAY)
        {
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, transform.position + transform.forward, movementSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(targetPosition.x, -7, 7);
            float clampedY = Mathf.Clamp(targetPosition.y, -7, 7);

            Vector3 newPosition = new Vector3(clampedX, clampedY, targetPosition.z);

            transform.position = newPosition;
            yield return null;
        }

        yield break;
    }

    IEnumerator RotatePlayer()
    {
        while (GameManager.instance.GetGameState() == GameState.PLAY)
        {
            Vector3 gyro = Input.gyro.rotationRateUnbiased;

            Vector3 newRotation = new Vector3(transform.rotation.x + -(gyro.x * Mathf.Rad2Deg * Time.deltaTime * rotationSpeed), transform.rotation.y + -(gyro.y * Mathf.Rad2Deg * Time.deltaTime * rotationSpeed), transform.rotation.z);

            transform.Rotate(newRotation);

            //float clampedX = Mathf.Clamp(transform.eulerAngles.x, -25f, 25f);
            //float clampedY = Mathf.Clamp(transform.eulerAngles.y, -25f, 25f);

            //Quaternion clampedRot = Quaternion.Euler(clampedX, clampedY, newRotation.z);

            transform.rotation = ClampRotation(transform.rotation, new Vector2(25,25));

            yield return null;
        }

        yield break;
    }

     Quaternion ClampRotation(Quaternion q, Vector2 bounds)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        return q;
    }

    IEnumerator DifficultyChange()
    {
        while(GameManager.instance.GetGameState() == GameState.PLAY && movementSpeed < maxSpeed)
        {
            yield return difficultyDuration;

            movementSpeed += speedIncreaseAmount;

            yield return null;
        }

        yield break;
    }
}