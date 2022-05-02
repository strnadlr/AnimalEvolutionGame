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
        /// ID of a selected entity, used for log info.
        /// </summary>
        ulong ID { get; set; }

        /// <summary>
        /// Used to determin if the Update function should be called.
        /// </summary>
        bool valid { get; set; }

        /// <summary>
        /// Set used to create mutated child of an existing animalEntity.
        /// </summary>
        /// <param name="parentEntity">the Entity to use as parent</param>
        void SetFrom(Entity parentEntity);

        /// <summary>
        /// Edit properties by 10% or complete an action.
        /// </summary>
        /// <param name="property"> 0 add life, 1 add food, 2 remove food, 3 kil, 4 make offspring</param>
        void ChangeMyProperties(int property);

        /// <summary>
        /// Creates a log of the Entity's death and adds it to the log file.s
        /// </summary>
        /// <param name="cause">Describe the cause (and causer) of death</param>
        void LogDeath(string cause);
    }
    
}
