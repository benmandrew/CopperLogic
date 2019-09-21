using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickManager : MonoBehaviour {

    private Draggable clicked_gate = null;
    private Draggable hovered_gate = null;

    public GameObject gate_context_menu;
    public GameObject connection_context_menu;
    private GateContextManager gate_context_manager;
    private ConnectionContextManager connection_context_manager;

    private void Start() {
        gate_context_manager = gate_context_menu.GetComponent<GateContextManager>();
        connection_context_manager = connection_context_menu.GetComponent<ConnectionContextManager>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mouse_one_down();
            gate_context_manager.close_menu();
            connection_context_manager.close_menu();
        }
        if (Input.GetMouseButton(0)) {
            mouse_one_pressed();
        }
        if (Input.GetMouseButtonUp(0)) {
            mouse_one_up();
        }
        if (Input.GetMouseButtonDown(1)) {
            gate_context_manager.close_menu();
            connection_context_manager.close_menu();
            mouse_two_down();
        }
    }

    private void mouse_one_down() {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouse_pos, Vector2.zero);
        if (hit.collider != null) {
            if (hit.collider.gameObject.GetComponent<Gate>() != null) {
                clicked_gate = hit.transform.gameObject.GetComponent<Draggable>();
                clicked_gate.set_mouse_start_pos(hit.point);
                clicked_gate.select(true);
            }
        }
    }

    private void mouse_one_pressed() {
        if (clicked_gate != null) {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked_gate.set_new_pos(mouse_pos);
        }
    }

    private void mouse_one_up() {
        if (clicked_gate != null) {
            clicked_gate.select(false);
            clicked_gate = null;
        }
    }

    private void mouse_two_down() {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouse_pos, Vector2.zero);
        if (hit.collider != null) {
            if (hit.collider.gameObject.GetComponent<Gate>() != null) {
                gate_context_manager.open_menu(Input.mousePosition);
            } else if (hit.collider.gameObject.GetComponent<Connection>() != null) {
                connection_context_manager.open_menu(Input.mousePosition);
            }
        }
    }
}
