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

    public GameObject connection_prefab;
    protected List<Connection> connections = new List<Connection>();

    private void Awake() {
        id = top_id;
        top_id++;
        if (connection_prefab == null) {
            throw new System.Exception("Connection prefab must be filled out");
        }
        Debug.Log(incoming_neighbours);
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
        for (int i = 0; i < incoming_neighbours.Count; i++) {
            connections[i].update(
                incoming_neighbours[i].transform.position,
                transform.position,
                incoming_neighbours[i].get_value()
            );
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