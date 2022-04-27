using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMovementKeysP2 : BaseInput
{
    public string TurnInputName = "HorizontalP2";
    public string AccelerateButtonName = "AccelerateP2";
    public string BrakeButtonName = "BrakeP2";
    public string BoostButtonName = "BoostP2";

    public override InputData GenerateInput() {
        return new InputData
        {
            Accelerate = Input.GetButton(AccelerateButtonName),
            Brake = Input.GetButton(BrakeButtonName),
            Boost = Input.GetButton(BoostButtonName),
            TurnInput = Input.GetAxis("HorizontalP2")
        };
    }
}