using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestText : MonoBehaviour
{
    public string text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        GUI.matrix = Matrix4x4.Scale(Vector3.one * 2);
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Space(100);
            GUILayout.Label(text);
        }
    }

}
