using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reference Credit: ArcadeKart.cs from Karting Microgame @ learn.unity.com/project/karting-template
// Adapted by: Donny Ebel (mechanics manager), Arden Butterfield, Oliver Hostick

/*  To Do:
 *  Implement Boost
 *  Implement Drifting
 *  Consider turning suspension wheels into a list and adding a helper function to update suspension
 * 
 */

public class KartController : MonoBehaviour
{
    // Kart base stat struct
    public struct Stats
    {
        // Stat struct declaration
        public float TopSpeed;
        public float Acceleration;
        public float ReverseSpeed;
        public float ReverseAcceleration;
        public float AccelerationCurve;     // How quickly kart accelerates from 0
        public float Braking;               // How quickly kart slows when brakes are applied
        public float CoastingDrag;          // How quickly kart will reach full stop with no inputs
        public float Grip;                  // Amount of side-to-side friction
        public float Steer;                 // The kart's handling
        public float AddedGravity;          // Extra gravity for airbourne state.

        // Overloads the + operator to easily add Stat structs.
        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats
            {
                Acceleration = a.Acceleration + b.Acceleration,
                AccelerationCurve = a.AccelerationCurve + b.AccelerationCurve,
                Braking = a.Braking + b.Braking,
                CoastingDrag = a.CoastingDrag + b.CoastingDrag,
                AddedGravity = a.AddedGravity + b.AddedGravity,
                Grip = a.Grip + b.Grip,
                ReverseAcceleration = a.ReverseAcceleration + b.ReverseAcceleration,
                ReverseSpeed = a.ReverseSpeed + b.ReverseSpeed,
                TopSpeed = a.TopSpeed + b.TopSpeed,
                Steer = a.Steer + b.Steer,
            };
        }
    }

    // Instantiate a Stats struct; 
    private Stats BaseStats = new Stats
    {
        TopSpeed = topSpeed,
        Acceleration = acceleration,
        ReverseSpeed = reverseSpeed,
        ReverseAcceleration = reverseAcceleration,
        AccelerationCurve = accelerationCurve,
        Braking = braking,
        CoastingDrag = coastingDrag,
        Grip = grip,
        Steer = steer,
        AddedGravity = addedGravity
    };

    // FinalStats is initially a copyp of BaseStats, but boosting changes this.
    private Stats FinalStats = new Stats
    {
        TopSpeed = topSpeed,
        Acceleration = acceleration,
        ReverseSpeed = reverseSpeed,
        ReverseAcceleration = reverseAcceleration,
        AccelerationCurve = accelerationCurve,
        Braking = braking,
        CoastingDrag = coastingDrag,
        Grip = grip,
        Steer = steer,
        AddedGravity = addedGravity
    };

    // This struct is for applying a boost. This is added directly to BaseStats
    private Stats BoostedStats = new Stats
    {
        TopSpeed = 30f,
        Acceleration = 10f,
        ReverseSpeed = 0,
        ReverseAcceleration = 0,
        AccelerationCurve = 0,
        Braking = 0,
        CoastingDrag = 0,
        Grip = 0,
        Steer = 0,
        AddedGravity = 0
    };

    // Necessary game objects
    public Rigidbody Rigidbody { get; private set; }        // references the physical kart
    public InputData Input { get; private set; }            // initializes the Input struct; KartInput.cs
    IInput[] Inputs;                                        // list of IInputs where generated inputs are stored
    public float AirPercent { get; private set; }           // AirPercent/GroundPercent track how many wheels are grounded
    public float GroundPercent { get; private set; }
    public bool WantsToDrift { get; private set; } = false; // Indicate drifting status
    public bool IsDrifting { get; private set; } = false;
    bool HasCollision = false;
    bool InAir = false;

    // Stat parameters
    static private float topSpeed = 50f;
    static private float acceleration = 1f;
    static private float reverseSpeed = 15f;
    static private float reverseAcceleration = 0.7f;
    static private float accelerationCurve = 4f;
    static private float braking = 4f;
    static private float coastingDrag = 10f;
    static private float grip = 0.9f;
    static private float steer = 5f;
    static private float addedGravity = 1f;

    // Boost
    public NitroManager nitroManager;
    bool isBoosting = false;
    float boostTimer = 0.0f;

    // Drift Params
    float DriftGrip = 0.4f;
    float DriftAdditionalSteer = 5.0f;
    float MinAngleToFinishDrift = 10.0f;
    float MinSpeedPercentToFinishDrift = 10.0f;
    float DriftControl = 10.0f;
    float DriftDampening = 10.0f;
    float CurrentGrip = 1.0f;
    float DriftTurningPower = 0.0f;
    float PreviousGroundPercent = 1.0f;

    // Suspension Params and Wheel Objects
    public Transform CenterOfMass;                      // attatch kart collider parent obj here in Inspector
    float AirborneReorientationCoeff = 3.0f;     // how quickly the kart rights itself while airborne
    float SuspensionHeight = 0.2f;
    float SuspensionSpring = 20000.0f;
    float SuspensionDamp = 500.0f;
    float WheelsPositionVerticalOffset = 0.0f;
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public WheelCollider RearLeftWheel;
    public WheelCollider RearRightWheel;
    public GameObject FrontLeftMesh;
    public GameObject FrontRightMesh;
    public GameObject RearLeftMesh;
    public GameObject RearRightMesh;

    // Sets which layers the physics engine cares about
    public LayerMask GroundLayers = Physics.DefaultRaycastLayers;

    // Used to handle rotations and collisions
    Quaternion LastValidRotation;
    Vector3 LastValidPosition;
    Vector3 LastCollisionNormal;

    // Constants; useful for some move calculations
    const float NullInput = 0.01f;
    const float NullSpeed = 0.01f;
    Vector3 VerticalReference = Vector3.up;

    // ====================== Primary Funcions ==========================
    // Get necessary components and update state
    void Awake()
    {
        // Hook up rigidbody, raw inputs, and assigns current grip (C# complained when attempted during init)
        Rigidbody = GetComponent<Rigidbody>();
        Inputs = GetComponents<IInput>();
        CurrentGrip = FinalStats.Grip;

        // Update suspension
        UpdateSuspensionParams(FrontLeftWheel);
        UpdateSuspensionParams(FrontRightWheel);
        UpdateSuspensionParams(RearLeftWheel);
        UpdateSuspensionParams(RearRightWheel);
    }

    void FixedUpdate()
    {
        // Update suspension params and get new centerOfMass
        Rigidbody.centerOfMass = transform.InverseTransformPoint(CenterOfMass.position);
        UpdateSuspensionParams(FrontLeftWheel);
        UpdateSuspensionParams(FrontRightWheel);
        UpdateSuspensionParams(RearLeftWheel);
        UpdateSuspensionParams(RearRightWheel);

        // Generate inputs and check if we're fully airborne for this frame!
        GetInputs();
        GetGroundedPercent();       // Populates GroundPercent/AirPercent vars
        AddAirborneGravity();       // If airborne, add a little extra gravity

        // Move!
        CheckBoost();
        MoveVehicle(Input.Accelerate, Input.Brake, Input.TurnInput);

        // Insert animation stuff here

    }

    // ====================== Helper Functions ==========================
    // Collision Handlers; these are lambda functions. If either is invoked, it sets HasCollision flag appropriately.
    void OnCollisionEnter(Collision collision) => HasCollision = true;
    void OnCollisionExit(Collision collision) => HasCollision = false;

    // This is called every frame there is a collision
    void OnCollisionStay(Collision collision)
    {
        HasCollision = true;
        LastCollisionNormal = Vector3.zero;
        float dot = -1.0f;

        // Looks at each point of contact in the collision and sets the normalized collision point.
        foreach (var contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > dot)
                LastCollisionNormal = contact.normal;
        }
    }

    void GetInputs()
    {
        Input = new InputData();
        WantsToDrift = false;
        for (int i = 0; i < Inputs.Length; i++)
        {
            Input = Inputs[i].GenerateInput();
            WantsToDrift = Input.Brake && Vector3.Dot(Rigidbody.velocity, transform.forward) > 0.0f;
        }
    }

    void GetGroundedPercent()
    {
        // Use suspension to determine how many wheels are grounded
        int numGrounded = 0;
        WheelHit hit;
        if (FrontLeftWheel.isGrounded && FrontLeftWheel.GetGroundHit(out hit))
            numGrounded++;
        if (FrontRightWheel.isGrounded && FrontRightWheel.GetGroundHit(out hit))
            numGrounded++;
        if (RearLeftWheel.isGrounded && RearLeftWheel.GetGroundHit(out hit))
            numGrounded++;
        if (RearRightWheel.isGrounded && RearRightWheel.GetGroundHit(out hit))
            numGrounded++;

        // Apply findings to necessary variables
        GroundPercent = (float)numGrounded / 4.0f;
        AirPercent = 1 - GroundPercent;
        if (AirPercent >= 1)
            InAir = true;
        else
            InAir = false;
    }

    void AddAirborneGravity()
    {
        if (InAir)
            Rigidbody.velocity += Physics.gravity * Time.fixedDeltaTime * FinalStats.AddedGravity;
    }

    void UpdateSuspensionParams(WheelCollider wheel)
    {
        wheel.suspensionDistance = SuspensionHeight;
        wheel.center = new Vector3(0.0f, WheelsPositionVerticalOffset, 0.0f);
        JointSpring spring = wheel.suspensionSpring;
        spring.spring = SuspensionSpring;
        spring.damper = SuspensionDamp;
        wheel.suspensionSpring = spring;
    }

    void CheckBoost()
    {
        // If user inputs boost AND there's enough nitro AND the timer was reset...
        if (Input.Boost && nitroManager.EnoughNitro(isBoosting))
        {
            // Set boosting to true, update stats
            isBoosting = true;
            FinalStats = BaseStats + BoostedStats;
        }
        // Otherwise, if boosting, update the timer.
        else if (isBoosting)
            boostTimer += Time.fixedDeltaTime;
        
        if (boostTimer > 1.5f)
        {
            isBoosting = false;
            boostTimer = 0.0f;
            FinalStats = BaseStats;
        }
    }

    void MoveVehicle(bool accelerate, bool brake, float turnInput)
    {
        // DETERMINE ACCELERATION
        // Get current speed and velocity; set acceleration curve coefficient
        float currentSpeed = Rigidbody.velocity.magnitude;
        Vector3 localVel = transform.InverseTransformVector(Rigidbody.velocity);

        // Determine direction based on input: 1.0 means we are accelerating; 0.0 means no input or both accel/brake; -1 means braking
        float accelInput = (accelerate ? 1.0f : 0.0f) - (brake ? 1.0f : 0.0f);

        // Get current local velocity and speed, then determine current direction and where we want to go
        bool accelDirectionIsFwd = accelInput >= 0;
        bool localVelDirectionIsFwd = localVel.z >= 0;

        // Grab the correct speed/acceleration numbers based on direction... but if we're braking, use braking stats instead of accelPower.
        float maxSpeed = localVelDirectionIsFwd ? FinalStats.TopSpeed : FinalStats.ReverseSpeed;
        float accelPower = accelDirectionIsFwd ? FinalStats.Acceleration : FinalStats.ReverseAcceleration;
        bool isBraking = (localVelDirectionIsFwd && brake) || (!localVelDirectionIsFwd && accelerate);
        float finalAccelPower = isBraking ? FinalStats.Braking : accelPower;

        // This clever bit uses currentSpeed/maxSpeed (value between 0 and 1) to accelerate more slowly the closer you are to your max speed.
        float accelerationCurveCoeff = 5;
        float accelRampT = currentSpeed / maxSpeed;
        float multipliedAccelerationCurve = FinalStats.AccelerationCurve * accelerationCurveCoeff;
        float accelRamp = Mathf.Lerp(multipliedAccelerationCurve, 1, accelRampT * accelRampT);

        // Yield finalAcceleration.
        float finalAcceleration = finalAccelPower * accelRamp;


        // DETERMINE VELOCITY
        // If we're drifting use drift turning; otherwise steer based on input
        float turningPower = IsDrifting ? DriftTurningPower : turnInput * FinalStats.Steer;

        // Multiply quaternion and the foward vector to yield a "toward" vector.
        Quaternion turnAngle = Quaternion.AngleAxis(turningPower, transform.up);
        Vector3 fwd = turnAngle * transform.forward;

        // Create movement vector by multiplying "toward" vector with accelInput and finalAcceleration.
        // accelInput is -1 <= 0 <= 1, so either our angle is updated, reversed, or canceled.
        // ableToTurn is similar; if we've crashed or we're in the air, we don't turn!
        float ableToTurn = (HasCollision || GroundPercent > 0.0f) ? 1.0f : 0.0f;
        Vector3 movement = fwd * finalAcceleration * accelInput * ableToTurn;

        // If we're over max speed, don't go any faster!
        bool wasOverMaxSpeed = currentSpeed >= maxSpeed;
        if (wasOverMaxSpeed && !isBraking && !isBoosting)
        {
            movement *= 0.0f;
        }

        // Create temp vector to store updated calculations at this point.
        Vector3 newVelocity = Rigidbody.velocity + movement * Time.fixedDeltaTime;
        newVelocity.y = Rigidbody.velocity.y;

        //  Limit new velocity based on max speed.
        if (GroundPercent > 0.0f && !wasOverMaxSpeed)
            newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);

        // Apply drag if we're coasting OR we just stopped boosting and we're over max speed
        // MoveTowards essentially shortens the y component by Stats.CoastingDrag
        if ((Mathf.Abs(accelInput) < NullInput && GroundPercent > 0.0f) || (wasOverMaxSpeed && !isBoosting))
            newVelocity = Vector3.MoveTowards(newVelocity, new Vector3(0, Rigidbody.velocity.y, 0), Time.fixedDeltaTime * FinalStats.CoastingDrag);

        // Assign the newly calculated velocity.
        Rigidbody.velocity = newVelocity;


        // DRIFTING
        if (GroundPercent > 0.0f)
        {
            if (InAir)
            {
                InAir = false;
            }

            // manual angular velocity coefficient
            float angularVelocitySteering = 0.4f;
            float angularVelocitySmoothSpeed = 20f;

            // turning is reversed if we're going in reverse and pressing reverse
            if (!localVelDirectionIsFwd && !accelDirectionIsFwd)
                angularVelocitySteering *= -1.0f;

            var angularVel = Rigidbody.angularVelocity;

            // move the Y angular velocity towards our target
            angularVel.y = Mathf.MoveTowards(angularVel.y, turningPower * angularVelocitySteering, Time.fixedDeltaTime * angularVelocitySmoothSpeed);

            // apply the angular velocity
            Rigidbody.angularVelocity = angularVel;

            // rotate rigidbody's velocity as well to generate immediate velocity redirection
            // manual velocity steering coefficient
            float velocitySteering = 25f;

            // If the karts lands with a forward not in the velocity direction, we start the drift
            if (GroundPercent >= 0.0f && PreviousGroundPercent < 0.1f)
            {
                Vector3 flattenVelocity = Vector3.ProjectOnPlane(Rigidbody.velocity, VerticalReference).normalized;
                if (Vector3.Dot(flattenVelocity, transform.forward * Mathf.Sign(accelInput)) < Mathf.Cos(MinAngleToFinishDrift * Mathf.Deg2Rad))
                {
                    IsDrifting = true;
                    CurrentGrip = DriftGrip;
                    DriftTurningPower = 0.0f;
                }
            }

            // Drift Management
            if (!IsDrifting)
            {
                if ((WantsToDrift || isBraking) && currentSpeed > maxSpeed * MinSpeedPercentToFinishDrift)
                {
                    IsDrifting = true;
                    DriftTurningPower = turningPower + (Mathf.Sign(turningPower) * DriftAdditionalSteer);
                    CurrentGrip = DriftGrip;
                }
            }

            if (IsDrifting)
            {
                float turnInputAbs = Mathf.Abs(turnInput);
                if (turnInputAbs < NullInput)
                    DriftTurningPower = Mathf.MoveTowards(DriftTurningPower, 0.0f, Mathf.Clamp01(DriftDampening * Time.fixedDeltaTime));

                // Update the turning power based on input
                float driftMaxSteerValue = FinalStats.Steer + DriftAdditionalSteer;
                DriftTurningPower = Mathf.Clamp(DriftTurningPower + (turnInput * Mathf.Clamp01(DriftControl * Time.fixedDeltaTime)), -driftMaxSteerValue, driftMaxSteerValue);

                bool facingVelocity = Vector3.Dot(Rigidbody.velocity.normalized, transform.forward * Mathf.Sign(accelInput)) > Mathf.Cos(MinAngleToFinishDrift * Mathf.Deg2Rad);

                bool canEndDrift = true;
                if (isBraking)
                    canEndDrift = false;
                else if (!facingVelocity)
                    canEndDrift = false;
                else if (turnInputAbs >= NullInput && currentSpeed > maxSpeed * MinSpeedPercentToFinishDrift)
                    canEndDrift = false;

                if (canEndDrift || currentSpeed < NullSpeed)
                {
                    // No Input, and car aligned with speed direction => Stop the drift
                    IsDrifting = false;
                    CurrentGrip = FinalStats.Grip;
                }

            }

            // rotate our velocity based on current steer value
            Rigidbody.velocity = Quaternion.AngleAxis(turningPower * Mathf.Sign(localVel.z) * velocitySteering * CurrentGrip * Time.fixedDeltaTime, transform.up) * Rigidbody.velocity;
        }
        else
        {
            InAir = true;
        }

        bool validPosition = false;
        if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out RaycastHit hit, 3.0f, 1 << 9 | 1 << 10 | 1 << 11)) // Layer: ground (9) / Environment(10) / Track (11)
        {
            Vector3 lerpVector = (HasCollision && LastCollisionNormal.y > hit.normal.y) ? LastCollisionNormal : hit.normal;
            VerticalReference = Vector3.Slerp(VerticalReference, lerpVector, Mathf.Clamp01(AirborneReorientationCoeff * Time.fixedDeltaTime * (GroundPercent > 0.0f ? 10.0f : 1.0f)));    // Blend faster if on ground
        }
        else
        {
            Vector3 lerpVector = (HasCollision && LastCollisionNormal.y > 0.0f) ? LastCollisionNormal : Vector3.up;
            VerticalReference = Vector3.Slerp(VerticalReference, lerpVector, Mathf.Clamp01(AirborneReorientationCoeff * Time.fixedDeltaTime));
        }

        validPosition = GroundPercent > 0.7f && !HasCollision && Vector3.Dot(VerticalReference, Vector3.up) > 0.9f;

        // Airborne / Half on ground management
        if (GroundPercent < 0.7f)
        {
            Rigidbody.angularVelocity = new Vector3(0.0f, Rigidbody.angularVelocity.y * 0.98f, 0.0f);
            Vector3 finalOrientationDirection = Vector3.ProjectOnPlane(transform.forward, VerticalReference);
            finalOrientationDirection.Normalize();
            if (finalOrientationDirection.sqrMagnitude > 0.0f)
            {
                Rigidbody.MoveRotation(Quaternion.Lerp(Rigidbody.rotation, Quaternion.LookRotation(finalOrientationDirection, VerticalReference), Mathf.Clamp01(AirborneReorientationCoeff * Time.fixedDeltaTime)));
            }
        }
        else if (validPosition)
        {
            LastValidPosition = transform.position;
            LastValidRotation.eulerAngles = new Vector3(0.0f, transform.rotation.y, 0.0f);
        }

    }
}

