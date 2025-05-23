using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
   public static SkillManager instance;

    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }   
    public DoubleJump doubleJump { get; private set; }
    public Roll roll { get; private set; }
    public SwordSkill sword { get; private set; }
 
    private void Awake()
    {
     
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance=this;
    }
    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        doubleJump= GetComponent<DoubleJump>();
        roll = GetComponent<Roll>();
        sword = GetComponent<SwordSkill>();
    }
}
