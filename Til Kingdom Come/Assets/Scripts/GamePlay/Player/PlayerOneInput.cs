using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerOneInput : PlayerKeyInput
    {
        public override KeyCode GetLeftKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Left", "A"));
        }
        public override KeyCode GetRightKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Right", "D"));
        }
        public override KeyCode GetRollKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Roll", "S"));
        }
        public override KeyCode GetAttackKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Attack", "F"));
        }
        public override KeyCode GetBlockKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Block", "G"));
        }
        public override KeyCode GetSkillKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P1Skill", "H"));
        }
    }
}