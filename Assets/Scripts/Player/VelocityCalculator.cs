using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCalculator
{
    public float TargetVelocity { get; set; }
    public float Acceleration { get; set; }

    public VelocityCalculator(float targetVelocity, float acceleration)
    {
        TargetVelocity = targetVelocity;
        Acceleration = acceleration;
    }

    public float CalculateNextVelocity(float currentVelocity, float deltaTime)
    {
        if(currentVelocity == TargetVelocity || Acceleration == -1)
        {
            return TargetVelocity;
        }
        
        float positiveAcceleration = Acceleration * Mathf.Sign(Acceleration);

        float direction = Mathf.Sign(TargetVelocity - currentVelocity);
        float tentative = currentVelocity + direction * positiveAcceleration * deltaTime;

        if((direction > 0 && tentative > TargetVelocity) || (direction < 0 && tentative < TargetVelocity))
        {
            tentative = TargetVelocity;
        }

        return tentative;
    }
}
