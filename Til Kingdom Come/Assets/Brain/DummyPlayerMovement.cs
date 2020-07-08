using UnityEngine;

public class DummyPlayerMovement : MonoBehaviour
{
    public Transform AIposition;
    public Transform dummyPosition;
    public Rigidbody2D rb;
    public float speed;
    public bool direction; // true goes right false goes left
    private float turnThreshold = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
        if (direction)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
    }

    private void Turn()
    {
        var probTurn = Random.Range(0f, 1f);
        if (probTurn < turnThreshold)
        {
            direction = dummyPosition.position.x < AIposition.transform.position.x;
        }
    }
}
