using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    private float SCROLL_SCALE = 0.05f;
    private float CAMERA_SCALE = 10.0f;
    private float PLANE_SIZE = 10.0f;
    private float HALF_PLANE_SIZE = 5.0f;
    private float MAX_ZOOM;
    private Camera cam;
    private GameObject cam_obj;
    private float old_cam_size = 0.0f;
    private Vector2 start_mouse_pos;
    private Vector2 texture_offset = Vector2.zero;

    public GameObject background_prefab;
    private List<GameObject> backgrounds;
    private int last_background = 0;

    // Start is called before the first frame update
    void Awake() {
        cam = transform.parent.GetComponent<Camera>();
        cam_obj = cam.gameObject;
        backgrounds = new List<GameObject>();
        CameraManager cam_manager = cam_obj.GetComponent<CameraManager>();
        MAX_ZOOM = cam_manager.MAX_ZOOM;
        instantiate_backgrounds();
        tile_background();
    }

    // Update is called once per frame
    void Update() {
        tile_background();
    }

    private void tile_background() {
        if (cam.orthographicSize == old_cam_size) {
            return;
        }
        old_cam_size = cam.orthographicSize;
        float half_height = cam.orthographicSize;
        float half_width = half_height * cam.aspect;

        // Round to farthest plane boundary
        float min_x = Mathf.Floor((PLANE_SIZE * Mathf.Floor((
            cam.transform.position.x - half_width - HALF_PLANE_SIZE) / PLANE_SIZE)));
        float max_x = Mathf.Floor((PLANE_SIZE * Mathf.Ceil((
            cam.transform.position.x + half_width + HALF_PLANE_SIZE) / PLANE_SIZE)));
        float min_y = Mathf.Floor((PLANE_SIZE * Mathf.Floor((
            cam.transform.position.x - half_height - HALF_PLANE_SIZE) / PLANE_SIZE)));
        float max_y = Mathf.Floor((PLANE_SIZE * Mathf.Ceil((
            cam.transform.position.x + half_height + HALF_PLANE_SIZE) / PLANE_SIZE)));

        disable_backgrounds();
        int index = 0;
        for (int x = (int)min_x; x <= (int)max_x; x += 10) {
            for (int y = (int)min_y; y <= (int)max_y; y += 10) {
                backgrounds[index].transform.position = new Vector3(x, y, 20.0f);
                backgrounds[index].SetActive(true);
                index++;
            }
        }
    }

    private void disable_backgrounds() {
        for (int i = 0; i < backgrounds.Count; i++) {
            backgrounds[i].SetActive(false);
        }
    }

    private void instantiate_backgrounds() {
        float half_height = MAX_ZOOM;
        float half_width = half_height * cam.aspect;
        
        float min_x = Mathf.Floor((PLANE_SIZE * Mathf.Floor((
            cam.transform.position.x - half_width - HALF_PLANE_SIZE) / PLANE_SIZE)));
        float max_x = Mathf.Floor((PLANE_SIZE * Mathf.Ceil((
            cam.transform.position.x + half_width + HALF_PLANE_SIZE) / PLANE_SIZE)));
        float min_y = Mathf.Floor((PLANE_SIZE * Mathf.Floor((
            cam.transform.position.x - half_height - HALF_PLANE_SIZE) / PLANE_SIZE)));
        float max_y = Mathf.Floor((PLANE_SIZE * Mathf.Ceil((
            cam.transform.position.x + half_height + HALF_PLANE_SIZE) / PLANE_SIZE)));

        for (int x = (int)min_x; x <= (int)max_x; x += 10) {
            for (int y = (int)min_y; y <= (int)max_y; y += 10) {
                GameObject background = Instantiate(background_prefab);
                backgrounds.Add(background);
                background.transform.parent = transform;
                background.SetActive(false);
            }
        }
    }

    public void start_scroll() {
        start_mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void scroll() {
        Vector2 new_mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        texture_offset += SCROLL_SCALE * (new_mouse_pos - start_mouse_pos);
        Vector2 camera_offset = texture_offset * CAMERA_SCALE;
        cam_obj.transform.position = new Vector3(-camera_offset.x, -camera_offset.y, -10.0f);
        for (int i = 0; i < backgrounds.Count; i++) {
            Renderer rend = backgrounds[i].GetComponent<Renderer>();
            rend.material.SetTextureOffset("_MainTex", texture_offset);
        }
        start_mouse_pos = new_mouse_pos;
    }
}
