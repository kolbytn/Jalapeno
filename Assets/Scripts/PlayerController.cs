using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float speed = 5;

    public float max_health = 100;
    public float max_hunger = 100;

    public UiBar health_bar;
    public UiBar hunger_bar;
    float health;
    float hunger;
    float hunger_speed = 0.1f;

    Rigidbody2D rigidbody2d;
    float horizontal = 0;
    float vertical = 0;
    float look_angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = max_health;
        hunger = max_hunger;

        health_bar.initializeValues(max_health, health);
        hunger_bar.initializeValues(max_hunger, hunger);
    }

    // Update is called once per frame
    void Update()
    {

        if (hunger > 0)
        {
            hunger -= hunger_speed * Time.deltaTime;
            hunger_bar.updateValue(hunger);
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float x1 = mousePosition.x;
        float x2 = rigidbody2d.position.x;
        float y1 = mousePosition.y;
        float y2 = rigidbody2d.position.y;
        float xDiff = x1 - x2;
        float yDiff = y1 - y2;
        look_angle = (float)(Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg) + 90;
    }

    void FixedUpdate()
    {
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 new_position = rigidbody2d.position + move * speed * Time.fixedDeltaTime;


        rigidbody2d.MovePosition(new_position);
        rigidbody2d.MoveRotation(look_angle);
        //rigidbody2d.transform.center
        //transform.position = new_position;
        //transform.rotation = Quaternion.Euler(0, 0, look_angle);
    }
}
