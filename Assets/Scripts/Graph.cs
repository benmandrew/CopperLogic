using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[ExecuteInEditMode]
public class Graph : MonoBehaviour {
    public GameObject gate_container;
    public List<Gate> gates;
    public Gate root;

    private void Start() {
        if (Application.isPlaying) {
            get_gates();
        }
    }

    void get_gates() {
        if (gate_container == null) {
            throw new System.Exception("Graph must be linked to the gate container!");
        }
        foreach (Transform child in gate_container.transform) {
            if (child.gameObject.activeSelf) {
                gates.Add(child.gameObject.GetComponent<Gate>());
            }
        }
    }

    bool get_value() {
        return root.get_value();
    }

    void save_graph_to_file() {
        string json = serialise();
        write_to_graph_file(json);
    }

    string serialise() {
        string json = "[";
        for (int i = 0; i < gates.Count; i++) {
            if (i != 0) {
                json += ", ";
            }
            json += gates[i].serialise();
        }
        return json + "]";
    }

    void write_to_graph_file(string str) {
        StreamWriter writer = new StreamWriter(Config.graph_file_path);
        writer.WriteLine(str);
        writer.Close();
    }
    
    private void FixedUpdate() {
        for (int i = 0; i < gates.Count; i++) {
            if (gates[i].changed) {
                gates[i].draw_all_connections();
                gates[i].changed = false;
            }
        }
    }
}
