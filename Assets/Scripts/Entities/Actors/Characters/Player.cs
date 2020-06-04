using UnityEngine;

public class Player : Character {

    public float Speed = 5;

    UiBar healthBar;
    UiBar hungerBar;
    private GameObject hightlightSprite = null;


    readonly float hungerSpeed = 1;

    float horizontal = 0;
    float vertical = 0;
    bool mouseDown = false;
    bool mouseUp = false;

    protected new void Start() {
        base.Start();
        health = maxHealth;
        hunger = maxHunger;

        healthBar = GameObject.Find("Healthbar").GetComponent<UiBar>();
        hungerBar = GameObject.Find("Hungerbar").GetComponent<UiBar>();
    }

    void Update() {

        if (hunger > 0) {
            hunger -= hungerSpeed * Time.deltaTime;
            hungerBar.UpdateValue(hunger);
        }

        CalcGridPosition();
        bool changed = CalcInteractableGridPos();
        
        if (changed) {

            Destroy(hightlightSprite);
            hightlightSprite = GameObject.Instantiate(WorldResources.HighlightPrefab,
                WorldController.Instance.GetCellLocation(interactGridLoc.col, interactGridLoc.row),
                Quaternion.identity);
        }


        CollectInputs();
        animator.SetFloat("Speed", Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        if (mouseDown && !isInteracting && interactableTile != null) {
            isInteracting = true;
            interactableTile.Interact(this);
            equipedItem.use(this);
        }
        else if(mouseUp) {
            isInteracting = false;
        }

    }

    void FixedUpdate() {
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 newPosition = rigidbody2d.position + move * Speed * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(newPosition);
        rigidbody2d.MoveRotation(lookAngle);
    }


    // collects inputs and calculates look direction
    void CollectInputs() {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseDown = Input.GetMouseButtonDown(0);
        mouseUp = Input.GetMouseButtonUp(0);

        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float x1 = mousePosition.x;
        float x2 = rigidbody2d.position.x;
        float y1 = mousePosition.y;
        float y2 = rigidbody2d.position.y;
        float xDiff = x1 - x2;
        float yDiff = y1 - y2;
        lookAngle = (Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg) + 90;
        if (lookAngle < 0) {
            lookAngle += 360;
        }
    }
}
