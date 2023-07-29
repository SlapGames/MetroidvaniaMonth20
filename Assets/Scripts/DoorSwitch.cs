using UnityEngine;

public class DoorSwitch:MonoBehaviour
{
    public bool On { get => on; set { on = value; renderer.sprite = On ? onSprite : offSprite; } }
    
    [SerializeField] Door[] doors;
    [SerializeField] private bool on;
    [SerializeField] private bool dontAllowManualTurningOff = true;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private SpriteRenderer renderer;

    public void Switch()
    {
        if(dontAllowManualTurningOff && On)
        {
            return;
        }

        On = !On;

        foreach(Door door in doors)
        {
            if (On)
            {
                door.Open();
            }
            else
            {
                door.Close();
            }
        }
    }
}