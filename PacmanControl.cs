using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PacmanControl : MonoBehaviour {

    private float speed = 4;
    private Text textScore;
    private static Text textBestScore;
    private int score;
    private static int bestScore;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D rgb;
    private bool inNode = true;
    private Animator anim;
    private Button btn;

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Inputs();
        Stop();
        Move();
        EndGame();
    }

    // Is called when trigger collider hits something
    void OnTriggerEnter2D(Collider2D collision)
    {
        NewNode(collision, true);
        Points(collision);
        Stop();
        Death(collision);
    }

    // Is called when trigger collider exits something
    void OnTriggerExit2D(Collider2D collision)
    {
        NewNode(collision, false);
    }

    // If collided or exited a node then changes inNode
    void NewNode(Collider2D collision, bool newState)
    {
        if (collision != null && collision.tag == "node")
        {
            inNode = newState;
        }
    }

    // Sets initial values for several variables
    void Initialize()
    {
        Time.timeScale = 0;
        rgb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        textScore = GameObject.Find("Score").GetComponent<Text>();
        textBestScore = GameObject.Find("BestScore").GetComponent<Text>();
        btn = GameObject.Find("Start").GetComponent<Button>();
        btn.onClick.AddListener(StartGame);
    }

    // Ends the game after reaching max score by reseting scene and stopping time
    void EndGame()
    {
        if(score == 149)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 0;
        }
    }
    
    // Ends the game when pacman collides with an enemy by reseting scene and pausing time
    void Death(Collider2D collision)
    {
        if(collision != null && collision.tag == "enemy")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 0;
        }
    }

    // Resumes time if pacman died after pressing right or left
    void StartGame()
    {
        btn.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    // Increases score after colliding with a pellet
    void Points(Collider2D collision)
    {
        if(collision != null && collision.tag == "pellet")
        {
            Destroy(collision.gameObject);
            score++;
            UpdateUI();
        }
    }

    // Updates the values for the UI
    void UpdateUI()
    {
        textScore.text = score.ToString();
        if(score > bestScore)
        {
            bestScore = score;
            textBestScore.text = score.ToString();
        }
    }

    // Registers input from player
    void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
        }
    }

    // Moves pacman towards moveDirection if he can move there 
    void Move()
    {
        //allows pacman to change directions only in nodes
        if(moveDirection != Vector2.zero && canMove(moveDirection) && inNode)
        {
            Animate(moveDirection);
            rgb.velocity = moveDirection * speed;
            moveDirection = Vector2.zero;
        }
        //allows pacman to flip back and forth at any time
        else if(moveDirection != Vector2.zero && canMove(moveDirection) && moveDirection + rgb.velocity.normalized == Vector2.zero)
        {
            Animate(moveDirection);
            rgb.velocity = moveDirection * speed;
            moveDirection = Vector2.zero;
        }
    }

    // Changes animation when direction changes
    void Animate(Vector2 direction)
    {
        if(direction == Vector2.left)
        {
            anim.SetTrigger("Left");
        }
        if (direction == Vector2.right)
        {
            anim.SetTrigger("Right");
        }
        if (direction == Vector2.up)
        {
            anim.SetTrigger("Up");
        }
        if (direction == Vector2.down)
        {
            anim.SetTrigger("Down");
        }
    }

    // Stops movement if about to collide with something
    void Stop()
    {
        if (inNode && rgb!=null && rgb.velocity != Vector2.zero && !canMove(rgb.velocity.normalized))
        {
            rgb.velocity = Vector2.zero;
        }
    }

    // Checks if there is an obstace in direction
    bool canMove(Vector2 direction)
    {
        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + direction, (float).1);
        if(hit != null && hit.tag == "tile")
        {
            return false;
        }
        return true;
    }
}
