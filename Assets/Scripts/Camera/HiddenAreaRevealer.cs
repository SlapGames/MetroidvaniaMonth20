using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenAreaRevealer:MonoBehaviour
{
    [SerializeField] private Tilemap toReveal;
    [SerializeField] private float alphaLossPerSecond;

    private bool revealing;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            revealing = true;
        }
    }

    private void Update()
    {
        if (revealing)
        {
            Color newColor = toReveal.color;
            newColor.a -= alphaLossPerSecond * Time.deltaTime;
            toReveal.color = newColor;

            if(newColor.a <= 0)
            {
                revealing = false;
            }
        }
    }
}