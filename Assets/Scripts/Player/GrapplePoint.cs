using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    public bool Active { get; set; }
    public bool Valid { get; set; }
    public bool Default { get => @default; private set => @default = value; }

    [SerializeField] private GameObject marker;
    [SerializeField] private bool @default;
    [SerializeField] private Color regular;
    [SerializeField] private Color activeColor;

    private void Update()
    {
        if (marker != null)
        {
            marker.SetActive(Valid);
        }

        if(Active)
        {
            GetComponent<SpriteRenderer>().color = activeColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = regular;
        }
    }
}
