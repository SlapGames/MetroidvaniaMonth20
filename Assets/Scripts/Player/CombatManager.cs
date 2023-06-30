using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [System.Serializable]
    public class NameToAttack
    {
        public string name;
        public Attack attack;
    }

    public int HP { get; set; }
    public PsychicPower ActivePsychicPower { get; private set; }
    public Attack LastAttack { get; private set; }
    public Hit LastHit { get => lastHit; set => lastHit = value; }
    public string AttackKey { get => attackKey; set => attackKey = value; }

    //There must be at least one entry with the name "Default"
    [SerializeField] private NameToAttack[] attacks;
    [SerializeField] private int hpMax;
    private Hit lastHit = null;
    private string attackKey = "Default";

    [Space(15)]
    [Header("Debug")]
    public Hit DEBUG_LastHit;
    public bool DEBUG_LastHitIsNull;
    public float DEBUG_DebugKnockBack;
    public float DEBUG_DebugDirection;

    private void Start()
    {
        HP = hpMax;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            LastHit = new Hit(0, Vector2.zero, Vector2.zero, DEBUG_DebugDirection, DEBUG_DebugKnockBack);
        }

        DEBUG_LastHit = lastHit;
        DEBUG_LastHitIsNull = lastHit == null;
    }

    public void ProcessLastHit()
    {
        //TODO: finish implementation

        LastHit = null;
    }

    public Attack GetCurrentAttack()
    {
        return GetByName(attackKey).attack;
    }

    private NameToAttack GetByName(string name)
    {
        foreach(var att in attacks)
        {
            if(att.name == name)
            {
                return att;
            }
        }

        return null;
    }

    public void DisregardLastHit()
    {
        LastHit = null;
    }
}
