using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{

    public float speed = 1;
    public float mouseSpeed = 350;

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(new Vector3(hori * speed, mouse * mouseSpeed, vert * speed) * Time.deltaTime, Space.World);
    }
}
 