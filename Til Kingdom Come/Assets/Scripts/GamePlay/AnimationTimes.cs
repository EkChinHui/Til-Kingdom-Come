using UnityEngine;

namespace GamePlay
{
    public class AnimationTimes : MonoBehaviour
    {
        public static AnimationTimes instance;

        public Animator anim;

        private float attackAnim;
        private float blockAnim; 
        private float rollAnim;
        private float forcePullAnim;
        private float forcePushAnim;
        private float throwKnivesAnim;

        public float AttackAnim => attackAnim;

        public float BlockAnim => blockAnim;

        public float RollAnim => rollAnim;

        public float ForcePullAnim => forcePullAnim;

        public float ForcePushAnim => forcePushAnim;

        public float ThrowKnivesAnim => throwKnivesAnim;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            } else if (instance != this)
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            UpdateAnimClipTimes();
        }

        // updates animation times
        private void UpdateAnimClipTimes()
        {
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

            foreach(AnimationClip clip in clips)
            {
                switch(clip.name)
                {
                    case "Attack":
                        instance.attackAnim = clip.length;
                        break;
                    case "Block":
                        instance.blockAnim = clip.length;
                        break;
                    case "Roll":
                        instance.rollAnim = clip.length;
                        break;
                    case "Force Pull":
                        instance.forcePullAnim = clip.length;
                        break;
                    case "Force Push":
                        instance.forcePushAnim = clip.length;
                        break;
                    case "ThrowKnives":
                        instance.throwKnivesAnim = clip.length;
                        break;
                }
            }
        }
    }
}
