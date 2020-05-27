using System;
using UnityEngine;

public class PlayerController : WorldObject
{
    public Animator animator;

    public float speed = 5;

    public float max_health = 100;
    public float max_hunger = 100;

    public UiBar health_bar;
    public UiBar hunger_bar;
    float health;
    float hunger;
    readonly float hunger_speed = 1;

    Rigidbody2D rigidbody2d;
    float horizontal = 0;
    float vertical = 0;
    float look_angle = 0;
    bool mouse_down = false;
    bool mouse_up = false;
    bool isInteracting = false;

    GridLocation PlayerGridLoc = new GridLocation();
    GridLocation InteractGridLoc = new GridLocation();
    
    InteractableObject InteractableTile = null;
    Inventory inventory = new Inventory(5);
    Item equipedItem = new Tool();

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = max_health;
        hunger = max_hunger;

        health_bar = GameObject.Find("Healthbar").GetComponent<UiBar>();
        hunger_bar = GameObject.Find("Hungerbar").GetComponent<UiBar>();
    }

    public void SetGridLocation(int col, int row)
    {
        PlayerGridLoc.col = col;
        PlayerGridLoc.row = row;
        InteractGridLoc.col = col;
        InteractGridLoc.row = row+1;
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


    // temporary debug function.
    public void Eat(float food)
    {
        hunger += food;
        if (hunger > max_hunger){
            hunger = max_hunger;
        }
    }

    private GameObject HightlightSprite = null;
    void CalcGridPosition()
    {
        // loops through the surrounding grid locations and checks to see which one is closest and updates row and column
        float closest_dist = 1000;
        int col = PlayerGridLoc.col;
        int row = PlayerGridLoc.row;
        int new_col=0;
        int new_row=0;
        for (int i=-1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                //check for updates to the player's grid location
                Vector3 cell_loc = WorldController.Instance.GetCellLocation(col+j, row+i);
                float dist = Vector3.Distance(cell_loc, transform.position);
                if (dist < closest_dist)
                {
                    closest_dist = dist;
                    new_col = col+j;
                    new_row = row+i;
                }
            }
        }
        PlayerGridLoc.col = new_col;
        PlayerGridLoc.row = new_row;

        int loc_col = 0;
        int loc_row = 0;
        if (look_angle >= 112.5 && look_angle < 247.5)
        {
            loc_row = 1;
        }
        else if (look_angle >= 292.5 || look_angle < 67.5)
        {
            loc_row = -1;
        }
        if (look_angle >= 22.5 && look_angle < 157.5)
        {
            loc_col = 1;
        }
        else if (look_angle >= 202.5 && look_angle < 338.5)
        {
            loc_col = -1;
        }
        new_col=PlayerGridLoc.col + loc_col;
        new_row=PlayerGridLoc.row + loc_row;
        bool changed = (new_col != InteractGridLoc.col || new_row != InteractGridLoc.row);
        InteractGridLoc.col = PlayerGridLoc.col + loc_col;
        InteractGridLoc.row = PlayerGridLoc.row + loc_row;

        InteractableTile = WorldController.Instance.GetInteractableAt(InteractGridLoc.col, InteractGridLoc.row);

        if (changed)
        {
            Destroy(HightlightSprite);
            HightlightSprite = GameObject.Instantiate(WorldResources.HighlightPrefab,
                WorldController.Instance.GetCellLocation(InteractGridLoc.col, InteractGridLoc.row),
                Quaternion.identity);
        }

        // WorldController.Instance.ChangeTile(InteractGridLoc.col, InteractGridLoc.row, WorldResources.GrassTile);
        // WorldController.Instance.ChangeTile(PlayerGridLoc.row, PlayerGridLoc.col, WorldResources.BlankTile);

        // Debug.Log(look_angle);
        //Debug.Log("Player Grid Location: " + new_row + ", " + new_col);
        //Debug.Log("Interactable Grid Location: " + InteractGridLoc.row + ", " + InteractGridLoc.col);

    }

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

    public InteractableObject GetInteractableTile(){
        return InteractableTile;
    }
    
    public Inventory GetInventory(){
        return inventory;
    }
    
    public Item GetEquipedItem(){
        return equipedItem;
    }

    [Serializable]
    private struct PlayerInfo
    {
        public float health;
        public float hunger;
    }

    public override WorldObject ObjectFromString(string info)
    {
        PlayerInfo playerInfo = JsonUtility.FromJson<PlayerInfo>(info);
        health = playerInfo.health;
        hunger = playerInfo.hunger;

        return this;
    }

    public override string ObjectToString()
    {
        PlayerInfo info;
        info.health = health;
        info.hunger = hunger;

        return JsonUtility.ToJson(info);
    }
}
