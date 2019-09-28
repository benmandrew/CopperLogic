using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ANDGate : Gate {
    public override bool get_value() {
        if (value_calculated) {
            return value;
        }
        if (incoming_neighbours.Count == 0) {
            return false;
        }
        value = true;
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            value = value && incoming_neighbours[i].get_value();
        }
        value_calculated = true;
        return value;
    }

    public override string serialise() {
        return internal_serialise("AND");
    }
}
