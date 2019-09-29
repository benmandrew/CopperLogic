using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class InputGate : Gate {
    private void Awake() {
        base.Awake();
        gate_type = GateType.Input;
    }

    public override bool get_value() {
        return value;
    }
}
