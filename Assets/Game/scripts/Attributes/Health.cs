
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using System;
using UnityEngine;

namespace RPG.Attribute
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;

        float healthPoints = -1f;
        bool isDead = false;

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if (healthPoints < 0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }            
        }
        public void TakeDamage(GameObject instigator,float damage)
        {
            print(gameObject.name + " took damage : " + damage);
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                AwardExperience(instigator);//put this after die
                Die();
                
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100* (healthPoints/GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void Die()
        {
            if(isDead)
            {
                return;
            }
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionSchedular>().CancelCurrentAction();
            
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if(healthPoints <= 0)
            {
                Die();
            }
        }
    }
}
