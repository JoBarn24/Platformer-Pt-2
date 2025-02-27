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
    
    private int timeLeft = 100;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0 && gameOver == false)
        {
            timeLeft = 100 - (int)Time.time;
            timerText.text = $"Time: {timeLeft}";
        }
        else if (timeLeft <= 0 && gameOver == false)
        {
            gameOver = true;
            PlayerLost();
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
        timeLeft = 100;
    }

    public void PlayerLost()
    {
        Debug.Log("Player lost!");
        StartCoroutine(Reload());
        timeLeft = 100;
        gameOver = false;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(5f);
        levelParser.ReloadLevel();
    }
}
