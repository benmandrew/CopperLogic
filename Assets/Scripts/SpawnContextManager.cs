using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContextManager : MonoBehaviour {

    public GameObject and_gate_prefab;
    public GameObject or_gate_prefab;
    public GameObject xor_gate_prefab;
    public GameObject not_gate_prefab;
    public GameObject input_gate_prefab;

    private GameObject context_menu;
    private Vector2 offset;
    private Graph graph;
    
    void Start() {
        context_menu = transform.GetChild(0).gameObject;
        context_menu.SetActive(false);
        RectTransform rect = context_menu.GetComponent<RectTransform>();
        offset = Vector2.Scale(rect.sizeDelta * 0.5f, new Vector2(1.0f, -1.0f)) + new Vector2(5.0f, -5.0f);
        graph = GameObject.FindGameObjectWithTag("Graph").GetComponent<Graph>();
    }

    public void open_menu(Vector2 mouse_pos) {
        context_menu.SetActive(true);
        context_menu.transform.position = mouse_pos + offset;
    }

    public void left_click() {
        close_on_outside();
    }

    private void close_on_outside() {
        RaycastHit2D screen_space_hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
        if (screen_space_hit.collider != null) {
            if (screen_space_hit.collider.tag == "ContextMenu") {
                return;
            }
        }
        context_menu.SetActive(false);
    }

    private void spawn_gate(GameObject gate_prefab) {
        Vector3 offset_3d = offset;
        Vector3 position = Camera.main.ScreenToWorldPoint(context_menu.transform.position - offset_3d);
        position.z = 0.0f;
        GameObject gate = Instantiate(
            gate_prefab,
            position,
            Quaternion.identity);
        graph.gates.Add(gate.GetComponent<Gate>());
        gate.transform.parent = graph.gate_container.transform;
        graph.update_gates();
        context_menu.SetActive(false);
    }

    public void spawn_and_gate() {
        spawn_gate(and_gate_prefab);
    }

    public void spawn_or_gate() {
        spawn_gate(or_gate_prefab);
    }

    public void spawn_xor_gate() {
        spawn_gate(xor_gate_prefab);
    }

    public void spawn_not_gate() {
        spawn_gate(not_gate_prefab);
    }

    public void spawn_input_gate() {
        spawn_gate(input_gate_prefab);
    }
}
