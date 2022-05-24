using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMovementKeysP2 : BaseInput
{
    public string TurnInputName = "HorizontalP2";
    public string AccelerateButtonName = "AccelerateP2";
    public string BrakeButtonName = "BrakeP2";
    public string BoostButtonName = "BoostP2";
    public string DriftButtonName = "DriftP2";
    public string RespawnButtonName = "RespawnP2";

    public override InputData GenerateInput() {
        return new InputData
        {
            Accelerate = Input.GetButton(AccelerateButtonName),
            Brake = Input.GetButton(BrakeButtonName),
            Boost = Input.GetButton(BoostButtonName),
            TurnInput = Input.GetAxis(TurnInputName),
            Drift = Input.GetButton(DriftButtonName),
            Respawn = Input.GetButton(RespawnButtonName)
        };
    }
}