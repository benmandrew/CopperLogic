using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GateType {
    AND,
    OR,
    NOT
};


public class Gate : MonoBehaviour {
    public int id;
    public GateType type;
    public List<Gate> incoming_neighbours;

    private void Start() {

    }

    public string serialise() {
        string json = (
            "{\"id\": \"" +
            id.ToString() +
            "\",\"type\": \"" +
            type.ToString() +
            "\",\"edge_ids\": ["
        );
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            if (i != 0) {
                json += ", ";
            }
            json += incoming_neighbours[i].id.ToString();
        }

        return json + "]}";
    }
}