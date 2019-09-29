using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class NOTGate : Gate {
    private void Awake() {
        base.Awake();
        gate_type = GateType.NOT;
    }

    public override bool get_value() {
        if (incoming_neighbours.Count == 0) {
            return false;
        }
        if (!changed) {
            return value;
        }
        value = !incoming_neighbours[0].get_value();
        changed = false;
        return value;
    }
}
