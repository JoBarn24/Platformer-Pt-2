using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float scrollSpeed = 5f;
    
    private int timeLeft;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        timeLeft = 300 - (int)Time.time;
        timerText.text = $"Time: {timeLeft}";

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mainCamera.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            mainCamera.transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
        }
    }
}
