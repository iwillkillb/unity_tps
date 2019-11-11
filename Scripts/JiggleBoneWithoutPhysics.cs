using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleBoneWithoutPhysics : MonoBehaviour
{
    public struct Bone
    {
        public Transform trn;
        public Vector3 initRot;
    }

    // All bones
    public Bone[] bones;

    // Rotation speed of bones
    public float rotSpeedBone = 2f;

    // Previous position
    Vector3 prePos;

    // Change between current position and previous position
    Vector3 changePos;

    float ratioChangeRot = 0f;



    // Initialize bones
    void Start()
    {
        // Queue of bones -> Array of bones
        Queue<Transform> boneQueue = new Queue<Transform>();

        // Queue[0] = transform
        Transform curTrn = transform;
        boneQueue.Enqueue(curTrn);

        // Queue[n+1] has child bone -> Enqueue child bone
        while (curTrn.childCount > 0)
        {
            curTrn = curTrn.GetChild(0);
            boneQueue.Enqueue(curTrn);
        }

        // Save queue's data to bones.
        bones = new Bone[boneQueue.Count];
        int boneCount = boneQueue.Count;
        for (int i=0; i< boneCount; i++)
        {
            bones[i].trn = boneQueue.Dequeue();
            bones[i].initRot = bones[i].trn.eulerAngles;
        }



        // Start checking position (Period : 0.1 sec)
        InvokeRepeating("CheckPosition", 0f, 0.1f);
    }

    void CheckPosition ()
    {
        prePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        changePos = prePos - transform.position;
        
        ratioChangeRot = Mathf.Lerp(0f, changePos.magnitude, Time.deltaTime * rotSpeedBone);

        float angleX = Mathf.Atan2(changePos.y, changePos.x) * Mathf.Rad2Deg;
        float angleZ = Mathf.Atan2(changePos.y, changePos.z) * Mathf.Rad2Deg;

        /*
        foreach (Bone bone in bones)
        {
            bone.trn.eulerAngles = Vector3.Lerp(bone.initRot, bone.initRot + new Vector3(angleX, 0f, angleZ), ratioChangeRot);
        }*/

        Debug.Log(changePos + " --- " + new Vector3(angleX, 0f, angleZ));
    }
}
