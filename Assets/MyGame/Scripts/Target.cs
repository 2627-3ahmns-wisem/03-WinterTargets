using UnityEngine;

public class Target : MonoBehaviour
{
    public GameManager gameManager;

    //points for hitting this target (can be negative, e.g. ice block = -10)
    public int points = 10;

    //movement speed in units per second (local units of the parent object)
    public float moveSpeed = 150f;

    //how long the target stays before it disappears (seconds)
    public float lifeTime = 3f;

    //border of the playing area (same values as the random spawn position in the GameManager)
    public float maxX = 365f;
    public float maxY = 202f;

    //movement direction (length 1), chosen randomly at start
    private Vector2 direction;

    //makes sure a target is only counted once
    private bool wasHit = false;

    private void Start()
    {
        //create a reference to the GameManager script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //random direction: a random point on a circle with radius 1
        direction = Random.insideUnitCircle.normalized;

        //targets move now, so they stay a bit longer than the old 2 seconds
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        //move: direction * speed * time since last frame
        //Time.deltaTime makes the speed independent of the frame rate
        Vector2 position = transform.localPosition;
        position += direction * moveSpeed * Time.deltaTime;

        //bounce at the left / right border
        if (position.x > maxX || position.x < -maxX)
        {
            direction.x = -direction.x;
            position.x = Mathf.Clamp(position.x, -maxX, maxX);
        }

        //bounce at the top / bottom border
        if (position.y > maxY || position.y < -maxY)
        {
            direction.y = -direction.y;
            position.y = Mathf.Clamp(position.y, -maxY, maxY);
        }

        transform.localPosition = position;
    }

    private void OnMouseDown()
    {
        if (wasHit)
        {
            return;
        }
        wasHit = true;

        //pass the points of this target to the GameManager
        gameManager.IncrementScore(points);
        Destroy(gameObject);
    }
}
