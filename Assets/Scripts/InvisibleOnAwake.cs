using UnityEngine;

public class InvisibleOnAwake : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

