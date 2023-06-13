using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public PsychicPower ActivePsychicPower { get; private set; }
    public Attack LastAttack { get; private set; }
    public Hit LastHit { get; set; }

    [SerializeField] private Attack defaultAttack;

    public void ProcessLastHit()
    {

    }

    public Attack GetNextAttack()
    {
        return defaultAttack;
    }

    public void ProcessNextAttack()
    {

    }
}
