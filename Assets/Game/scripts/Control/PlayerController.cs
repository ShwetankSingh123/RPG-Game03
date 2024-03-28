using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Core;
using RPG.Attribute;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        // this script is responsible for controlling all the things like input controls 

        private void Awake()
        {
            health = GetComponent<Health>();
        }


        void Update()
        {
            if(health.IsDead()) { return; }
            if (InteractWithCombat()) { return; }
            if (InteractWithMovement()) { return; }
            //print("nothing is there");
            

        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if(target == null) { continue; }

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) {  continue; }
                if (target == null) { continue; }

                if(Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);                   
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
                
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                    {
                        GetComponent<Mover>().StartMoveAction(hit.point,1f); 
                    }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}