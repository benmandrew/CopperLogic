using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ANDGate : Gate {
    private void Awake() {
        base.Awake();
        gate_type = GateType.AND;
    }

    public override bool get_value() {
        if (!changed) {
            return value;
        }
        if (incoming_neighbours.Count == 0) {
            return false;
        }
        value = true;
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            value = value && incoming_neighbours[i].get_value();
        }
        changed = false;
        return value;
    }
}
