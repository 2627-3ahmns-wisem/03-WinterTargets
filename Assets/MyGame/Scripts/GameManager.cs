using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //different target types (snowman, present, ice block), one is chosen randomly
    public GameObject[] targets;
    public GameObject parentOfTargets;
    public GameObject winSprite;
    public Text scoreText;

    //score needed to win
    public int winScore = 100;

    private int score = 0;
    private bool win = false;

    private void Start()
    {
        InvokeRepeating("Spawn",1f,1f);
    }

    private void Update()
    {
        //if the player reaches winScore, the player has won and the Spawn method will not called further. 
        if(win)
        {
            CancelInvoke("Spawn");
        }

        //every shoot (clicking left mouse button) produce a shooting sound.
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    //Spawn a target at a random position within a specified x and y range.
    //Instantiate (make a concrete GameObject, i.e., a clone from the given prefeb target) the target as a child
    //of the GameObject ParentTargets. In this case transform.localPosition instead of transform.position is important!!
    private void Spawn()
    {
        float randomX = Random.Range(-365f, 365f);
        float randomY = Random.Range(-202f,202f);

        Vector2 random2DPosition = new Vector2(randomX, randomY);

        //choose a random target type
        GameObject randomTarget = targets[Random.Range(0, targets.Length)];

        GameObject newTarget = Instantiate(randomTarget, parentOfTargets.transform);
        newTarget.transform.localPosition = random2DPosition;
    }

    //is called in the class Target with the points of the hit target (can be negative)
    public void IncrementScore(int points)
    {
        //after winning the score does not change anymore
        if (win)
        {
            return;
        }

        score += points;
        print(score);

        scoreText.text = score.ToString();

        if (score >= winScore)
        {
            win = true;
            winSprite.SetActive(true);
        }
    }
}
