using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloneSkill : Skill
{
    [SerializeField] public float cloneDuration;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private bool _canAttack;

     public void CreateClone(Transform _clonePosition)
    {
        GameObject clone = Instantiate(clonePrefab);
        clone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, cloneDuration,_canAttack);
    }
}
