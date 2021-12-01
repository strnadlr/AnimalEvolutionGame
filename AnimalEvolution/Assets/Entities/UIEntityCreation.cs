using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEntityCreation : MonoBehaviour
{
    public Text nutriValueText;
    public Text timeBeOfText;
    public Text sizeText;
    public Text mutationStrText;

    public GameObject panel;
    private bool active = false;
    

    // Start is called before the first frame update
    private void Start()
    {
        panel.SetActive(active);
    }

    public void EntityUIButtonClicked()
    {
        active = !active;
        panel.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
