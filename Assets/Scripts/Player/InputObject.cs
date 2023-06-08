using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputObject
{
    public const float defaultBufferTime = .5f;

    public InputType InputType { get; private set; }
    public bool HasBeenRead { get; private set; }
    public float ExpiresOn { get; private set; }

    public InputType DEBUG_inputType;
    public float DEBUG_expiresOn;

    public InputObject(InputType inputType, float bufferTime = defaultBufferTime)
    {
        InputType = inputType;
        ExpiresOn = Time.time + bufferTime;


        DEBUG_inputType = inputType;
        DEBUG_expiresOn = ExpiresOn;
    }

    public void MarkAsRead()
    {
        HasBeenRead = true;
    }
}
