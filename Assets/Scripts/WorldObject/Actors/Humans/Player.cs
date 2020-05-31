using System;
using UnityEngine;

public class Player : Human
{

    public float speed = 5;

    UiBar health_bar;
    UiBar hunger_bar;
    private GameObject HightlightSprite = null;


    readonly float hunger_speed = 1;

    float horizontal = 0;
    float vertical = 0;
    bool mouse_down = false;
    bool mouse_up = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        health = max_health;
        hunger = max_hunger;

        health_bar = GameObject.Find("Healthbar").GetComponent<UiBar>();
        hunger_bar = GameObject.Find("Hungerbar").GetComponent<UiBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hunger > 0)
        {
            hunger -= hunger_speed * Time.deltaTime;
            hunger_bar.UpdateValue(hunger);
        }

        CalcGridPosition();
        bool changed = CalcInteractableGridPos();
        
        if (changed)
        {
            Destroy(HightlightSprite);
            HightlightSprite = GameObject.Instantiate(WorldResources.HighlightPrefab,
                WorldController.Instance.GetCellLocation(InteractGridLoc.col, InteractGridLoc.row),
                Quaternion.identity);
        }


        CollectInputs();
        animator.SetFloat("Speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        if (mouse_down && !isInteracting && InteractableTile != null){
            isInteracting = true;
            InteractableTile.Interact(this);
            equipedItem.use(this);
        }
        else if(mouse_up){
            isInteracting = false;
        }

    }

    void FixedUpdate()
    {
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 new_position = rigidbody2d.position + move * speed * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(new_position);
        rigidbody2d.MoveRotation(look_angle);
    }


    // collects inputs and calculates look direction
    void CollectInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouse_down = Input.GetMouseButtonDown(0);
        mouse_up = Input.GetMouseButtonUp(0);

        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float x1 = mousePosition.x;
        float x2 = rigidbody2d.position.x;
        float y1 = mousePosition.y;
        float y2 = rigidbody2d.position.y;
        float xDiff = x1 - x2;
        float yDiff = y1 - y2;
        look_angle = (Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg) + 90;
        if (look_angle < 0)
        {
            look_angle += 360;
        }
    }

}
