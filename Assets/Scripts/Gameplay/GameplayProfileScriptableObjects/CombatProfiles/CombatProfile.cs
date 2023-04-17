using MushiCore;
using MushiCore.EditorAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplayProfiles/Combat/Combat Profile", fileName = "CombatProfile")]
public class CombatProfile : DescriptionBaseSO
{
    [ColorHeader("Combat Profile")]
    public List<AttackSO> combo;
}