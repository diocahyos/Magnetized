using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float moveSpeed = 5f;
    private Vector3 startPosition;

    // For Tower
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    private GameObject closestTower;
    private GameObject hookedTower;
    private bool isPulled = false;

    private UIControllerScript uiControl;
    //Audio
    private AudioSource myAudio;
    private bool isCrashed = false;

    public Tower Tower1;
    public Tower Tower2;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        startPosition = rb2D.transform.position;
        myAudio = this.gameObject.GetComponent<AudioSource>();
        uiControl = GameObject.Find("Canvas").GetComponent<UIControllerScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move the object
        rb2D.velocity = -transform.up * moveSpeed;

        if (isCrashed)
        {
            if(!myAudio.isPlaying)
            {
                //Restart scene
                restartPosition();
            }           
        }
        else
        {
            //Move the object
            rb2D.velocity = -transform.up * moveSpeed;
        }
    }

    public void restartPosition()
    {
        //Set to Start position
        this.transform.position = startPosition;

        //Restart rotation
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        //Set isCrashed to flase
        isCrashed = false;

        if (closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = Color.white;
            closestTower = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            if (!isCrashed)
            {
                //Play SFX
                myAudio.Play();
                rb2D.velocity = new Vector3(0f, 0f, 0f);
                rb2D.angularVelocity = 0f;
                isCrashed = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            Debug.Log("Levelclear!");
            uiControl.endGame();
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tower")
        {   
            closestTower = collision.gameObject;

            Tower coliderTower = collision.gameObject.GetComponent<Tower>();
            if (coliderTower != null)
            {
                
                if (coliderTower.gameObject.name == "Tower 1")
                {
                    
                    Tower1.SetPlayer(this);
                }
                else if (coliderTower.gameObject.name == "Tower 2")
                {
                    
                    Tower2.SetPlayer(this);
                }
            }
            
            //Change tower color black to green as indicator
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isPulled) return;

        if(collision.gameObject.tag == "Tower")
        {
            closestTower = null;
            hookedTower = null;
            //Change tower color back to normal
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        }
    }

    public void Pull()
    {
        if (!isPulled)
        {
            if (closestTower != null && hookedTower == null)
            {
                hookedTower = closestTower;
            }
            if (hookedTower)
            {
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);

                //Gravitasi toward tower
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;
                float newPullForce = Mathf.Clamp(pullForce / distance, 20, 50);
                rb2D.AddForce(pullDirection * newPullForce);

                //Angular velocity
                rb2D.angularVelocity = -rotateSpeed / distance;
                isPulled = true;
            }
        }
    }

    public void Release()
    {
            rb2D.angularVelocity = 0;
            isPulled = false;
            hookedTower = null;
    }

}
