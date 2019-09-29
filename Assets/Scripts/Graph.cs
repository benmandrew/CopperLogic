using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[ExecuteInEditMode]
public class Graph : MonoBehaviour {
    public GameObject gate_container;
    public List<Gate> gates;

    private void Awake() {
        if (Application.isPlaying) {
            get_gates();
            update_gates();
        }
    }

    private void get_gates() {
        if (gate_container == null) {
            throw new System.Exception("Graph must be linked to the gate container!");
        }
        foreach (Transform child in gate_container.transform) {
            if (child.gameObject.activeSelf) {
                gates.Add(child.gameObject.GetComponent<Gate>());
            }
        }
    }

    public void remove_gate(Gate gate) {
        gates.Remove(gate);
    }
    
    public void update_gates() {
        for (int i = 0; i < gates.Count; i++) {
            gates[i].changed = true;
        }
        for (int i = 0; i < gates.Count; i++) {
            gates[i].draw_all_connections();
            gates[i].changed = false;
        }
    }

    private void LateUpdate() {
        for (int i = 0; i < gates.Count; i++) {
            if (gates[i].changed) {
                gates[i].draw_all_connections();
                gates[i].changed = false;
            }
        }
    }
}
