using UnityEngine;

namespace GamePlay
{
    public class AnimationTimes : MonoBehaviour
    {
        public static AnimationTimes instance;

        public Animator anim;

        private float attackAnim;
        private float attack2Anim;
        private float attack3Anim;
        private float blockAnim; 
        private float rollAnim;
        private float confusionAnim;
        private float throwKnivesAnim;
        public float AttackAnim => attackAnim;
        public float Attack2Anim => attack2Anim;
        public float Attack3Anim => attack3Anim;
        public float BlockAnim => blockAnim;
        public float RollAnim => rollAnim;
        public float ConfusionAnim => confusionAnim;
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
                    case "Attack 2":
                        instance.attack2Anim = clip.length;
                        break;
                    case "Attack 3":
                        instance.attack3Anim = clip.length;
                        break;
                    case "Block":
                        instance.blockAnim = clip.length;
                        break;
                    case "Roll":
                        instance.rollAnim = clip.length;
                        break;
                    case "Confusion":
                        instance.confusionAnim = clip.length;
                        break;
                    case "ThrowKnives":
                        instance.throwKnivesAnim = clip.length;
                        break;
                }
            }
        }
    }
}
