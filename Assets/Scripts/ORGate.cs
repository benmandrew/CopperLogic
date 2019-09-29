using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ORGate : Gate {
    private void Awake() {
        base.Awake();
        gate_type = GateType.OR;
    }

    public override bool get_value() {
        if (!changed) {
            return value;
        }
        value = false;
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            value = value || incoming_neighbours[i].get_value();
        }
        changed = false;
        return value;
    }
}
