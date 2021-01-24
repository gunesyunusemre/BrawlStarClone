using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    //[Tooltip("The distance in the local x-z plane to the target")]
    Transform target;

    public bool isFollowing;

    private Vector3 offset;
    public Vector3 offsetRed = new Vector3(0, 7, 5);
    public Vector3 offsetBlue = new Vector3(0, 7, -5);

    public void SetTarget(Transform _target)
    {
        target = _target;
        gameObject.transform.parent = target.parent;
        if (target.GetComponent<PlayerScript>().Team == "red")
        {
            offset = offsetRed;
        }
        else if (target.GetComponent<PlayerScript>().Team == "blue")
        {
            offset = offsetBlue;
        }

    }


    private void LateUpdate()
    {
        FollowObject();
    }

    private void FollowObject()
    {
        if (!isFollowing)
        {
            return;
        }
        gameObject.transform.position = target.position + offset;
    }


}
