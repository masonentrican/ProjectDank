using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBehaviour : Bolt.EntityBehaviour<IRobotState>
{
    public float angularSpeedMag = 0.1f;
    public float speedMag = 0.1f;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        state.SetAnimator(GetComponent<Animator>());

        state.Animator.applyRootMotion = entity.isOwner;
    }

    public override void SimulateOwner()
    {
        var speed = state.Speed;
        var angularSpeed = state.AngularSpeed;

        if (Input.GetKey(KeyCode.W))
        {
            speed += speedMag;
        }
        else
        {
            speed -= speedMag;
        }

        if (Input.GetKey(KeyCode.A))
        {
            angularSpeed -= angularSpeedMag;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            angularSpeed += angularSpeedMag;
        }
        else
        {
            if (angularSpeed < 0)
            {
                angularSpeed += angularSpeedMag;
                angularSpeed = Mathf.Clamp(angularSpeed, -1f, 0);
            }
            else if (angularSpeed > 0)
            {
                angularSpeed -= angularSpeedMag;
                angularSpeed = Mathf.Clamp(angularSpeed, 0, +1f);
            }
        }

        state.Speed = Mathf.Clamp(speed, 0f, 1.5f);
        state.AngularSpeed = Mathf.Clamp(angularSpeed, -2f, +2f);
    }
}
