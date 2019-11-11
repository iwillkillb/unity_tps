using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCustom : MonoBehaviour
{
    Animator _Animator;

    [Range (0f, 1f)]
    public float IKWeight = 1f;

    // Foots goal position
    public Transform leftIKTarget;
    public Transform rightIKTarget;

    // Knees goal position
    public Transform leftHint;
    public Transform rightHint;

    Vector3 leftFootPos;
    Quaternion leftFootRot;
    [Range(0f, 1f)] float leftFootWeight = 1f;
    Transform leftFoot;

    Vector3 rightFootPos;
    Quaternion rightFootRot;
    [Range(0f, 1f)] float rightFootWeight = 1f;
    Transform rightFoot;

    public float offsetY;

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();

        leftFoot = _Animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = _Animator.GetBoneTransform(HumanBodyBones.RightFoot);

        leftFootRot = leftFoot.rotation;
        rightFootRot = rightFoot.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit leftHit;
        RaycastHit rightHit;

        Vector3 leftPos = leftFoot.TransformPoint(Vector3.zero);
        Vector3 rightPos = rightFoot.TransformPoint(Vector3.zero);

        if (Physics.Raycast(leftPos, Vector3.down, out leftHit, 1))
        {
            leftFootPos = leftHit.point;
            leftFootRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
        }

        if (Physics.Raycast(rightPos, Vector3.down, out rightHit, 1))
        {
            rightFootPos = rightHit.point;
            rightFootRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //IKByTransform();

        _Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
        _Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
        _Animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos + Vector3.up * offsetY);
        _Animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
        
        _Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
        _Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);
        _Animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos + Vector3.up * offsetY);
        _Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
        
    }

    void IKByTransform()
    {
        // Foots
        _Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, IKWeight);
        _Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, IKWeight);
        _Animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftIKTarget.position);
        _Animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftIKTarget.rotation);

        _Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, IKWeight);
        _Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, IKWeight);
        _Animator.SetIKPosition(AvatarIKGoal.RightFoot, rightIKTarget.position);
        _Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightIKTarget.rotation);

        // Knees
        _Animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, IKWeight);
        _Animator.SetIKHintPosition(AvatarIKHint.LeftKnee, leftHint.position);

        _Animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, IKWeight);
        _Animator.SetIKHintPosition(AvatarIKHint.RightKnee, rightHint.position);
    }
}
