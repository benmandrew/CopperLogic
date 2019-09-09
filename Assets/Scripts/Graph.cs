using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[ExecuteInEditMode]
public class Graph : MonoBehaviour {
    public List<Gate> gates;
    public Gate root;

    private void Start() {
        /*
        StreamReader reader = new StreamReader(Config.graph_file_path);
        string json = reader.ReadToEnd();
        reader.Close();
        */
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
            gates[i].draw_connections();
        }
    }
}
