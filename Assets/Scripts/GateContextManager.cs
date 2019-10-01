using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GateContextManager : MonoBehaviour {

    public GameObject connection_prefab;
    private Gate selected_gate = null;
    private Connection dangling_connection = null;
    private bool dangling_is_from_gate;
    private GameObject context_menu;
    private Vector2 offset;
    private Graph graph;

    private Button create_input_button;
    private Button switch_input_button;

    private void Start() {
        context_menu = transform.GetChild(0).gameObject;
        context_menu.SetActive(false);
        RectTransform rect = context_menu.GetComponent<RectTransform>();
        offset = Vector2.Scale(rect.sizeDelta * 0.5f, new Vector2(1.0f, -1.0f)) + new Vector2(5.0f, -5.0f);
        graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
        create_input_button = context_menu.transform.GetChild(0).GetComponent<Button>();
        switch_input_button = context_menu.transform.GetChild(1).GetComponent<Button>();
    }

    public bool has_dangling_connection() {
        return dangling_connection != null;
    }

    public void set_selected(Gate gate) {
        selected_gate = gate;
        if (selected_gate.gate_type == GateType.Input) {
            create_input_button.gameObject.SetActive(false);
            switch_input_button.gameObject.SetActive(true);
            if (selected_gate.get_value()) {
                switch_input_button.transform.GetChild(0).GetComponent<Text>().text = "Switch Off";
            } else {
                switch_input_button.transform.GetChild(0).GetComponent<Text>().text = "Switch On";
            }
        } else {
            create_input_button.gameObject.SetActive(true);
            switch_input_button.gameObject.SetActive(false);
        }
    }

    public void open_menu(Vector2 mouse_pos) {
        context_menu.SetActive(true);
        context_menu.transform.position = mouse_pos + offset;
    }

    private void close_menu(bool forget_selected = true) {
        if (forget_selected) {
            selected_gate = null;
        }
        context_menu.SetActive(false);
    }

    public void switch_input() {
        selected_gate.value = !selected_gate.value;
        graph.update_gates();
        close_menu();
    }

    public void left_click() {
        close_on_outside();
        place_connection_on_gate();
    }

    private void close_on_outside() {
        RaycastHit2D screen_space_hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
        if (selected_gate != null && dangling_connection == null) {
            if (screen_space_hit.collider != null) {
                if (screen_space_hit.collider.tag == "ContextMenu") {
                    return;
                }
            }
            close_menu();
        }
    }

    private void place_connection_on_gate() {
        RaycastHit2D world_space_hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (dangling_connection != null && world_space_hit.collider != null) {
            Gate new_gate = world_space_hit.collider.GetComponent<Gate>();
            if (world_space_hit.collider.tag == "Gate" && new_gate != selected_gate && new_gate != null) {
                if (dangling_is_from_gate) {
                    place_from_connection(new_gate);
                } else {
                    place_to_connection(new_gate);
                }
                dangling_connection = null;
                selected_gate = null;
                graph.update_gates();
                return;
            }
        }
        if (dangling_connection != null) {
            cancel_connection();
        }
    }

    private void cancel_connection() {
        Destroy(dangling_connection.gameObject);
        dangling_connection = null;
        selected_gate = null;
    }

    private void place_from_connection(Gate new_gate) {
        if (new_gate.gate_type == GateType.Input) {
            cancel_connection();
            return;
        }
        new_gate.add_incoming_neighbour(selected_gate);
        selected_gate.add_outgoing_neighbour(new_gate);
        new_gate.add_incoming_connection(dangling_connection);
        selected_gate.add_outgoing_connection(dangling_connection);
        dangling_connection.transform.parent = new_gate.transform;
        dangling_connection.set_parent(new_gate);
        dangling_connection.transform.position = new_gate.transform.position;
        dangling_connection.is_temporary = false;
    }

    private void place_to_connection(Gate new_gate) {
        if (selected_gate.gate_type == GateType.Input) {
            cancel_connection();
            return;
        }
        selected_gate.add_incoming_neighbour(new_gate);
        new_gate.add_outgoing_neighbour(selected_gate);
        selected_gate.add_incoming_connection(dangling_connection);
        new_gate.add_outgoing_connection(dangling_connection);
        dangling_connection.set_parent(selected_gate);
        dangling_connection.transform.position = selected_gate.transform.position;
        dangling_connection.is_temporary = false;
    }

    public void delete_gate() {
        selected_gate.delete_all_connections();
        graph.remove_gate(selected_gate);
        Destroy(selected_gate.gameObject);
        graph.update_gates();
        close_menu();
    }

    public void create_output() {
        dangling_connection = Instantiate(
            connection_prefab,
            Vector3.zero,
            Quaternion.identity
        ).GetComponent<Connection>();
        dangling_connection.is_temporary = true;
        dangling_is_from_gate = true;
        close_menu(false);
    }

    public void create_input() {
        dangling_connection = Instantiate(
            connection_prefab,
            selected_gate.transform.position,
            Quaternion.identity
        ).GetComponent<Connection>();
        dangling_connection.is_temporary = true;
        dangling_is_from_gate = false;
        dangling_connection.transform.parent = selected_gate.transform;
        close_menu(false);
    }

    public void Update() {
        if (dangling_connection != null) {
            if (dangling_is_from_gate) {
                dangling_connection.update(
                     Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    selected_gate.get_output_position(),
                    false);
            } else {
                dangling_connection.update(
                    selected_gate.get_input_position(0, 1),
                    (
                        Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                        selected_gate.transform.position
                    ),
                    false);
            }
        }
    }
}
