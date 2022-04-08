using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AnimalEvolution
{
    public interface Entity
    {
        /// <summary>
        /// How much will eating this entity fill an Animal's foodCurrent.
        /// </summary>
        float nutritionalValue { get; set; }
        /// <summary>
        /// How long will the entity live.
        /// </summary>
        float lifeMax { get; set; }
        /// <summary>
        /// How long has the entity lived for.
        /// </summary>
        float lifeCurrent { get; set; }
        /// <summary>
        /// Minimum time between two times the entity can have offspring
        /// </summary>
        float timeToBreedMin { get; set; }
        /// <summary>
        /// Countdown to zero for when the entity can have offspring
        /// </summary>
        float timeToBreedCurrent { get; set; }
        /// <summary>
        /// Size of the entity, Carnivores may only eat smaller Animals.
        /// </summary>
        float size { get; set; }
        /// <summary>
        /// The strength of the mutation the entity's offspring goes through upon creation.
        /// </summary>
        int mutationStrength { get; set; }
        Color color { get; set; }

        /// <summary>
        /// Set used to create mutated child of an existing animalEntity.
        /// </summary>
        /// <param name="parentEntity">the Entity to use as parent</param>
        /// <param name="targetGObject">the game Object to which the code should attach the child Entity</param>
        void SetFrom(Entity parentEntity, GameObject targetGObject);

        void ChangeMyProperties(int property);
    }
    
}
