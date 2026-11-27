using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //the three target types (prefabs), set in the Inspector
    public GameObject snowmanPrefab;
    public GameObject iceBlockPrefab;
    public GameObject presentPrefab;

    //spawn probabilities in percent (must add up to 100)
    public int snowmanChance = 60;   // 60 % -> common
    public int iceBlockChance = 30;  // 30 % -> less common
    public int presentChance = 10;   // 10 % -> rare
    public GameObject parentOfTargets;
    public GameObject winSprite;
    public Text scoreText;

    //score needed to win
    public int winScore = 100;

    private int score = 0;
    private bool win = false;

    private void Start()
    {
        //check that the probabilities add up to 100 %
        if (snowmanChance + iceBlockChance + presentChance != 100)
        {
            Debug.LogWarning("Spawn probabilities do not add up to 100 %!");
        }

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

        GameObject newTarget = Instantiate(ChooseTargetType(), parentOfTargets.transform);
        newTarget.transform.localPosition = random2DPosition;
    }

    //chooses a target type by chance: a random number from 0 to 99 falls into one of three ranges
    //  0 - 59 -> snowman   (60 %)
    // 60 - 89 -> ice block (30 %)
    // 90 - 99 -> present   (10 %)
    private GameObject ChooseTargetType()
    {
        int randomValue = Random.Range(0, 100); // 0 to 99, 100 is excluded

        if (randomValue < snowmanChance)
        {
            return snowmanPrefab;
        }
        else if (randomValue < snowmanChance + iceBlockChance)
        {
            return iceBlockPrefab;
        }
        else
        {
            return presentPrefab;
        }
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
