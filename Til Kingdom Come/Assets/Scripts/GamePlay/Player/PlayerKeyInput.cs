using UnityEngine;

namespace GamePlay.Player
{
    public abstract class PlayerKeyInput : MonoBehaviour
    {
        public abstract KeyCode GetLeftKey();
        public abstract KeyCode GetRightKey();
        public abstract KeyCode GetRollKey();
        public abstract KeyCode GetAttackKey();
        public abstract KeyCode GetBlockKey();
        public abstract KeyCode GetSkillKey();

    }
}