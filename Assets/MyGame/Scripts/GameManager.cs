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

    //UI texts for the countdown and the result at the end of a round
    public Text timeText;
    public Text resultText;

    //length of one round in seconds
    public float roundTime = 60f;

    //score needed to "win" a round. The round does NOT end early anymore:
    //it always runs the full time, at the end it is checked whether winScore was reached.
    public int winScore = 100;

    private int score = 0;
    private float timeLeft;

    //game state: true while a round is running, false after the time is up
    private bool isRunning = false;

    private void Start()
    {
        //check that the probabilities add up to 100 %
        if (snowmanChance + iceBlockChance + presentChance != 100)
        {
            Debug.LogWarning("Spawn probabilities do not add up to 100 %!");
        }

        StartRound();
    }

    private void Update()
    {
        if (isRunning)
        {
            //count down the time (Time.deltaTime = seconds since the last frame)
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                EndRound();
            }

            UpdateTimeText();

            //every shoot (clicking left mouse button) produce a shooting sound.
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            //after the round: press R to start a new round
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartRound();
            }
        }
    }

    //starts a new round: reset score and time, remove old targets, start spawning
    private void StartRound()
    {
        score = 0;
        timeLeft = roundTime;
        isRunning = true;

        scoreText.text = score.ToString();
        UpdateTimeText();
        resultText.text = "";
        winSprite.SetActive(false);

        //remove targets that are still left from the last round
        foreach (Transform child in parentOfTargets.transform)
        {
            Destroy(child.gameObject);
        }

        InvokeRepeating("Spawn", 1f, 1f);
    }

    //ends the round: stop spawning and show the result
    private void EndRound()
    {
        isRunning = false;
        CancelInvoke("Spawn");

        //remove the targets that are still on the screen
        foreach (Transform child in parentOfTargets.transform)
        {
            Destroy(child.gameObject);
        }

        if (score >= winScore)
        {
            winSprite.SetActive(true);
            resultText.text = "Zeit abgelaufen! " + score + " Punkte - gewonnen!\nDrücke R für eine neue Runde";
        }
        else
        {
            resultText.text = "Zeit abgelaufen! " + score + " Punkte\nDrücke R für eine neue Runde";
        }
    }

    //shows the remaining time in whole seconds (rounded up, so 0.3 s still shows 1)
    private void UpdateTimeText()
    {
        timeText.text = "Zeit: " + Mathf.CeilToInt(timeLeft);
    }

    //is used by the Target class to check if hits still count
    public bool IsRunning()
    {
        return isRunning;
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
        //after the round has ended the score does not change anymore
        if (!isRunning)
        {
            return;
        }

        score += points;
        print(score);

        scoreText.text = score.ToString();
    }
}
