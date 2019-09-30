using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour {

    public readonly float MAX_ZOOM = 10.0f;
    public readonly float MIN_ZOOM = 4.0f;

    private float SCROLL_SCALE = 0.1f;

    private BackgroundScroller background;
    private Camera cam;

    // Start is called before the first frame update
    void Start() {
        background = transform.GetChild(0).GetComponent<BackgroundScroller>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        cam.orthographicSize = clamp_zoom(
            cam.orthographicSize - (Input.mouseScrollDelta.y * SCROLL_SCALE * cam.orthographicSize));
    }

    private float clamp_zoom(float new_zoom) {
        return Mathf.Min(Mathf.Max(new_zoom, MIN_ZOOM), MAX_ZOOM);
    }
}
