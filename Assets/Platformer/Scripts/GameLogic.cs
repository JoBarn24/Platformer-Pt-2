using System.Collections;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject parachuteObj;
    public GameObject packageObj;
    public Transform parachutePivot;
    public Transform debugSphereTransform;
    public Camera cam;
    public float chuteOpenDuration;
    
    public float parachuteOpenDistance = 5f;
    public float parachuteDrag = 7f;
    
    float defaultParachuteDrag;
    bool hasParachuteOpened = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultParachuteDrag = parachuteObj.GetComponent<Rigidbody>().linearDamping;
        parachuteObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray lookForGroundRay = new Ray(parachuteObj.transform.position, Vector3.down);
        if (Physics.Raycast(lookForGroundRay, out RaycastHit hitInfo))
        {
            bool chuteOpen = (hitInfo.distance < parachuteOpenDistance);
            
            //draw a colored line from the payload downward by the distance from ground where we open parachute
            Color lineColor = (hitInfo.distance< parachuteOpenDistance) ? Color.red : Color.blue;
            Debug.DrawRay(parachuteObj.transform.position, Vector3.down * parachuteOpenDistance, Color.blue);
            
            //use parachute drag
            packageObj.GetComponent<Rigidbody>().linearDamping = (chuteOpen) ? parachuteDrag : defaultParachuteDrag;
            parachuteObj.SetActive(true);
            
            if (chuteOpen && !hasParachuteOpened)
            {
                StartCoroutine(AnimateParachuteOpen());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            
            //create a ray that goes through the screen position using a camera
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);
            if (rayHitSomething && screenHitInfo.transform.gameObject.tag == "Brick")
            {
                debugSphereTransform.position = screenHitInfo.point;
            }
        }
    }
    
    IEnumerator AnimateParachuteOpen()
    {
        float timeElapsed = 0f;
        parachutePivot.localScale = Vector3.zero;

        while (timeElapsed < chuteOpenDuration)
        {
            float percentComplete = timeElapsed / chuteOpenDuration;
            parachutePivot.localScale = new Vector3(percentComplete, percentComplete, percentComplete);
            
            yield return null;
            
            timeElapsed += Time.deltaTime;
        }
    }
}
