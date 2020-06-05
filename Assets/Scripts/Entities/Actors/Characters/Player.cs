using UnityEngine;

public class Player : Character {

    public float Speed = 5;

    UiBar healthBar;
    UiBar hungerBar;
    UiHotbar hotbar;
    Inventory hotBarInventory = new Inventory(4);
    private GameObject hightlightSprite = null;


    readonly float hungerSpeed = 1;

    float horizontal = 0;
    float vertical = 0;
    bool mouseDown = false;
    bool mouseUp = false;
    float mouseScroll = 0; //negative down, 0 no change, positive up

    protected new void Start() {
        base.Start();
        health = maxHealth;
        hunger = maxHunger;

        healthBar = GameObject.Find("Healthbar").GetComponent<UiBar>();
        hungerBar = GameObject.Find("Hungerbar").GetComponent<UiBar>();
        hotbar = GameObject.Find("Inventory").GetComponent<UiHotbar>();

        hotBarInventory.AddItem(new Tool());
        hotBarInventory.AddItem(new Food(5));
        hotBarInventory.AddItem(new Food(2));


        hotbar.SetInventory(hotBarInventory);
        hotbar.SetEquiped(equipedItemIndex);
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

        if (mouseDown && !isInteracting) {
            isInteracting = true;
            if (equipedItem != null) {
                equipedItem.use(this);
            }
            else {
                // default behavior
                if (interactableTile != null){
                    interactableTile.Interact(this);
                }
            }
        }
        else if(mouseUp) {
            isInteracting = false;
        }

        if (mouseScroll > 0){
            equipedItemIndex++;
            if (equipedItemIndex >= hotBarInventory.Size())
                equipedItemIndex=0;
        }
        else if (mouseScroll < 0) {
            equipedItemIndex--;
            if (equipedItemIndex < 0)
                equipedItemIndex = hotBarInventory.Size()-1;
        }

        UpdateInventory();

    }

    void FixedUpdate() {
        Vector2 move = new Vector2(horizontal, vertical);
        Vector2 newPosition = rigidbody2d.position + move * Speed * Time.fixedDeltaTime;

        rigidbody2d.MovePosition(newPosition);
        rigidbody2d.MoveRotation(lookAngle);
    }

    void UpdateInventory() {
        hotBarInventory.ClearEmptyItems();
        equipedItem = hotBarInventory.ItemAt(equipedItemIndex);
        hotbar.SetInventory(hotBarInventory);
        hotbar.SetEquiped(equipedItemIndex);
    }

    public override void GiveItem(Item item){
        Item remaining = hotBarInventory.AddItem(item);
        if (remaining == null) {
            inventory.AddItem(remaining);
        }
    }


    // collects inputs and calculates look direction
    void CollectInputs() {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseDown = Input.GetMouseButtonDown(0);
        mouseUp = Input.GetMouseButtonUp(0);
        mouseScroll = Input.mouseScrollDelta.y;

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
