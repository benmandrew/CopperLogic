using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool being_dragged = false;
    private Vector2 mouse_relative_start_pos;

    public void set_new_pos(Vector2 new_pos) {
        transform.position = new_pos + mouse_relative_start_pos;
    }

    public void set_mouse_start_pos(Vector3 mouse_pos) {
        mouse_relative_start_pos = transform.position - mouse_pos;
    }

    public void select(bool is_selected) {
        being_dragged = is_selected;
    }
}
