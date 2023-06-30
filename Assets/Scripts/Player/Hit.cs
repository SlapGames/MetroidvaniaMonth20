using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hit
{
    [SerializeField] private int attackDamage;
    [SerializeField] private Vector2 attackOrigin;
    [SerializeField] private Vector2 hitOrigin;
    [SerializeField] private float direction;
    [SerializeField] private float knockBack;

    public int AttackDamage { get => attackDamage; private set => attackDamage = value; }
    public Vector2 AttackOrigin { get => attackOrigin; private set => attackOrigin = value; }
    public Vector2 HitOrigin { get => hitOrigin; private set => hitOrigin = value; }
    public float KnockBack { get => knockBack; set => knockBack = value; }
    
    /// <summary>
    /// 1 or -1, depending on if the attacker was to the right or left (respectively) of the attackee.
    /// </summary>
    public float Direction { get => direction; private set => direction = value; }

    public Hit(int attackDamage, Vector2 attackOrigin, Vector2 hitOrigin, float direction, float knockback)
    {
        AttackDamage = attackDamage;
        AttackOrigin = attackOrigin;
        HitOrigin = hitOrigin;
        Direction = direction;
        KnockBack = knockback;
    }
}
