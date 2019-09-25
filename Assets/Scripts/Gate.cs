using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<Gate> outgoing_neighbours;
    private List<int> incoming_index;

    private Vector2 connection_offset = new Vector2(0.3f, 0.0f);
    private Vector2 connection_array_max_offset = new Vector2(0.0f, 1.0f);

    public GameObject connection_prefab;
    protected List<Connection> incoming_connections = new List<Connection>();
    protected List<Connection> outgoing_connections = new List<Connection>();
    private Comp comp;
    public bool changed = true;

    private void Awake() {
        comp = new Comp(transform.position);
        id = top_id;
        top_id++;
        if (connection_prefab == null) {
            throw new System.Exception("Connection prefab must be filled out");
        }
        if (Application.isPlaying) {
            for (int i = 0; i < incoming_neighbours.Count; i++) {
                Connection connection = Instantiate(
                    connection_prefab,
                    transform.position,
                    Quaternion.identity
                ).GetComponent<Connection>();
                connection.transform.parent = transform;
                incoming_connections.Add(connection);
                incoming_neighbours[i].add_outgoing_connection(connection);
                incoming_neighbours[i].add_outgoing_neighbour(this);
            }
        }
    }

    public abstract bool get_value();

    public abstract string serialise();
    
    public void add_outgoing_neighbour(Gate gate) {
        outgoing_neighbours.Add(gate);
    }

    public void draw_all_connections() {
        draw_all_incoming_connections();
        for (int i = 0; i < outgoing_neighbours.Count; i++) {
            outgoing_neighbours[i].draw_all_incoming_connections();
        }
    }

    public void draw_all_incoming_connections() {
        sort_incoming_neighbours();
        for (int i = 0; i < incoming_index.Count; i++) {
            incoming_connections[i].update(
                get_input_position(
                    incoming_index[i],
                    incoming_neighbours.Count),
                incoming_neighbours[i].get_relative_output_position(
                    transform.position),
                incoming_neighbours[i].get_value()
            );
        }
    }

    private void sort_incoming_neighbours() {
        comp.set_pos(transform.position);
        int[] permutation = Enumerable.Range(0, incoming_neighbours.Count).ToArray();
        Gate[] gates = incoming_neighbours.ToArray();
        Array.Sort(gates, permutation, comp);
        incoming_index = permutation.ToList();
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

    public void add_outgoing_connection(Connection connection) {
        outgoing_connections.Add(connection);
    }

    public void delete_incoming_connection(Connection connection) {
        incoming_connections.Remove(connection);
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            if (incoming_neighbours[i].outgoing_connections.Contains(connection)) {
                Debug.Log(incoming_neighbours[i]);
                incoming_neighbours.RemoveAt(i);
            }
        }
    }

    public void delete_all_connections() {
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            Gate neighbour = incoming_neighbours[i];
            for (int j = neighbour.outgoing_connections.Count - 1;  j >= 0; j--) {
                Connection connection = neighbour.outgoing_connections[j];
                if (incoming_connections.Contains(connection)) {
                    neighbour.outgoing_connections.Remove(connection);
                    incoming_connections.Remove(connection);
                    neighbour.outgoing_neighbours.Remove(this);
                }
            }
        }
        for (int i = 0; i < outgoing_neighbours.Count; i++) {
            Gate neighbour = outgoing_neighbours[i];
            for (int j = neighbour.incoming_connections.Count - 1; j >= 0; j--) {
                Connection connection = neighbour.incoming_connections[j];
                if (outgoing_connections.Contains(connection)) {
                    neighbour.incoming_connections.Remove(connection);
                    outgoing_connections.Remove(connection);
                    Destroy(connection.gameObject);
                    neighbour.incoming_neighbours.Remove(this);
                }
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