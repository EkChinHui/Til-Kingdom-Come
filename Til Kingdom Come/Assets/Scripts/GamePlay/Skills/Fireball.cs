using System.Collections;
using System.Runtime.CompilerServices;
using GamePlay.Player;
using UnityEngine;

namespace GamePlay.Skills
{
    public class Fireball : Skill
    {
        private GameObject fireParticles;
        public GameObject damageCollider;
        private float fireBreathDelay = 0.4f;
        private float originOffset = 10f;
        private float fireDuration = 5f;
        
        // Start is called before the first frame update
        void Start()
        {
            skillName = "Fireball";
            skillInfo = "Throws a fireball burning the ground where the fireball lands";
            skillCooldown = 10f;
            skillNumber = 4;
        }

        public override void Cast(PlayerController opponent)
        {
            if (!CanCast()) return;
            StartCoroutine(AnimDelay());
            StartCoroutine(SpawnFire());
            StartCoroutine(player.cooldownUiController.skillIcon.ChangesFillAmount(skillCooldown));
        }

        private IEnumerator AnimDelay()
        {
            player.anim.SetTrigger(skillName);
            var animDelay = AnimationTimes.instance.FireBallAnim;
            player.combatState = PlayerController.CombatState.Skill;
            yield return new WaitForSeconds(animDelay);
            player.combatState = PlayerController.CombatState.NonCombat;
            yield return null;
        }

        private IEnumerator SpawnFire()
        {
            yield return new WaitForSeconds(fireBreathDelay);
            var direction = Mathf.Abs(player.transform.localRotation.eulerAngles.y - 180) < Mathf.Epsilon
                ? -1 *  originOffset
                : originOffset;
            var xOffset = new Vector3(direction, 0, 0);
            
            var fire = Instantiate(damageCollider, player.transform.position + xOffset, Quaternion.identity);
            yield return new WaitForSeconds(fireDuration);
            // fire will no longer damage when fire duration is over
            fire.GetComponent<Collider2D>().enabled = false;
            
            // destroy game object when all particles have disappeared
            var particles = fire.gameObject.GetComponent<ParticleSystem>().particleCount;
            yield return new WaitUntil(() => particles == 0);
            Destroy(fire);
            yield return null;
        }
    }
}
