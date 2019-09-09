using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public abstract class Gate : MonoBehaviour {
    static int top_id = 0;

    public int id;
    public bool value = false;
    protected bool value_calculated = false;
    public List<Gate> incoming_neighbours;
    
    private Color off_colour = Color.black;
    private Color on_colour = Color.green;

    private void Awake() {
        id = top_id;
        top_id++;
    }

    public abstract bool get_value();

    public abstract string serialise();

    public void draw_connections() {
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            if (incoming_neighbours[i].get_value()) {
                Debug.DrawLine(
                    transform.position,
                    incoming_neighbours[i].transform.position,
                    on_colour
                );
            } else {
                Debug.DrawLine(
                    transform.position,
                    incoming_neighbours[i].transform.position,
                    off_colour
                );
            }
        }
    }

    protected string internal_serialise(string type) {
        string json = (
            "{\"id\": \"" +
            id.ToString() +
            "\",\"type\": \"" +
            type +
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