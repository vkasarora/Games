using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;  //approximate time req to move camera from one position to another               
    public float m_ScreenEdgeBuffer = 4f;  // keeps tanks on the edge without tripping from the  boundry          
    public float m_MinSize = 6.5f;  //  prevents camera to get very far, making everything look small                
    /*[HideInInspector]*/ public Transform[] m_Targets; 


    private Camera m_Camera;   //reference to camera
    private float m_ZoomSpeed;  //damp the zoom
    private Vector3 m_MoveVelocity;   //damp the caamera move velocity     
    private Vector3 m_DesiredPosition;   // position camera wants to reach, which is average of the tanks position. Zoom is also based on this


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>(); //get camera which is child of the current object. This is useful only when we have only one child.
    }


    private void FixedUpdate()
    {
        Move();  //moves the camera
        Zoom();  
    }


    private void Move()
    {
        FindAveragePosition();  //find avg positions between the tank

        //move smoothly from transform.position to m_DesiredPosition with m_MoveVelocity speed. ref keyword writes back to the m_MoveVelocity variable
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); 
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3(); //create a new blank vector3 variable
        int numTargets = 0;  //number of target we are averaging over

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf) //if game is not active
                continue;

            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
        }
        
        size += m_ScreenEdgeBuffer;

        size = Mathf.Max(size, m_MinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}