using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : Character {

    public float Speed = 5;

    UiBar healthBar;
    UiBar hungerBar;
    UiHotbar hotbar;
    Inventory hotBarInventory = new Inventory(4);
    GameObject hightlightSprite = null;
    public Light2D flashLight;


    readonly float hungerSpeed = 0.5f;
    readonly float starvationSpeed = 2f;

    float horizontal = 0;
    float vertical = 0;
    bool mouseDownL = false;
    bool mouseUpL = false;
    bool mouseDownR = false;
    bool mouseUpR = false;
    float mouseScroll = 0; //negative down, 0 no change, positive up
    float mouseX; //absolute world coordinates
    float mouseY;

    protected new void Start() {
        base.Start();
        health = maxHealth;
        hunger = maxHunger;

        healthBar = GameObject.Find("HealthBar").GetComponent<UiBar>();
        hungerBar = GameObject.Find("HungerBar").GetComponent<UiBar>();
        hotbar = GameObject.Find("Inventory").GetComponent<UiHotbar>();

        hotBarInventory.AddItem(new Tool());

        hotbar.SetInventory(hotBarInventory);
        hotbar.SetEquiped(equipedItemIndex);
    }

    void Update() {

        if (hunger > 0) {
            hunger -= hungerSpeed * Time.deltaTime;
            if (hunger < 0) {
                hunger = 0;
            }
            hungerBar.UpdateValue(hunger);
        }
        else {
            health -= starvationSpeed * Time.deltaTime;
            healthBar.UpdateValue(health);
        }

        flashLight.intensity = 1 - WorldController.Instance.DayCycleController.GetLightIntensity();

        if (health <= 0) {
            Application.Quit(); // disabled in editor
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

        if (!isInteracting) {
            if (mouseDownL) {
                isInteracting = true;
                if (equipedItem != null) {
                    equipedItem.UseL(this);
                }
                else {
                    // default behavior
                    DefaultAction();
                }
            }
            else if (mouseDownR) {
                isInteracting = true;
                if (equipedItem != null) {
                    equipedItem.UseR(this);
                }
            }
            
        }
        else if(mouseUpL || mouseUpR) {
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

    public override bool CalcInteractableGridPos() {
        float closest_dist = 1000;
        int col = gridLoc.col;
        int row = gridLoc.row;
        int newCol=0;
        int newRow=0;
        for (int i=-1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                Vector3 cell_loc = WorldController.Instance.GetCellLocation(col+j, row+i);
                float dist = Vector3.Distance(cell_loc, new Vector3(mouseX, mouseY, 0));
                if (dist < closest_dist) {

                    closest_dist = dist;
                    newCol = col+j;
                    newRow = row+i;
                }
            }
        }
        bool changed = (newCol != interactGridLoc.col || newRow != interactGridLoc.row);
        interactGridLoc.col = newCol;
        interactGridLoc.row = newRow;
        interactableTile = WorldController.Instance.GetInteractableAt(interactGridLoc.col, interactGridLoc.row);
        return changed;
    }


    // collects inputs and calculates look direction
    void CollectInputs() {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseDownL = Input.GetMouseButtonDown(0);
        mouseUpL = Input.GetMouseButtonUp(0);
        mouseDownR = Input.GetMouseButtonDown(1);
        mouseUpR = Input.GetMouseButtonUp(1);
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

        mouseX = x1;
        mouseY = y1;
    }
}
