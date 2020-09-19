using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    Vector3 basePosition = new Vector3(200, 400, -20);
    Quaternion baseRotation = Quaternion.Euler(60, 0, 0);


    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey("h"))
        {
            transform.position = basePosition;
            transform.rotation = baseRotation;
        }
        else
        {
            Vector3 position = transform.position;
            Vector3 rotation = transform.rotation.eulerAngles;

            if (Input.GetKey("w"))
            {
                position += transform.forward * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                position -= transform.forward * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d"))
            {
                position += transform.right * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("a"))
            {
                position -= transform.right * panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("q"))
            {
                rotation.y += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("e"))
            {
                rotation.y -= panSpeed * Time.deltaTime;
            }

            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);
        }


    }
}
