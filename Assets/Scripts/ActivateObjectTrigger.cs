using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectTrigger : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            objectToActivate.SetActive(true);
        }
    }
}
