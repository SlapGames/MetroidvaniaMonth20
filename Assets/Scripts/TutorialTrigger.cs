using UnityEngine;

public class TutorialTrigger:MonoBehaviour
{
    [SerializeField, TextArea(5, 10)] private string tutorialMessage;
    [SerializeField] private TutorialMarker tutorialMarker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            tutorialMarker.gameObject.SetActive(true);
            tutorialMarker.Message = tutorialMessage;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player" && tutorialMarker.Message == tutorialMessage)
        {
            tutorialMarker.gameObject.SetActive(false);
        }
    }
}