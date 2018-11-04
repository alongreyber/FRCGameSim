using UnityEngine;
using System.Collections.Generic;

public abstract class RobotController : MonoBehaviour
{
    // By convention, the wheels alternate sides starting with left
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
        for (int i = 0; i < wheels.Count; i++)
        {
            float speed = wheels[i].rpm * wheels[i].radius;
            currentSpeed[i] = speed;
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