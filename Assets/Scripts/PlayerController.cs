using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; 

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f);

        GetComponent<Rigidbody2D>().MovePosition(new Vector2((transform.position.x + horizontal * speed * Time.deltaTime),
            transform.position.y + vertical * speed * Time.deltaTime));
        
        //transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}

