using System;
using TMPro;
using UnityEngine;

public class MarioBehaviorScript : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 20f;
    public float jumpBoostForce = 5.7f;
    public int coinCount = 0;
    public TextMeshProUGUI coinCounter;
    public int scoreCount = 0;
    public TextMeshProUGUI scoreCounter;
    public GameManager gameManager;
    
    [Header("Debug Stuff")]
    public bool isGrounded;
    
    Animator animator;
    Rigidbody rb;
    private Vector3 initialPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAmount = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * horizontalAmount * Time.deltaTime * acceleration;
        
        float horizontalSpeed = rb.linearVelocity.x;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);

        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalSpeed;
        rb.linearVelocity = newVelocity;
        
        //should also clamp vertical velocity
        
        //test if character on ground surface
        Collider c = GetComponent<Collider>();
        float castDistance = c.bounds.extents.y + 0.1f;
        Vector3 startPoint = transform.position;
        
        Color color = (isGrounded) ? Color.green : Color.red;
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, depthTest: false);
        
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //apply an impulse force upward
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
            }
        }

        if (horizontalAmount == 0f)
        {
            Vector3 decayedVelocity = rb.linearVelocity;
            newVelocity.x *= 1f - Time.deltaTime * 2f;
            rb.linearVelocity = decayedVelocity;
        }
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }
        
        //break brick when character hits it
        Vector3 headPosition = transform.position + Vector3.up * 1f;
        RaycastHit headHit;
        Debug.DrawRay(headPosition, Vector3.up * castDistance, Color.red);
        
        if (Physics.Raycast(headPosition, Vector3.up, out headHit, castDistance))
        {
            if (headHit.collider.CompareTag("Brick"))
            {
                Destroy(headHit.collider.gameObject);
                Debug.Log("Brick broken!");
                scoreCount += 100;
                scoreCounter.text = scoreCount.ToString().PadLeft(6,'0');
            }
            if (headHit.collider.CompareTag("Question"))
            {
                Destroy(headHit.collider.gameObject);
                Debug.Log("Question block broken!");
                coinCount += 1;
                scoreCount += 100;
                coinCounter.text = coinCount.ToString().PadLeft(2,'0');
                scoreCounter.text = scoreCount.ToString().PadLeft(6,'0');
            }
        }
        
        //break brick when mouse clicks it
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit) && hit.collider != null)
            {
                if (hit.collider.CompareTag("Brick"))
                {
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Question"))
                {
                    coinCount += 1;
                    coinCounter.text = "x" + coinCount.ToString().PadLeft(2,'0');
                }
            }
        }
        
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        animator.SetFloat("Speed",Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("In Air", !isGrounded);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            transform.position = initialPosition;
            coinCount = 0;
            scoreCount = 0;
            coinCounter.text = coinCount.ToString().PadLeft(2,'0');
            scoreCounter.text = scoreCount.ToString().PadLeft(6,'0');
            gameManager.PlayerLost();
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            transform.position = initialPosition;
            gameManager.PlayerWon();
        }
    }
}