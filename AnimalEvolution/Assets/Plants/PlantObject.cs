using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantObject
{
    static System.Random r;
    float childDistance;
    float childTime;
    float size;
    /// <summary>
    /// Mutation Rate
    /// </summary>
    public int mR;

    public PlantObject(float _childDistance, float _childTime, float _size, int _mR, Color _color)
    {
        childDistance = _childDistance;
        childTime = _childTime;
        size = _size;
        mR = _mR;
        //GetComponent<Material>().color = _color;
    }

    public PlantObject(PlantObject parent)
    {
        childDistance = parent.childDistance + r.Next(-mR, mR) / 100f;
        childTime = parent.childTime + r.Next(-mR, mR) / 100f;
        size = parent.size + r.Next(-mR, mR) / 100f;
        //Material material = GetComponent<Material>();
       /* Material parentMaterial = parent.GetComponent<Material>();
        material.color = new Color(
            (parentMaterial.color.r + r.Next(-mR, mR) / 100f) % 1,
            (parentMaterial.color.g + r.Next(-mR, mR) / 100f) % 1,
            (parentMaterial.color.b + r.Next(-mR, mR) / 100f) % 1
            );*/
    }
}


