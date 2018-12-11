using Bolt;
using com.dankstudios;
using UnityEngine;

public class PlayerController : Bolt.EntityBehaviour<IRobotState>
{
    const float MOUSE_SENSITIVITY = 2f;

    bool _forward;
    bool _backward;
    bool _left;
    bool _right;
    bool _jump;

    float _yaw;
    float _pitch;

    PlayerMotor _motor;

    void Awake()
    {
        _motor = GetComponent<PlayerMotor>();
    }

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        //Broken //Animations may be broken
        state.SetAnimator(GetComponentInChildren<Animator>());

        // Configure Animator
        state.Animator.SetLayerWeight(0, 1);
        state.Animator.SetLayerWeight(1, 1);
    }

    void PollKeys(bool mouse)
    {
        _forward = Input.GetKey(KeyCode.W);
        _backward = Input.GetKey(KeyCode.S);
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _jump = Input.GetKeyDown(KeyCode.Space);

        if (mouse)
        {
            _yaw += (Input.GetAxisRaw("Mouse X") * MOUSE_SENSITIVITY);
            _yaw %= 360f;

            _pitch += (-Input.GetAxisRaw("Mouse Y") * MOUSE_SENSITIVITY);
            _pitch = Mathf.Clamp(_pitch, -85f, +85f);
        }
    }

    void Update()
    {
        PollKeys(true);
    }

    public override void SimulateController()
    {
        PollKeys(false);

        IPlayerCommandInput input = PlayerCommand.Create();

        input.Forward = _forward;
        input.Backward = _backward;
        input.Left = _left;
        input.Right = _right;
        input.Jump = _jump;
        input.Yaw = _yaw;
        input.Pitch = _pitch;

        entity.QueueInput(input);
    }
    public override void ExecuteCommand(Command command, bool resetState)
    {
        PlayerCommand cmd = (PlayerCommand)command;

        if (resetState)
        {
            // we got a correction from the server, reset (this only runs on the client)
            _motor.SetState(cmd.Result.Position, cmd.Result.Velocity, cmd.Result.IsGrounded, cmd.Result.JumpFrames);
        }
        else
        {
            // apply movement (this runs on both server and client)
            PlayerMotor.State motorState = _motor.Move(cmd.Input.Forward, cmd.Input.Backward, cmd.Input.Left, cmd.Input.Right, cmd.Input.Jump, cmd.Input.Yaw);

            // copy the motor state to the commands result (this gets sent back to the client)
            cmd.Result.Position = motorState.position;
            cmd.Result.Velocity = motorState.velocity;
            cmd.Result.IsGrounded = motorState.isGrounded;
            cmd.Result.JumpFrames = motorState.jumpFrames;

            //Animations may be broken
            if (cmd.IsFirstExecution)
            {
                AnimatePlayer(cmd);
            }
        }
    }
    //animate player broken
    void AnimatePlayer(PlayerCommand cmd)
    {
        // FWD <> BWD movement
        if (cmd.Input.Forward ^ cmd.Input.Backward)
        {
            state.MoveZ = cmd.Input.Forward ? 1 : -1;
        }
        else
        {
            state.MoveZ = 0;
        }

        // LEFT <> RIGHT movement
        if (cmd.Input.Left ^ cmd.Input.Right)
        {
            state.MoveX = cmd.Input.Right ? 1 : -1;
        }
        else
        {
            state.MoveX = 0;
        }

        // JUMP
        if (_motor.jumpStartedThisFrame)
        {
            state.Jump();
        }
    }
}