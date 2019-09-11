using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Connection : MonoBehaviour {
    
    private LineRenderer rend;
    private bool is_on = false;

    private static Color on_colour = new_colour(146, 161, 122); // Meadow green
    private static Color off_colour = new_colour(255, 253, 240); // Pale white

    public void Start() {
        rend = GetComponent<LineRenderer>();
        rend.startColor = off_colour;
        rend.endColor = off_colour;
    }

    public void update(Vector2 input_pos, Vector2 output_pos, bool is_on_new) {
        update_position(input_pos, output_pos);
        update_colour(is_on_new);
    }

    public void update_position(Vector2 input_pos, Vector2 output_pos) {
        if (input_pos.Equals(rend.GetPosition(0)) && output_pos.Equals(rend.GetPosition(3))) {
            return;
        }
        float input_third_x = (input_pos.x * 2 + output_pos.x) / 3;
        float output_third_x = (output_pos.x * 2 + input_pos.x) / 3;
        Vector3 input_pos_3d = new Vector3(input_pos.x, input_pos.y, 1.0f);
        Vector3 output_pos_3d = new Vector3(output_pos.x, output_pos.y, 1.0f);
        Vector3 input_anchor = new Vector3(input_third_x, input_pos.y, 1.0f);
        Vector3 output_anchor = new Vector3(output_third_x, output_pos.y, 1.0f);
        rend.positionCount = 4;
        rend.SetPositions(new Vector3[4] { input_pos_3d, input_anchor, output_anchor, output_pos_3d });
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

    private static Color new_colour(int r, int g, int b) {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
}