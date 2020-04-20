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

    [Header("Main Properties (No NULL)")]
    public Transform target;
    public LayerMask terrainLayerMask;
    RaycastHit hit;
    Transform trnCam;               // Camera's moving position by zooming

    [Header("FPS Mode")]
    public Transform trnFPS;

    // Input
    float inputAxisX;
    float inputAxisY;
    float inputAxisZ;

    // Camera's rotation sequence
    // Angle -> Quaternion -> Transform
    [Header("Rotation")]
    public float rotationSpeed = 2f;
    [Range(-180f, 180f)] public float minXAxis = -45f;           // X Axis has limit.
    [Range(-180f, 180f)] public float maxXAxis = 75f;
    float angleYAxis;
    float angleXAxis;
    Transform trnRot;

    [Header("Zoom")]
    [Range(0f, 1f)] public float zoomAxisMin = 0f;
    [Range(0f, 1f)] public float zoomAxisMax = 1f;
    float zoomAxis = 1f;                    // Default zoom axis, Nearest : 0
    float lengthOfZoomLine;                 // Length of camera's zooming line
    Transform trnZoomNearest;               // Camera's nearest position
    Transform trnZoomFarest;                // Camera's farest position



    // Start is called before the first frame update
    void Awake()
    {
        trnRot = transform.GetChild(0);
        trnCam = trnRot.GetComponentInChildren<Camera>().transform;
        trnZoomFarest = trnRot.Find("Farest");
        trnZoomNearest = trnRot.Find("Nearest");

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
        angleYAxis += inputAxisX * rotationSpeed;
        angleXAxis -= inputAxisY * rotationSpeed;
        angleXAxis = Mathf.Clamp(angleXAxis, minXAxis, maxXAxis);
        trnRot.localRotation = Quaternion.Euler(angleXAxis, angleYAxis, 0f);

        // FPS Mode
        if (trnFPS != null)
        {
            trnCam.position = trnFPS.position;
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
        zoomAxis = Mathf.Clamp(zoomAxis - inputAxisZ, zoomAxisMin, zoomAxisMax);

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
