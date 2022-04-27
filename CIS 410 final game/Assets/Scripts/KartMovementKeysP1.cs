using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMovementKeysP1 : BaseInput
{
    public string TurnInputName = "HorizontalP1";
    public string AccelerateButtonName = "AccelerateP1";
    public string BrakeButtonName = "BrakeP1";
    public string BoostButtonName = "BoostP1";

    public override InputData GenerateInput() {
        return new InputData
        {
            Accelerate = Input.GetButton(AccelerateButtonName),
            Brake = Input.GetButton(BrakeButtonName),
            Boost = Input.GetButton(BoostButtonName),
            TurnInput = Input.GetAxis("HorizontalP1")
        };
    }
}