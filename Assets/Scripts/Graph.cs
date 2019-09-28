using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[ExecuteInEditMode]
public class Graph : MonoBehaviour {
    public GameObject gate_container;
    public List<Gate> gates;

    private void Start() {
        if (Application.isPlaying) {
            get_gates();
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

    public void evaluate() {
        for (int i = 0; i < gates.Count; i++) {
            gates[i].get_value();
        }
    }

    public void save_graph_to_file() {
        string json = serialise();
        write_to_graph_file(json);
    }

    private string serialise() {
        string json = "[";
        for (int i = 0; i < gates.Count; i++) {
            if (i != 0) {
                json += ", ";
            }
            json += gates[i].serialise();
        }
        return json + "]";
    }

    private void write_to_graph_file(string str) {
        StreamWriter writer = new StreamWriter(Config.graph_file_path);
        writer.WriteLine(str);
        writer.Close();
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
