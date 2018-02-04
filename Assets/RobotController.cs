using UnityEngine;
using System.Collections.Generic;

public abstract class RobotController : MonoBehaviour
{
    // By convention, the wheels alernate sides starting with left
    public List<WheelCollider> wheels;
    public abstract void SetMotors(float[] currentSpeed, out float[] torques, out float[] steers);

    public void Start()
    {
        foreach (WheelCollider w in wheels)
        {
            w.ConfigureVehicleSubsteps(5, 12, 14);
        }
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation * Quaternion.Euler(0, 0, 90);
    }

    private Vector3[] lastFrameWheelPositions;
    public void FixedUpdate()
    {
        float[] steering, torque;
        float[] currentSpeed = new float[wheels.Count];
        // Keep track of last frame's wheel positions so we can calculate the velocity
        lastFrameWheelPositions = new Vector3[wheels.Count];
        for (int i = 0; i < wheels.Count; i++)
        {
            // Vector magics
            Vector3 vel = wheels[i].transform.TransformPoint(wheels[i].transform.position) - lastFrameWheelPositions[i];
            Vector3 localVel = wheels[i].transform.InverseTransformDirection(vel);
            float speed = Vector3.Project(localVel, wheels[i].transform.forward).magnitude;
            currentSpeed[i] = speed;
            lastFrameWheelPositions[i] = wheels[i].transform.TransformPoint(wheels[i].transform.position);
        }
        SetMotors(currentSpeed, out torque, out steering);
        for (int i = 0; i < wheels.Count; i++)
        {
            WheelCollider w = wheels[i];
            w.steerAngle = steering[i];
            w.motorTorque = torque[i];
            ApplyLocalPositionToVisuals(w);
        }
    }
}