using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float health;
    public float speed;
    public float maxHealth;
    public float maxSpeed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GameObject().GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey)
        {
            foreach (char Key in Input.inputString)
            {
                switch(Key)
                {
                    case 'w':
                        rb.velocity = new Vector2(0, speed);
                        rb.velocity = Vector3.zero;
                        break;
                    case 'a':
                        rb.velocity = new Vector2(-speed, 0);
                        rb.velocity = Vector3.zero;
                        break;
                    case 's':
                        rb.velocity = new Vector2(0, -speed);
                        rb.velocity = Vector3.zero;
                        break;
                    case 'd':
                        rb.velocity = new Vector2(speed, 0);
                        rb.velocity = Vector3.zero;
                        break;
                }
            }
        }
    }
}
