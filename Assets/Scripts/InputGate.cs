using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class InputGate : Gate {
    public override bool get_value() {
        return value;
    }

    public override string serialise() {
        return internal_serialise("Input");
    }
}
