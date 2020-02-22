using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
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
    public Transform trnCam;               // Camera's moving position by zooming
    public Transform target;
    public LayerMask terrainLayerMask;
    RaycastHit hit;

    [Header("FPS Mode")]
    public bool isCamFPS = false;
    public Transform trnFPS;

    // Input
    float inputAxisX;
    float inputAxisY;
    float inputAxisZ;

    // Camera's rotation sequence
    // Angle -> Quaternion -> Transform
    [Header("Rotation")]
    public Transform trnRot;
    public float rotationSpeed = 2f;
    public float minXAxis = -45f;           // X Axis has limit.
    public float maxXAxis = 75f;
    float angleYAxis;
    float angleXAxis;

    [Header("Zoom")]
    public Transform trnZoomNearest;        // Camera's nearest position
    public Transform trnZoomFarest;         // Camera's farest position
    float zoomAxis = 1f;                    // Default zoom axis, Nearest : 0
    float lengthOfZoomLine;                 // Length of camera's zooming line



    // Start is called before the first frame update
    void Awake()
    {
        lengthOfZoomLine = Vector3.Distance(trnZoomFarest.position, trnZoomNearest.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Positioning
        transform.position = target.position;

        // Shift Key -> Camera Stop
        if (Input.GetKey(KeyCode.LeftShift))
            return;

        // Take input
        inputAxisX = Input.GetAxis("Mouse X");
        inputAxisY = Input.GetAxis("Mouse Y");
        inputAxisZ = Input.GetAxis("Mouse ScrollWheel");

        // XY Axis Rotation (with clamping)
        if (trnRot != null)
        { 
            angleYAxis += inputAxisX * rotationSpeed;
            angleXAxis -= inputAxisY * rotationSpeed;
            angleXAxis = Mathf.Clamp(angleXAxis, minXAxis, maxXAxis);
            trnRot.localRotation = Quaternion.Euler(angleXAxis, angleYAxis, 0f);
        }

        // FPS Mode
        if (isCamFPS)
        {
            trnCam.position = (trnFPS != null) ? trnFPS.position : transform.position;
            return;
        }

        Zoom();
    }

    void Zoom ()
    {
        // Camera Collision Check and Zoom
        // 1. Input
        // 2. Check collider in camera zoom line (Zoom Limit ~ Zoom Limit's Parent)
        // 3. Compare Collision zoom axis to Default zoom axis.
        // 4. Select lesser one.

        // Take Input.
        zoomAxis = Mathf.Clamp(zoomAxis - inputAxisZ, 0f, 1f);

        // Is there something(Without itself) between camera and character?
        // Don't put Target's layer in terrainLayerMask!!
        if (Physics.Linecast(trnZoomNearest.position, trnZoomFarest.position, out hit, terrainLayerMask))
        {
            // Lesser Zoom Axis -> Near
            trnCam.localPosition = Vector3.Lerp(trnZoomNearest.localPosition, trnZoomFarest.localPosition, Mathf.Min(zoomAxis, hit.distance / lengthOfZoomLine));
        }
        else
        {
            // Nothing. -> Use input value (zoomAxis).
            trnCam.localPosition = Vector3.Lerp(trnZoomNearest.localPosition, trnZoomFarest.localPosition, zoomAxis);
        }
    }
}