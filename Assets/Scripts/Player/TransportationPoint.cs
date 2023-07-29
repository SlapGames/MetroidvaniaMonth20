using UnityEngine;

public class TransportationPoint : MonoBehaviour
{
    public enum State
    {
        Idle,
        Moving,
        Pause
    }

    public State CurrentState { get => state; private set => state = value; }

    [SerializeField] private Transform[] toMoveBetween;
    [SerializeField] private float speed;

    //The initial direction and index should be compatible. IE: Don't set the target index to 0 when the direction is forward; index 0 can only ever be backwards from other indices.
    [SerializeField] private bool movingForward = true;
    [SerializeField] private int currentlyAtIndex = 0;
    [SerializeField] private int targetPointIndex = 0;
    [SerializeField] private State state = State.Idle;
    [SerializeField] private float closeEnoughDistance = .1f;
    [SerializeField] private TransportPointSwitch transportPointSwitchStart;
    [SerializeField] private TransportPointSwitch transportPointSwitchEnd;

    private Transform playerTransform;
    private Player player;
    private Vector3 previousPosition;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (state == State.Idle && collision.tag == "Player" && collision.GetComponent<Player>().ActiveState is HangState)
        {
            playerTransform = collision.transform;
            player = collision.GetComponent<Player>();

            transportPointSwitchStart.On = !transportPointSwitchStart.On;
            transportPointSwitchEnd.On = !transportPointSwitchEnd.On;

            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == State.Pause && collision.tag == "Player")
        {
            state = State.Idle;
        }
    }

    //The Pause state is meant to keep the transport point from moving between the locations continuously if the player never lets go.
    //But, the TransportPointSwitch class Activates and Deactivates the transport point, leading to the state being set to Pause
    //even though the player had no chance to exit the collider, and thus reset the point. So, to account for that, the pause state
    //gets changed to Idle when the player either exits or enters the collider.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Pause && collision.tag == "Player")
        {
            state = State.Idle;
        }
    }

    private int GetNextTargetPoint()
    {
        int indexIncrement = movingForward ? 1 : -1;
        int temp = currentlyAtIndex + indexIncrement;

        return temp;
    }

    public void Activate()
    {
        state = State.Moving;
        targetPointIndex = GetNextTargetPoint();
    }
    public void Deactivate()
    {
        currentlyAtIndex = targetPointIndex;
        state = State.Pause;
        movingForward = !movingForward;

        player = null;
        playerTransform = null;
    }

    private void Update()
    {
        if (state == State.Moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, toMoveBetween[targetPointIndex].position, speed * Time.deltaTime);

            if (player != null && playerTransform != null)
            {
                if (player.ActiveState is HangState)
                {
                    //The hang state, as of 7/29/23, does halt the player's gravity, so that isn't necessary here.
                    playerTransform.position += transform.position - previousPosition;
                }
                else
                {
                    player = null;
                    playerTransform = null;
                }
            }

            if (Vector2.Distance(transform.position, toMoveBetween[targetPointIndex].position) <= closeEnoughDistance)
            {
                transform.position = toMoveBetween[targetPointIndex].position;
                currentlyAtIndex = targetPointIndex;
                int tempIndex = GetNextTargetPoint();

                if (tempIndex < 0 || tempIndex >= toMoveBetween.Length)
                {
                    //The last point has been reached, stop movement and reverse the direction
                    Deactivate();
                }
                else
                {
                    targetPointIndex = tempIndex;
                }
            }
        }

        previousPosition = transform.position;
    }
}