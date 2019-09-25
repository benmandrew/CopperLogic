using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class NOTGate : Gate {
    public override bool get_value() {
        if (incoming_neighbours.Count == 0) {
            return false;
        }
        if (value_calculated) {
            return value;
        }
        value = !incoming_neighbours[0].get_value();
        value_calculated = true;
        return value;
    }

    public override string serialise() {
        return internal_serialise("NOT");
    }
}
