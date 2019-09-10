using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickManager : MonoBehaviour {

    private Draggable clicked_gate = null;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mouse_down();
        }
        if (Input.GetMouseButton(0)) {
            mouse_pressed();
        }
        if (Input.GetMouseButtonUp(0)) {
            mouse_up();
        }
    }

    void mouse_down() {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouse_pos, Vector2.zero);
        Debug.Log(hit);
        if (hit.collider != null) {
            Debug.Log(hit.collider);
            if (hit.collider.gameObject.GetComponent<Gate>() != null) {
                Debug.Log("OBJ");
                clicked_gate = hit.transform.gameObject.GetComponent<Draggable>();
                clicked_gate.set_mouse_start_pos(hit.point);
                clicked_gate.select(true);
            }
        }
    }

    void mouse_pressed() {
        if (clicked_gate != null) {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked_gate.set_new_pos(mouse_pos);
        }
    }

    void mouse_up() {
        if (clicked_gate != null) {
            clicked_gate.select(false);
            clicked_gate = null;
        } }
}
