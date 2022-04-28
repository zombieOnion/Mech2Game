﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGuidedRocket : WeaponBase, ILockTarget {
    private Transform Target;
    public float force = 0.1f;
    private bool hasRecievedTarget = false;
    private bool stopTurning = false;
    private Vector3 EndPos = Vector3.zero;
    
    // Update is called once per frame
    void FixedUpdate () {
        if(Target != null) {
            HomeInOnTarget();
        }
        else if(Target == null && !hasRecievedTarget)
        {
            MoveForward();
        }
        else if(Target == null && hasRecievedTarget)
        {
            Destroy(gameObject, 0.001f);
        }
        
    }

    public void SetTarget(Transform target) {
        Target = target;
        hasRecievedTarget = true;
    }

    private void HomeInOnTarget() {
        var target = Target.position;
        Vector3 targetDelta = new Vector3(target.x, target.y+1f, target.z) - transform.position;

        //get the angle between transform.forward and target delta
        float angleDiff = Vector3.Angle(transform.forward, targetDelta);
        Debug.Log(angleDiff);
        if(angleDiff > 20)
        {
            Destroy(gameObject, 5);
            stopTurning = true;
            EndPos = transform.position + Vector3.forward * 100;
        }
        if (stopTurning)
        {
            targetDelta = new Vector3(EndPos.x, EndPos.y + 1f, EndPos.z) - transform.position;
            Vector3 cross = Vector3.Cross(transform.forward, targetDelta);
            rb.AddTorque(cross * angleDiff * force);
            MoveForward();
        }
        else
        {
            // get its cross product, which is the axis of rotation to
            // get from one vector to the other
            Vector3 cross = Vector3.Cross(transform.forward, targetDelta);

            // apply torque along that axis according to the magnitude of the angle.
            rb.AddTorque(cross * angleDiff * force);
            MoveForward();
        }
    }

}
