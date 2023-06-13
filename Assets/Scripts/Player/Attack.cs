using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int Damage { get; private set; }
    public string AnimationName { get; set; }
    public Attack NextInCombo { get; set; }

    private Rigidbody2D body;

    private void Start()
    {
        
    }

    public void TriggerAttack()
    {

    }
}
