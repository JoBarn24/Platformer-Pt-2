using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float scrollSpeed = 5f;
    public bool gameOver = false;
    public LevelParser levelParser;
    
    private float timeLeft = 100;
    private Camera mainCamera;
    private Vector3 initialPosition;

    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = mainCamera.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = $"Time: {Math.Ceiling(timeLeft)}";
            }
            else
            {
                gameOver = true;
                PlayerLost();
            }
        }
        else
        {
            timerText.text = $"Time: {Math.Ceiling(timeLeft)}";
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mainCamera.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            mainCamera.transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        }
    }

    public void PlayerWon()
    {
        Debug.Log("Player won!");
        StartCoroutine(Reload());
    }

    public void PlayerLost()
    {
        Debug.Log("Player lost!");
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        mainCamera.transform.position = initialPosition;
        yield return new WaitForSeconds(1f);
        levelParser.ReloadLevel();
        timeLeft = 100f;
        gameOver = false;
    }
}
