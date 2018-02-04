using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : RobotController {

    public float maxTorque;
    public PID left;
    public PID right; // Same parameters so we initialize it later
    public float targetSpeed;
    // Run every fixedupdate

    public override void SetMotors(float[] currentSpeeds, out float[] torques, out float[] steers)
    {
        // Run PID: Setpoint is based on controller input
        // Actual is current speed
        // Everything is normalized to 1, meaning when input = 1, speed is trying to equal targetSpeed
        Debug.Log("Actual speed: " + currentSpeeds[0] / targetSpeed);
        Debug.Log("Target speed: " + Input.GetAxis("L_YAxis_1"));
        float leftPID =  left.Update( Input.GetAxis("L_YAxis_1"), currentSpeeds[0] / targetSpeed, Time.fixedDeltaTime) * maxTorque;
        float rightPID = right.Update(Input.GetAxis("R_YAxis_1"), currentSpeeds[1] / targetSpeed, Time.fixedDeltaTime) * maxTorque;
        Debug.Log("PID: " + leftPID);

        // Set the motors
        torques = new float[base.wheels.Count];
        steers = new float[base.wheels.Count];
        for (int i = 0; i < torques.Length; i++)
        {
            if (i % 2 == 0) // Left
                torques[i] = leftPID;
            else
                torques[i] = rightPID;
            steers[i] = 0;
        }
    }
}
