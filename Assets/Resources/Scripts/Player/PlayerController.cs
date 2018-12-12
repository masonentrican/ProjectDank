using Bolt;
using com.dankstudios;
using UnityEngine;

public class PlayerController : Bolt.EntityBehaviour<IPlayerState>
{
    const float MOUSE_SENSITIVITY = 2f;

    bool _forward;
    bool _backward;
    bool _left;
    bool _right;
    bool _jump;
    bool _primaryFire;
    bool _secondaryFire;
    float _yaw;
    float _pitch;

    PlayerMotor _motor;

    [SerializeField]
    ItemBase[] items;

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

        state.OnPrimaryFire = () =>
        {
            items[0].PrimaryFire(entity);
        };
    }

    void PollKeys(bool mouse)
    {
        _forward = Input.GetKey(KeyCode.W);
        _backward = Input.GetKey(KeyCode.S);
        _left = Input.GetKey(KeyCode.A);
        _right = Input.GetKey(KeyCode.D);
        _jump = Input.GetKeyDown(KeyCode.Space);

        _primaryFire = Input.GetMouseButton(0);
        _secondaryFire = Input.GetMouseButton(1);

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
        input.secondaryFire = _secondaryFire;
        input.primaryFire = _primaryFire;
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

                
                state.pitch = cmd.Input.Pitch;

                if (cmd.Input.primaryFire)
                {
                    UseItem(cmd);
                }
            }
        }
    }

    void UseItem(Command cmd)
    {
        if (items[0].FireFrame + items[0].FireInterval <= BoltNetwork.serverFrame)
        {
            items[0].FireFrame = BoltNetwork.serverFrame;
            state.PrimaryFire();
        }
    }


    //animate player broken -fixed
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