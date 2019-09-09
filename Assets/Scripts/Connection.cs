using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SplineMesh.Spline))]
public class Connection : MonoBehaviour {
    private SplineMesh.Spline spline;

    public void Start() {
        spline = GetComponent<SplineMesh.Spline>();
    }

    public void update_position(Vector2 input_pos, Vector2 output_pos) {
        Vector2 mid_point = Vector2.Scale(input_pos + output_pos, new Vector2(0.5f, 0.5f));
        Vector2 input_anchor = new Vector2(mid_point.x, input_pos.y);
        Vector2 mid_point_anchor = new Vector2(mid_point.x, output_pos.y);
        Vector2 output_anchor = output_pos + input_anchor - input_pos;
        spline.nodes[0].Position = input_pos;
        spline.nodes[0].Direction = input_anchor;
        spline.nodes[1].Position = mid_point;
        spline.nodes[1].Position = mid_point_anchor;
        spline.nodes[2].Position = output_pos;
        spline.nodes[2].Position = output_anchor;
        Debug.Log(input_pos);
        Debug.Log(output_pos);
        Debug.Log(mid_point);
        Debug.Log(input_anchor);
        Debug.Log(mid_point_anchor);
        Debug.Log(output_anchor);
        Debug.Log(" ");
    }
}