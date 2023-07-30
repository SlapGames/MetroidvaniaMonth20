using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    [SerializeField] bool closed;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (closed)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        animator.Play("Base Layer.Open");
        closed = false;
        GetComponent<Collider2D>().enabled = false;
    }
    public void Close()
    {
        animator.Play("Base Layer.Close");
        closed = true;
        GetComponent<Collider2D>().enabled = true;
    }

    public void Toggle()
    {
        if (!closed)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
