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
        float leftSetpoint = -1 * Input.GetAxis("L_YAxis_1");
        float rightSetpoint = -1 * Input.GetAxis("R_YAxis_1");
        float leftActual = currentSpeeds[2] / targetSpeed;
        float rightActual = currentSpeeds[3] / targetSpeed;
        float leftPID  = left.Update(leftSetpoint, leftActual, Time.fixedDeltaTime);
        float rightPID = right.Update(rightSetpoint, rightActual, Time.fixedDeltaTime);
        Debug.LogFormat("Left Setpoint + Actual + PID : {0,12:F2} {1,12:F2} {2,12:F2}", leftSetpoint, leftActual, leftPID);
        Debug.LogFormat("Right Setpoint + Actual + PID: {0,12:F2} {1,12:F2} {2,12:F2}", rightSetpoint, rightActual, rightPID);

        // Set the motors
        torques = new float[base.wheels.Count];
        steers = new float[base.wheels.Count];
        for (int i = 0; i < torques.Length; i++)
        {
            if (i % 2 == 0) // Left
                torques[i] = leftPID * maxTorque;
            else
                torques[i] = rightPID * maxTorque;
            steers[i] = 0;
        }
    }
}
