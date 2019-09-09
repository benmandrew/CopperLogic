using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Config : MonoBehaviour
{
    public static string graph_file_path = "Assets/Resources/graph.json";

    public static Config config;

    void Awake() {
        if (config != null) {
            GameObject.Destroy(config);
        } else {
            config = this;
        }
        DontDestroyOnLoad(this);
    }
}
