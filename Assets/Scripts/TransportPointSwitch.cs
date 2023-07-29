using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPointSwitch : MonoBehaviour
{
    [SerializeField] private TransportationPoint transportPoint;
    [SerializeField] private TransportPointSwitch linkedTo;
    [SerializeField] private bool on;
    [SerializeField] private bool dontAllowManualTurningOff = true;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private SpriteRenderer renderer;

    public bool On { get => on; set { on = value; renderer.sprite = On ? onSprite : offSprite; } }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = On ? onSprite : offSprite;
    }

    public void Switch(bool firstToBeSwitched)
    {
        if (dontAllowManualTurningOff && On)
        {
            return;
        }

        On = !On;

        if(transportPoint.CurrentState == TransportationPoint.State.Moving)
        {
            transportPoint.Deactivate();
        }
        
        transportPoint.Activate();

        linkedTo.On = !on;
    }
}
