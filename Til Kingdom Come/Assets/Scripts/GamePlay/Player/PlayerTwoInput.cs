using UnityEngine;

namespace GamePlay.Player
{
    public class PlayerTwoInput : PlayerKeyInput
    {
        public override KeyCode GetLeftKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Left", "LeftArrow"));
        }
        public override KeyCode GetRightKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Right", "RightArrow"));
        }
        public override KeyCode GetRollKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Roll", "DownArrow"));
        }
        public override KeyCode GetAttackKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Attack", "Slash"));
        }
        public override KeyCode GetBlockKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Block", "Period"));
        }
        public override KeyCode GetSkillKey()
        {
            return (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("P2Skill", "Comma"));
        }
    }
}
