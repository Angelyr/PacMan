using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    private int speed = 4;
    private Vector2 moveDirection;
    private bool inNode;
    private Rigidbody2D rgb;
    private Animator anim;

	// Use this for initialization
	void Start ()
    {
        Initialize();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Stop();
    }
    
    // Is called when physics updates
    void FixedUpdate()
    {
        Move();
        MoveAgain();
    }

    // Moves location if it gets stuck
    void MoveAgain()
    {
        if(rgb.velocity == Vector2.zero)
        {
            rgb.transform.position = new Vector2(Mathf.RoundToInt(rgb.transform.position.x), Mathf.RoundToInt(rgb.transform.position.y));
            ChooseDirection();
        }
    }

    // Is called when trigger collider hits something
    void OnTriggerEnter2D(Collider2D collision)
    {
        NewNode(collision, true);
        ChooseDirection();
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
        rgb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Stops movement if about to collide with something
    void Stop()
    {
        if (inNode && rgb.velocity != Vector2.zero && !canMove(rgb.velocity.normalized))
        {
            rgb.velocity = Vector2.zero;
        }
        //Collider2D hit = Physics2D.
    }

    // Chooses random direction for movement
    void ChooseDirection()
    {
        ArrayList directions = new ArrayList();
        if (canMove(Vector2.left))
        {
            directions.Add(Vector2.left);
        }
        if (canMove(Vector2.right))
        {
            directions.Add(Vector2.right);
        }
        if (canMove(Vector2.up))
        {
            directions.Add(Vector2.up);
        }
        if (canMove(Vector2.down))
        {
            directions.Add(Vector2.down);
        }
        if(directions.Count != 0)
        {
            int rand = (int)Random.Range(0, directions.Count);
            moveDirection = (Vector2)directions[rand];
        }
    }

    // Changes animation when direction changes
    void Animate(Vector2 direction)
    {
        if (direction == Vector2.left)
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

    // Moves enemy towards moveDirection if he can move there 
    void Move()
    {
        if (moveDirection != Vector2.zero && canMove(moveDirection) && inNode)
        {
            Animate(moveDirection);
            rgb.velocity = moveDirection * speed;
            moveDirection = Vector2.zero;
        }
    }

    // Checks if there is an obstace in direction
    bool canMove(Vector2 direction)
    {
        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + direction/2, (float).1);
        if (hit != null && (hit.tag == "tile" || hit.tag == "enemy"))
        {
            return false;
        }
        // prevents switching directions back and forth
        if (moveDirection != Vector2.zero && direction + rgb.velocity.normalized == Vector2.zero)
        {
            return false;
        }
        return true;
    }
}