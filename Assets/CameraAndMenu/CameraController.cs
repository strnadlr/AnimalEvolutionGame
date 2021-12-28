using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalEvolution
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {

        public float panSpeed = 20f;
        Vector3 basePosition = new Vector3(200, 400, -20);
        Quaternion baseRotation = Quaternion.Euler(60, 0, 0);
        private bool movement = false;

        public void MovementSwitch(bool target)
        {
            movement = target;
        }

        // Update is called once per frame
        void Update()
        {

            if (movement)
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
                        Vector3 where = new Vector3(transform.forward.x, 0, transform.forward.z);
                        position += where * panSpeed * Time.deltaTime;
                    }
                    if (Input.GetKey("s"))
                    {
                        Vector3 where = new Vector3(transform.forward.x, 0, transform.forward.z);
                        position -= where * panSpeed * Time.deltaTime;
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
                        rotation.y += panSpeed * 0.5f * Time.deltaTime;
                    }
                    if (Input.GetKey("e"))
                    {
                        rotation.y -= panSpeed * 0.5f * Time.deltaTime;
                    }
                    if (Input.GetKey("r"))
                    {
                        position += transform.forward * panSpeed * Time.deltaTime;
                    }
                    if (Input.GetKey("f"))
                    {
                        position -= transform.forward * panSpeed * Time.deltaTime;
                    }

                    transform.position = position;
                    transform.rotation = Quaternion.Euler(rotation);
                }

            }
        }
    }
}