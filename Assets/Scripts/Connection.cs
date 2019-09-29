using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Connection : MonoBehaviour {
    
    private LineRenderer rend;
    private MeshCollider meshColl;
    private Mesh temp_mesh;
    private bool is_on = false;
    public bool is_temporary = false;
    private Gate input_parent;
    private Gate output_parent;

    private static Color on_colour = new_colour(146, 161, 122); // Meadow green
    private static Color off_colour = new_colour(255, 253, 240); // Pale white

    public void Awake() {
        rend = GetComponent<LineRenderer>();
        rend.startColor = off_colour;
        rend.endColor = off_colour;
        meshColl = GetComponent<MeshCollider>();
        meshColl.sharedMesh = new Mesh();
    }

    public void set_parent(Gate gate) {
        input_parent = gate;
    }

    public void update(Vector2 input_pos, Vector2 output_pos, bool is_on_new) {
        update_position(input_pos, output_pos);
        update_colour(is_on_new);
        temp_mesh = new Mesh();
        //rend.BakeMesh(temp_mesh, true);
        meshColl.sharedMesh = temp_mesh;
    }

    public void update_position(Vector2 input_pos, Vector2 output_pos) {
        if (input_pos.x - 2.0f > output_pos.x) {
            update_position_behind(input_pos, output_pos);
        } else {
            update_position_infront(input_pos, output_pos);
        }
    }

    private void update_position_behind(Vector2 input_pos, Vector2 output_pos) {
        float input_third_x = (input_pos.x * 2 + output_pos.x) / 3;
        float output_third_x = (output_pos.x * 2 + input_pos.x) / 3;
        Vector3 input_pos_3d = new Vector3(input_pos.x, input_pos.y, 1.0f);
        Vector3 output_pos_3d = new Vector3(output_pos.x, output_pos.y, 1.0f);
        Vector3 input_anchor = new Vector3(input_third_x, input_pos.y, 1.0f);
        Vector3 output_anchor = new Vector3(output_third_x, output_pos.y, 1.0f);
        rend.positionCount = 4;
        rend.SetPositions(new Vector3[4] {
            input_pos_3d, input_anchor, output_anchor, output_pos_3d
        });
    }

    private void update_position_infront(Vector2 input_pos, Vector2 output_pos) {
        Vector3 corner_modifier = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 anchor_modifier = new Vector3(1.0f, -1.0f, 0.0f);
        if (output_pos.y < input_pos.y) {
            corner_modifier.y *= -1;
            anchor_modifier.y *= -1;
        }
        Vector3 input_pos_3d = new Vector3(input_pos.x, input_pos.y, 1.0f);
        Vector3 output_pos_3d = new Vector3(output_pos.x, output_pos.y, 1.0f);
        Vector3 input_corner = input_pos_3d - corner_modifier;
        Vector3 output_corner = output_pos_3d + corner_modifier;
        Vector3 input_anchor = input_pos_3d - anchor_modifier;
        Vector3 output_anchor = output_pos_3d + anchor_modifier;
        rend.positionCount = 6;
        rend.SetPositions(new Vector3[6] {
            input_pos_3d, input_corner, input_anchor, output_anchor, output_corner, output_pos_3d
        });
    }

    public void update_colour(bool is_on_new) {
        if (is_on == is_on_new) {
            return;
        }
        is_on = is_on_new;
        if (is_on_new) {
            rend.startColor = on_colour;
            rend.endColor = on_colour;
        } else {
            rend.startColor = off_colour;
            rend.endColor = off_colour;
        }
    }

    public void set_visibility(bool visible) {
        rend.enabled = visible;
    }

    public void delete_from_parents() {
        input_parent.delete_incoming_connection(this);
    }

    public void add_to_output_parent(Gate gate) {
        output_parent = gate;
    }

    private static Color new_colour(int r, int g, int b) {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}
