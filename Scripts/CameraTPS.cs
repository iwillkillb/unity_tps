using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTPS : MonoBehaviour
{
    // How to Use?
    // 1. Make this hierarchy
    //  Component's object
    //      > Y Axis (0, 0, 0) All Zero Position
    //          > X Axis (0, 0, 0) All Zero Position
    //              > Main Camera
    //              > Zoom out limit (x, y, z) Free Position
    // 2. Set Target and other public objects.
    
    [Header("Main Properties")]
    public Transform target;
    public float rotationSpeed = 2f;
    public LayerMask terrainLayerMask;
    RaycastHit hit;

    // Camera's rotation sequence
    // Angle -> Quaternion -> Transform

    [Header("X Axis rotation")]
    public Transform trnXAxisRot;           // This rotates by X Axis only.
    float angleXAxis;
    Quaternion qutXAxis;
    public float minXAxis = -45f;           // X Axis has limit.
    public float maxXAxis = 75f;

    [Header("Y Axis rotation")]
    public Transform trnYAxisRot;           // This rotates by Y Axis only.
    float angleYAxis;
    Quaternion qutYAxis;

    [Header("Zoom")]
    public Transform trnZoom;               // Camera's moving position by zooming
    public Transform trnZoomLimit;          // Camera's farest position
    [Range(0f, 1f)]public float minZoomAxisWithoutCollision = 0.2f;
    float zoomAxisWithoutCollision = 1f;    // Default zoom axis, Nearest : 0
    float zoomAxisWithCollision = 1f;       // When Camera's zoom line collide with Terrain or something.
    float lengthOfZoomLine;                 // Length of camera's zooming line



    // Start is called before the first frame update
    void Awake()
    {
        qutXAxis = trnXAxisRot.localRotation;
        qutYAxis = trnYAxisRot.localRotation;

        lengthOfZoomLine = Vector3.Distance(trnZoomLimit.position, trnZoomLimit.parent.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Positioning
        transform.position = target.position;



        // Camera Stop
        if (Input.GetKey(KeyCode.LeftShift))
            return;



        // Input
        float inputAxisX = Input.GetAxis("Mouse X");
        float inputAxisY = Input.GetAxis("Mouse Y");
        float inputAxisZ = Input.GetAxis("Mouse ScrollWheel");



        // Y Axis Rotation
        angleYAxis += inputAxisX * rotationSpeed;
        qutYAxis = Quaternion.Euler(0f, angleYAxis, 0f);

        // X Axis Rotation (with clamping)
        angleXAxis -= inputAxisY * rotationSpeed;
        angleXAxis = Mathf.Clamp(angleXAxis, minXAxis, maxXAxis);
        qutXAxis = Quaternion.Euler(angleXAxis, 0f, 0f);

        // Actual Rotation
        trnXAxisRot.localRotation = qutXAxis;
        trnYAxisRot.localRotation = qutYAxis;



        // Camera Collision Check and Zoom
        // 1. Input
        // 2. Check collider in camera zoom line (Zoom Limit ~ Zoom Limit's Parent)
        // 3. Compare Collision zoom axis to Default zoom axis.
        // 4. Select lesser one.

        // Take Input.
        zoomAxisWithoutCollision = Mathf.Max(Mathf.Clamp(zoomAxisWithoutCollision - inputAxisZ, 0f, 1f), minZoomAxisWithoutCollision);

        // Is there something(Without itself) between camera and character?
        // Don't put Target's layer in terrainLayerMask!!
        if (Physics.Linecast(trnZoomLimit.parent.position, trnZoomLimit.position, out hit, terrainLayerMask))
        {
            // There is something. -> Use lesser zoom axis.
            zoomAxisWithCollision = Mathf.Min(Mathf.Clamp(hit.distance / lengthOfZoomLine, 0f, 1f), zoomAxisWithoutCollision);
            trnZoom.localPosition = Vector3.Lerp(trnZoomLimit.parent.localPosition, trnZoomLimit.localPosition, zoomAxisWithCollision);
        }
        else
        {
            // Nothing. -> Use input value (zoomAxisWithoutCollision).
            trnZoom.localPosition = Vector3.Lerp(trnZoomLimit.parent.localPosition, trnZoomLimit.localPosition, zoomAxisWithoutCollision);
        }
    }
}
