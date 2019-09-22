using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Comp : IComparer<Gate> {

    private Vector2 gate_pos;

    public Comp(Vector2 new_gate_pos) {
        gate_pos = new_gate_pos;
    }

    public void set_pos(Vector2 new_gate_pos) {
        if (gate_pos.Equals(new_gate_pos)) {
            return;
        }
        gate_pos = new_gate_pos;
    }

    private float reverse_angle(float angle) {
        if (angle >= 0) {
            return Mathf.PI - angle;
        }
        return -(Mathf.PI + angle);
    }

    public int Compare(Gate g0, Gate g1) {
        Vector2 g0_pos = g0.transform.position;
        Vector2 g1_pos = g1.transform.position;
        Vector2 g0_relative = g0_pos - gate_pos;
        Vector2 g1_relative = g1_pos - gate_pos;
        float scale = 3.0f;
        float g0_atan = Mathf.Atan2(g0_relative.y * scale, g0_relative.x);
        float g1_atan = Mathf.Atan2(g1_relative.y * scale, g1_relative.x);
        if (g0_relative.x < 0.0f) {
            g0_atan = reverse_angle(g0_atan);
        }
        if (g1_relative.x < 0.0f) {
            g1_atan = reverse_angle(g1_atan);
        }

        return g0_atan.CompareTo(g1_atan);
    }
}

[ExecuteInEditMode]
public abstract class Gate : MonoBehaviour {
    static int top_id = 0;

    public int id;
    public bool value = false;
    protected bool value_calculated = false;
    public List<Gate> incoming_neighbours;

    private Vector2 connection_offset = new Vector2(0.3f, 0.0f);
    private Vector2 connection_array_max_offset = new Vector2(0.0f, 1.0f);

    public GameObject connection_prefab;
    protected List<Connection> connections = new List<Connection>();
    private Comp comp;

    private void Awake() {
        comp = new Comp(transform.position);
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
        comp.set_pos(transform.position);
        incoming_neighbours.Sort(comp);
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            connections[i].update(
                get_input_position(i, incoming_neighbours.Count),
                incoming_neighbours[i].get_relative_output_position(
                    transform.position),
                incoming_neighbours[i].get_value()
            );
        }
    }

    public Vector2 get_input_position(int input_index, int input_num) {
        Vector2 array_offset = connection_array_max_offset * ((input_index - ((input_num - 1.0f) / 2.0f)) / input_num);
        return array_offset - connection_offset;
    }

    public Vector2 get_output_position() {
        return new Vector2(transform.position.x, transform.position.y) + connection_offset;
    }

    public Vector2 get_relative_output_position(Vector2 other) {
        return get_output_position() - other;
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