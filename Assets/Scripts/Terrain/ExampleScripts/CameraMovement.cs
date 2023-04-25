using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float Speed;
    public float xAmount;
    public float yAmount;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yAmount = 0.5f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            yAmount = -0.5f;
        }
        else
        {
            yAmount = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xAmount = -0.5f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xAmount = 0.5f;
        }
        else
        {
            xAmount = 0;
        }


        transform.Translate(new Vector3(xAmount, yAmount, 0).normalized * Speed * Time.deltaTime);
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y;
        Camera.main.orthographicSize = Mathf.Max(1, Camera.main.orthographicSize);
    }
}
