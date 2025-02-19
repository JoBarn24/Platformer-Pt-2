using UnityEngine;

public class MarioBehaviorScript : MonoBehaviour
{
    public float acceleration = 10f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    public float jumpBoostForce = 5.7f;
    
    [Header("Debug Stuff")]
    public bool isGrounded;
    
    Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        float castDistance = c.bounds.extents.y + 0.01f;
        Vector3 startPoint = transform.position;
        
        Color color = (isGrounded) ? Color.green : Color.red;
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, depthTest: false);
        
        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //apply an impulse force updward
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
    }
}