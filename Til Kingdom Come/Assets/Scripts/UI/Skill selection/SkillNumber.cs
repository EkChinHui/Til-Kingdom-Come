using UnityEngine;

public class SkillNumber : MonoBehaviour
{
    public int skillNumber;
    public bool border1;
    public bool border2;

    public string SelectBorder()
    {
        return border1 ? "Border2" : "Border1";
    }
}
