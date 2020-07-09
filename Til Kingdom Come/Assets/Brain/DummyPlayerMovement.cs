using UnityEngine;

public class DummyPlayerMovement : MonoBehaviour
{
    public Transform AIposition;
    public Transform dummyPosition;
    public Rigidbody2D rb;
    public float speed;
    public bool direction; // true goes right false goes left
    //private float turnThreshold = 0.02f;
    private float leftBound;
    private float rightBound;

    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        leftBound = AIposition.position.x - 16f;
        rightBound = AIposition.position.x + 21f;
        direction = Random.Range(0f, 1f) > 0.5;
    }

    // Update is called once per frame
    void Update()
    {
        // Turn();
        if (rb.velocity.x < 0 && dummyPosition.position.x < leftBound) // moving left  and cross left bound
        {
            direction = !direction;
        }
        else if (rb.velocity.x > 0 && dummyPosition.position.x > rightBound) // moving right and cross right bound
        {
            direction = !direction;
        }
        if (direction)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
    }

    /*private void Turn()
    {
        var probTurn = Random.Range(0f, 1f);
        if (probTurn < turnThreshold)
        {
            direction = dummyPosition.position.x < AIposition.transform.position.x;
        }
    }*/
}
