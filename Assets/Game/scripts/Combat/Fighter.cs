
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attribute;
using RPG.Stats;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float timeBetweenAttacks = 0f;     
        //[SerializeField] Weapon weapon = null;       
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        

        private float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        Weapon currentWeapon = null;

        
        private void Start()
        {
            //print("hello");
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            

        }

        

        private void Update()
        {
            
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if(target.IsDead()) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                //GetComponent<Mover>().Stop();
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            if(weapon == null) { return; }
            //print(weapon.name + " used");
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform,leftHandTransform, animator);//error solve later nullreference
        }

        public Health GetTarget()
        {
            return target;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;

                //This will trigger the Hit() event
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            if(target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject.gameObject, damage);
            }
            else
            {
                
                target.TakeDamage(gameObject, damage);
            }
            
        }

        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionSchedular>().StartAction(this);

            target = combatTarget.GetComponent<Health>();
            
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
