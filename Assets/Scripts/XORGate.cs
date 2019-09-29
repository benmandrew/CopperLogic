using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class XORGate : Gate {
    private void Awake() {
        base.Awake();
        gate_type = GateType.XOR;
    }

    public override bool get_value() {
        if (!changed) {
            return value;
        }
        value = false;
        int count = 0;
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            count += incoming_neighbours[i].get_value() ? 1 : 0;
        }
        if (count == 1) {
            value = true;
        }
        changed = false;
        return value;
    }
}
