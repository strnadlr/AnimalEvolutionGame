using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimalEvolution
{
    public interface Entity
    {
        float nutritionalValue { get; set; }
        float lifeMax { get; set; }
        float lifeRemaining { get; set; }
        float timeWithoutChildren { get; set; }
        float currentTimeWithoutChild { get; set; }
        float size { get; set; }
        int mutationStrength { get; set; }
        Color color { get; set; }

        void SetFrom(Entity parentEntity, GameObject targetGObject);
    }
    
}
