using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RobotAgent : Agent {

    public override void InitializeAgent()
    {

    }

    public override void CollectObservations()
    {
        AddVisualObservation();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

    }

    public override void AgentReset()
    {

    }
}
