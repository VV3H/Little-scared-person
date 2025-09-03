using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float health;
    public float speed = 10;
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

        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");

        if (H != 0 && V != 0)
        {
            transform.position += new Vector3(H/2, 0, 0) * speed * Time.deltaTime;
            transform.position += new Vector3(0, V/2, 0) * speed * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(H, 0, 0) * speed * Time.deltaTime;
            transform.position += new Vector3(0, V, 0) * speed * Time.deltaTime;
        }
    }
}
