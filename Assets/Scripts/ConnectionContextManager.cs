using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionContextManager : MonoBehaviour {

    private Connection selected_connection = null;
    private RectTransform rect;
    private GameObject context_menu;

    private void Start() {
        context_menu = transform.GetChild(0).gameObject;
        rect = context_menu.GetComponent<RectTransform>();
    }

    public void set_selected_connection(Connection connection) {
        selected_connection = connection;
    }

    public void open_menu(Vector2 mouse_pos) {
        context_menu.SetActive(true);
        context_menu.transform.position = mouse_pos + rect.sizeDelta * 0.5f;
    }

    public void close_menu() {
        context_menu.SetActive(false);
    }
}
