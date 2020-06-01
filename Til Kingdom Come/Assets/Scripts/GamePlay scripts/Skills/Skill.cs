using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected string skillName;
    [SerializeField] protected string skillDescription;
    [SerializeField] protected float skillCooldown;

    public string SkillName { get { return skillName; } }

    public abstract void Cast(PlayerController player, PlayerController opponent);

}
