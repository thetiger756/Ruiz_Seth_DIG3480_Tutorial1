using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public Text countText;
    public Text scoreText;
    public Text livesText;
    public Text winText;
    public GameObject player;

    private Rigidbody rb;
    private int count;
    private int score;
    private int lives;
    private string nextLevel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        score = 0;
        lives = 3;
        SetCountText();
        SetScoreText();
        SetLivesText();
        winText.text = "";
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            score++;
            SetScoreText();
        }
        if(other.gameObject.CompareTag("Bad Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            //TIER 3 - STEP 2
            //Here we take out the functionality from TIER 2
            //so that our game can use the lives system
            //there are many ways to do this, but I believe
            //this is the least problematic
            //score++;
            //SetScoreText();

            lives--;
            SetLivesText();
        }
    }

    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    private void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();

        if (score >= 12)
        {
            winText.text = "You Win!";

            //Get information about the current level (scene)
            Scene m_Scene = SceneManager.GetActiveScene();

            //Store level's (scene's) name in a variable
            string sceneName = m_Scene.name;

            //If the player is on the first level, go to the second level
            //This prevents the game from restarting the second level upon completion
            if (sceneName == "MiniGame Level 1")
            {
                GoToSecondLevel();
            }
        }
    }

    private void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();

        if(lives == 0)
        {
            //Destorying the player throws errors, so it's better to Deactivate
            player.SetActive(false);

            //We'll reuse the win text because it's got the right size/location
            winText.text = "YOU DIED";
        }
    }

    private void GoToSecondLevel()
    {
        SceneManager.LoadScene("MiniGame Level 2", LoadSceneMode.Single);
    }
}
