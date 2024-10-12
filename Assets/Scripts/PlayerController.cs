using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float moveSpeed = 3.4f;
    private float inputMovement;
    public float jumpForce = 5f;
    public int diamondCount = 0;
    public AudioClip coinPickup;
    public AudioSource playerSpeaker;
    private bool autoWalk = false;
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;
    public TextMeshProUGUI uiDiamondCount;
    public TextMeshProUGUI ui_Distance;
    public GameObject start;
    public Canvas pauseMenu;
    public Canvas mainCanvas;
    public int distanceTraveled = 0;
    public Canvas endGameCanvas;
    private endScreen deathLogic;
    private bool isGrounded;
    private bool isPaused = true;
    public int heartCount = 3;
    public GameObject heartPrefab;
    private List<GameObject> hearts = new List<GameObject>();
    highScoreHelper hsh;
    private float timer = 0;
    private float startTime = 0;
    public TextMeshProUGUI highscoreSplash;
    public bool invincible = false;
    public TextMeshProUGUI powerupText;
    public GameObject activeJetpack;
    public GameObject inactiveJetpack;
    void Start()
    {
        hsh = new highScoreHelper();
        deathLogic = endGameCanvas.GetComponent<endScreen>();
        playerRb = GetComponent<Rigidbody2D>();
        uiDiamondCount.text = "X " + diamondCount.ToString();
        pauseMenu.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(unpause);
        pauseMenu.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(mainMenu);
        pauseMenu.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(quitGame);
        handleHealth();
    }

    void Update()
    {
        timer += Time.deltaTime;

        movement();

        distanceTraveled = Mathf.RoundToInt(Mathf.Abs(transform.position.x - start.transform.position.x));
        ui_Distance.text = "Distance: " + distanceTraveled.ToString();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                unpause();
            }
            else
            {
                pauseGame();
            }
        }

        if(invincible)
        {
            powerupText.text = "Super charge: " + Mathf.RoundToInt(((startTime + 18) - timer));
        }
        if(invincible && (startTime + 18) <= timer)
        {
            invincible = false;
            jumpForce /= 1.5f;
            moveSpeed /= 1.5f;
            powerupText.enabled = false;
        }

    }

    private void movement()
    {
        isGrounded = false;
        isGrounded = Physics2D.CircleCast(groundCheck.position, groundDistance, Vector2.down, 0.1f, groundMask);
        if(playerRb.velocity.y > 0.5)
        {
            inactiveJetpack.GetComponent<SpriteRenderer>().enabled = false;
            activeJetpack.GetComponent<SpriteRenderer>().enabled = true;

        }
        else
        {
            activeJetpack.GetComponent<SpriteRenderer>().enabled = false;
            inactiveJetpack.GetComponent<SpriteRenderer>().enabled = true;
        }
        float movement = Input.GetAxisRaw("Horizontal");
        if (movement > 0 && !autoWalk)
        {
            playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);
        }
        else if(autoWalk)
        {
            playerRb.velocity = new Vector2(1 * moveSpeed, playerRb.velocity.y);
        }
        else if(movement == 0)
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.velocity = Vector2.up * jumpForce;
            playerRb.velocity = new Vector2((playerRb.velocity.x / 2), playerRb.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            autoWalk = !autoWalk;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        timer += Time.deltaTime;
        if(other.gameObject.tag == "diamond")
        {
            playerSpeaker.PlayOneShot(coinPickup);
            diamondCount++;
            Destroy(other.gameObject);
            uiDiamondCount.text = "x " + diamondCount.ToString();
        }
        
        if(other.gameObject.tag == "lava" || other.gameObject.tag == "bouncyLava")
        {
            if(!invincible)
            {
                endGame();
            }
            
        }

        if(other.gameObject.tag == "droppingRock" && !invincible)
        {
            Destroy(other.gameObject);
            endGame();
        }

        if(other.gameObject.tag == "life")
        {
            Destroy(other.gameObject);
            lifePickup();
        }
        if(other.gameObject.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            powerUpInit();
        }

    }


    private void pauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseMenu.enabled = true;
        mainCanvas.enabled = false;
    }

    private void unpause()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.enabled = false;
        mainCanvas.enabled = true;

    }

    private void quitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    private void mainMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene("mainMenu");
    }

    private void endGame()
    {
        if(hearts.Count == 1)
        {
            hsh.saveScores((distanceTraveled * diamondCount));
            Destroy(hearts[hearts.Count-1]);
            hearts.RemoveAt(hearts.Count - 1);
            List<int> scores = hsh.getScores();
            if(scores.Count >= 5)
            {
                foreach(int score in scores)
                {
                    if((distanceTraveled * diamondCount) > score)
                    {
                        highscoreSplash.text = "NEW HIGH SCORE";
                    }
                }
            }
            else
            {
                highscoreSplash.text = "NEW HIGH SCORE";
            }
            Time.timeScale = 0;
            mainCanvas.enabled = false;
            endGameCanvas.enabled = true;
            deathLogic.onDeath();
        }
        else
        {
            Destroy(hearts[hearts.Count-1]);
            hearts.RemoveAt(hearts.Count - 1);
        }
    }

    private void handleHealth()
    {
        for(int i = 0; i < heartCount; i++)
        {
            GameObject temp = Instantiate(heartPrefab);
            temp.transform.SetParent(mainCanvas.transform, false);
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200 + (75 * i), -50);
            hearts.Add(temp);
        }
    }

    private void lifePickup()
    {
        if(hearts.Count < heartCount)
        {
            float lastHeartPos = hearts[hearts.Count - 1].transform.localPosition.x;
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(mainCanvas.transform, false);
            newHeart.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200 + (75 * hearts.Count), -50);
            hearts.Add(newHeart);
        }
        else
        {
            diamondCount += 10;
            uiDiamondCount.text = "x " + diamondCount.ToString();
        }
    }

    private void powerUpInit()
    {
        startTime = timer;
        if(!invincible)
        {
            powerupText.enabled = true;
            invincible = true;
            jumpForce *= 1.5f;
            moveSpeed *= 1.5f;
        }
    }

}
