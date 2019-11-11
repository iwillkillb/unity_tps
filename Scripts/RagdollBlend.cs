using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBlend : MonoBehaviour
{
    [Range(0f, 1f)]
    public float blend = 0f;

    public GameObject objRagdoll;
    public GameObject objAnimator;

    public string objNamePelvis = "Hips";
    public string objNameLeftHips = "Left leg";
    public string objNameLeftKnee = "Left knee";
    public string objNameLeftFoot = "Left ankle";
    public string objNameRightHips = "Right leg";
    public string objNameRightKnee = "Right knee";
    public string objNameRightFoot = "Right ankle";
    public string objNameLeftArm = "Left arm";
    public string objNameLeftElbow = "Left elbow";
    public string objNameRightArm = "Right arm";
    public string objNameRightElbow = "Right elbow";
    public string objNameMiddleSpine = "Spine";
    public string objNameHead = "Head";

    Transform trnRagdollPelvis;
    Transform trnRagdollLeftHips;
    Transform trnRagdollLeftKnee;
    Transform trnRagdollLeftFoot;
    Transform trnRagdollRightHips;
    Transform trnRagdollRightKnee;
    Transform trnRagdollRightFoot;
    Transform trnRagdollLeftArm;
    Transform trnRagdollLeftElbow;
    Transform trnRagdollRightArm;
    Transform trnRagdollRightElbow;
    Transform trnRagdollMiddleSpine;
    Transform trnRagdollHead;

    Transform trnAnimatorPelvis;
    Transform trnAnimatorLeftHips;
    Transform trnAnimatorLeftKnee;
    Transform trnAnimatorLeftFoot;
    Transform trnAnimatorRightHips;
    Transform trnAnimatorRightKnee;
    Transform trnAnimatorRightFoot;
    Transform trnAnimatorLeftArm;
    Transform trnAnimatorLeftElbow;
    Transform trnAnimatorRightArm;
    Transform trnAnimatorRightElbow;
    Transform trnAnimatorMiddleSpine;
    Transform trnAnimatorHead;

    // Start is called before the first frame update
    void Start()
    {
        
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            
            rb.isKinematic = true;
        }


        trnRagdollPelvis = objRagdoll.transform.Find("Armature").Find(objNamePelvis);

        trnRagdollLeftHips = trnRagdollPelvis.Find(objNameLeftHips);
        trnRagdollLeftKnee = trnRagdollLeftHips.Find(objNameLeftKnee);
        trnRagdollLeftFoot = trnRagdollLeftKnee.Find(objNameLeftFoot);

        trnRagdollRightHips = trnRagdollPelvis.Find(objNameRightHips);
        trnRagdollRightKnee = trnRagdollRightHips.Find(objNameRightKnee);
        trnRagdollRightFoot = trnRagdollRightKnee.Find(objNameRightFoot);

        trnRagdollMiddleSpine = trnRagdollPelvis.Find(objNameMiddleSpine);

        trnRagdollLeftArm = trnRagdollMiddleSpine.Find("Chest").Find("Left shoulder").Find(objNameLeftArm);
        trnRagdollLeftElbow = trnRagdollLeftArm.Find(objNameLeftElbow);

        trnRagdollRightArm = trnRagdollMiddleSpine.Find("Chest").Find("Right shoulder").Find(objNameRightArm);
        trnRagdollRightElbow = trnRagdollRightArm.Find(objNameRightElbow);

        trnRagdollHead = trnRagdollMiddleSpine.Find("Chest").Find("Neck").Find(objNameHead);



        trnAnimatorPelvis = objAnimator.transform.Find("Armature").Find(objNamePelvis);

        trnAnimatorLeftHips = trnAnimatorPelvis.Find(objNameLeftHips);
        trnAnimatorLeftKnee = trnAnimatorLeftHips.Find(objNameLeftKnee);
        trnAnimatorLeftFoot = trnAnimatorLeftKnee.Find(objNameLeftFoot);

        trnAnimatorRightHips = trnAnimatorPelvis.Find(objNameRightHips);
        trnAnimatorRightKnee = trnAnimatorRightHips.Find(objNameRightKnee);
        trnAnimatorRightFoot = trnAnimatorRightKnee.Find(objNameRightFoot);

        trnAnimatorMiddleSpine = trnAnimatorPelvis.Find(objNameMiddleSpine);

        trnAnimatorLeftArm = trnAnimatorMiddleSpine.Find("Chest").Find("Left shoulder").Find(objNameLeftArm);
        trnAnimatorLeftElbow = trnAnimatorLeftArm.Find(objNameLeftElbow);

        trnAnimatorRightArm = trnAnimatorMiddleSpine.Find("Chest").Find("Right shoulder").Find(objNameRightArm);
        trnAnimatorRightElbow = trnAnimatorRightArm.Find(objNameRightElbow);

        trnAnimatorHead = trnAnimatorMiddleSpine.Find("Chest").Find("Neck").Find(objNameHead);
    }

    // Update is called once per frame
    void Update()
    {


        trnRagdollPelvis.localRotation = Quaternion.Lerp(trnRagdollPelvis.localRotation, trnAnimatorPelvis.localRotation, blend);
        trnRagdollLeftHips.localRotation = Quaternion.Lerp(trnRagdollLeftHips.localRotation, trnAnimatorLeftHips.localRotation, blend);
        trnRagdollLeftKnee.localRotation = Quaternion.Lerp(trnRagdollLeftKnee.localRotation, trnAnimatorLeftKnee.localRotation, blend);
        trnRagdollLeftFoot.localRotation = Quaternion.Lerp(trnRagdollLeftFoot.localRotation, trnAnimatorLeftFoot.localRotation, blend);
        trnRagdollRightHips.localRotation = Quaternion.Lerp(trnRagdollRightHips.localRotation, trnAnimatorRightHips.localRotation, blend);
        trnRagdollRightKnee.localRotation = Quaternion.Lerp(trnRagdollRightKnee.localRotation, trnAnimatorRightKnee.localRotation, blend);
        trnRagdollRightFoot.localRotation = Quaternion.Lerp(trnRagdollRightFoot.localRotation, trnAnimatorRightFoot.localRotation, blend);
        trnRagdollLeftArm.localRotation = Quaternion.Lerp(trnRagdollLeftArm.localRotation, trnAnimatorLeftArm.localRotation, blend);
        trnRagdollLeftElbow.localRotation = Quaternion.Lerp(trnRagdollLeftElbow.localRotation, trnAnimatorLeftElbow.localRotation, blend);
        trnRagdollRightArm.localRotation = Quaternion.Lerp(trnRagdollRightArm.localRotation, trnAnimatorRightArm.localRotation, blend);
        trnRagdollRightElbow.localRotation = Quaternion.Lerp(trnRagdollRightElbow.localRotation, trnAnimatorRightElbow.localRotation, blend);
        trnRagdollMiddleSpine.localRotation = Quaternion.Lerp(trnRagdollMiddleSpine.localRotation, trnAnimatorMiddleSpine.localRotation, blend);
        trnRagdollHead.localRotation = Quaternion.Lerp(trnRagdollHead.localRotation, trnAnimatorHead.localRotation, blend);

    }
}
