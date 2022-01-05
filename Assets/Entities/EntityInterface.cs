using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimalEvolution
{
    public interface Entity
    {
        void SetFrom(Entity parent, GameObject targetGObject);
    }
    
}
