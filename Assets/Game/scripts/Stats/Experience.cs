using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;

        public delegate bool ExampleDelegate(float value);
        public event ExampleDelegate onDoneStuff;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            bool result = onDoneStuff(5f);
            print(result);
        }

        public float GetPoints()
        {
            return experiencePoints; 
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;

        }
    }
}
