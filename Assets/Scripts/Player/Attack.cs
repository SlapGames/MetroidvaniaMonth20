using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage { get => damage; private set => damage = value; }
    public string Name { get => name; private set => name = value; }
    public Attack NextInCombo { get => nextInCombo; private set => nextInCombo = value; }
    public float KnockBack { get => knockBack; set => knockBack = value; }
    public float StepForwardValue { get => stepForwardValue; set => stepForwardValue = value; }

    [SerializeField] private Rigidbody2D body;
    [SerializeField] private LayerMask canAttackMask;
    private ContactFilter2D contactFilter;

    //Make sure not to damage the one making the attack.
    private GameObject attacker;

    //capacity of 16 is arbitrary. It seems like it would always be more than enough to me.
    private RaycastHit2D[] results = new RaycastHit2D[16];
    [SerializeField] private string name;
    [SerializeField] private Attack nextInCombo;
    [SerializeField] private int damage;
    [SerializeField] private float knockBack;
    [SerializeField] private float stepForwardValue;

    public void Initialize(GameObject attacker)
    {
        contactFilter = new ContactFilter2D();

        body = GetComponent<Rigidbody2D>();

        contactFilter.layerMask = canAttackMask;
        contactFilter.useLayerMask = true;
        contactFilter.useTriggers = true;

        this.attacker = attacker;
    }

    public void TriggerAttack()
    {
        int howManyResults = body.Cast(Vector2.zero, contactFilter, results);
        for (int i = 0; i < howManyResults; i++)
        {
            //Debug.Log("Detected: ", results[i].collider.gameObject);

            CombatManager currentCombatManager = results[i].collider.GetComponent<CombatManager>();
            if (currentCombatManager == null || currentCombatManager.gameObject == attacker)
            {
                continue;
            }

            //Debug.Log("Combat Manager: ", results[i].collider.gameObject);
            Hit newHit = new Hit(Damage, transform.position, currentCombatManager.transform.position, Mathf.Sign(currentCombatManager.transform.position.x - transform.position.x), KnockBack);
            currentCombatManager.LastHit = newHit;
        }
    }
}
