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
        private Vector3 lastMousePosRot;

        /// <summary>
        /// This allows all camera movement to be turned on or off.
        /// </summary>
        /// <param name="target">True if the camera movement should be on.</param>
        /// <returns></returns>
        public bool MovementSwitch(bool target)
        {
            bool prev = movement;
            movement = target;
            return prev;
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
                    if (Input.GetMouseButtonDown(1))
                    {
                        lastMousePosRot = Input.mousePosition;
                    }
                    if (Input.GetMouseButton(1))
                    {
                        int rot = 1;
                        if ((Input.mousePosition - lastMousePosRot).x>0)rot = -1;
                        rotation.y += rot* Vector3.Distance(Input.mousePosition, lastMousePosRot) * panSpeed * Time.deltaTime * 0.05f;
                        lastMousePosRot = Input.mousePosition;
                    }
                    if (Input.GetKey("q"))
                    {
                        rotation.y += panSpeed * 0.5f * Time.deltaTime;
                    }
                    if (Input.GetKey("e"))
                    {
                        rotation.y -= panSpeed * 0.5f * Time.deltaTime;
                    }
                    if (Input.mouseScrollDelta.y != 0)
                    {
                        position += transform.forward * panSpeed * Time.deltaTime * Input.mouseScrollDelta.y * 8;
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