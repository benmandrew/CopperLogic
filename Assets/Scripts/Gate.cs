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

    private Vector2 connection_offset = new Vector2(0.1f, 0.0f);
    private Vector2 connection_array_max_offset = new Vector2(0.0f, 0.5f);

    public GameObject connection_prefab;
    protected List<Connection> connections = new List<Connection>();

    private void Awake() {
        id = top_id;
        top_id++;
        if (connection_prefab == null) {
            throw new System.Exception("Connection prefab must be filled out");
        }
        if (Application.isPlaying) {
            for (int i = 0; i < incoming_neighbours.Count; i++) {
                GameObject connection_gameobject = Instantiate(connection_prefab, transform.position, Quaternion.identity);
                connection_gameobject.transform.parent = transform;
                connections.Add(connection_gameobject.GetComponent<Connection>());
            }
        }
    }

    public abstract bool get_value();

    public abstract string serialise();
    
    public void draw_connections() {
        incoming_neighbours.Sort(sort_by_height);
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            connections[i].update(
                incoming_neighbours[i].get_output_position(),
                get_input_position(i, incoming_neighbours.Count),
                incoming_neighbours[i].get_value()
            );
        }
    }

    static int sort_by_height(Gate g0, Gate g1) {
        return g0.transform.position.y.CompareTo(g1.transform.position.y);
    }

    public Vector2 get_input_position(int input_index, int input_num) {
        Vector2 array_offset = connection_array_max_offset * ((input_index - ((input_num - 1.0f) / 2.0f)) / input_num);
        return new Vector2(transform.position.x, transform.position.y) - connection_offset + array_offset;
    }

    public Vector2 get_output_position() {
        return new Vector2(transform.position.x, transform.position.y) + connection_offset;
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