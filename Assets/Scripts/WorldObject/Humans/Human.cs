using System;
using UnityEngine;

public class Human : WorldObject
{

    public Animator animator;

    protected float health;
    protected float hunger;

    public float max_health = 100;
    public float max_hunger = 100;

    protected float look_angle = 0;
    protected bool isInteracting = false;

    protected Rigidbody2D rigidbody2d;
    protected GridLocation GridLoc = new GridLocation();
    protected GridLocation InteractGridLoc = new GridLocation();
    protected InteractableObject InteractableTile = null;

    protected Inventory inventory = new Inventory(5);
    protected Item equipedItem = new Tool();


    public InteractableObject GetInteractableTile(){
        return InteractableTile;
    }
    
    public Inventory GetInventory(){
        return inventory;
    }
    
    public Item GetEquipedItem(){
        return equipedItem;
    }

    protected void Init(){
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void SetGridLocation(int col, int row)
    {
        GridLoc.col = col;
        GridLoc.row = row;
        InteractGridLoc.col = col;
        InteractGridLoc.row = row+1;
    }

    // updates the humans's grid location
    // returns true if the location changed
    public bool CalcGridPosition()
    {
        // loops through the surrounding grid locations and checks to see which one is closest and updates row and column
        float closest_dist = 1000;
        int col = GridLoc.col;
        int row = GridLoc.row;
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
        bool changed = (new_col != GridLoc.col || new_row != GridLoc.row);
        GridLoc.col = new_col;
        GridLoc.row = new_row;
        return changed;

    }

    // updates the human's interactable grid location and tile based on look angle
    // returns true if the location changed
    public bool CalcInteractableGridPos()
    {
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
        int new_col=GridLoc.col + loc_col;
        int new_row=GridLoc.row + loc_row;
        bool changed = (new_col != InteractGridLoc.col || new_row != InteractGridLoc.row);
        InteractGridLoc.col = GridLoc.col + loc_col;
        InteractGridLoc.row = GridLoc.row + loc_row;

        InteractableTile = WorldController.Instance.GetInteractableAt(InteractGridLoc.col, InteractGridLoc.row);

        return changed;
    }


    // felt cute, might delete later
    public void Eat(float food)
    {
        hunger += food;
        if (hunger > max_hunger){
            hunger = max_hunger;
        }
    }

    [Serializable]
    private struct HumanInfo
    {
        public float health;
        public float hunger;
    }

    public override WorldObject ObjectFromString(string info)
    {
        HumanInfo humanInfo = JsonUtility.FromJson<HumanInfo>(info);
        health = humanInfo.health;
        hunger = humanInfo.hunger;

        return this;
    }

    public override string ObjectToString()
    {
        HumanInfo info;
        info.health = health;
        info.hunger = hunger;

        return JsonUtility.ToJson(info);
    }
}