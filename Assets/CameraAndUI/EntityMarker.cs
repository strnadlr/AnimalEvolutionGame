using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnimalEvolution
{
    public class EntityMarker : MonoBehaviour
    {
        GameObject selected;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (selected != null)
            {
                gameObject.SetActive(true);
                gameObject.transform.position = selected.transform.position;
                gameObject.transform.rotation = selected.transform.rotation;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void EntitySelected(GameObject selectedEntity)
        {
            selected = selectedEntity;
            if (selected != null)
            {
                gameObject.transform.localScale = new Vector3(selectedEntity.transform.localScale.x + 0.5f, selectedEntity.transform.localScale.y + 0.5f, selectedEntity.transform.localScale.z + 0.5f);
                gameObject.SetActive(true);
            }
        }
    }
}