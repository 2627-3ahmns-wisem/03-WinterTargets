using UnityEngine;

public class Target : MonoBehaviour
{
    public GameManager gameManager;

    //points for hitting this target (can be negative, e.g. ice block = -10)
    public int points = 10;

    //makes sure a target is only counted once
    private bool wasHit = false;

    private void Start()
    {
        //create a reference to the GameManager script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Destroy(gameObject, 2f);
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
