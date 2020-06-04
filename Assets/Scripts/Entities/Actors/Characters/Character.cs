using System;
using UnityEngine;

public class Character : Actor {

    public Animator Animator;

    protected float health;
    protected float hunger;

    public float maxHealth = 100;
    public float maxHunger = 100;

    protected float lookAngle = 0;
    protected bool isInteracting = false;

    protected Rigidbody2D rigidbody2d;
    protected GridLocation gridLoc = new GridLocation();
    protected GridLocation interactGridLoc = new GridLocation();
    protected WorldObject interactableTile = null;

    protected Inventory inventory = new Inventory(5);
    protected Item equipedItem = new Tool();

    public WorldObject GetInteractableTile() {
        return interactableTile;
    }
    
    public Inventory GetInventory() {
        return inventory;
    }
    
    public Item GetEquipedItem() {
        return equipedItem;
    }

    protected void Init() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void SetGridLocation(int col, int row) {
        gridLoc.col = col;
        gridLoc.row = row;
        interactGridLoc.col = col;
        interactGridLoc.row = row+1;
    }

    // updates the humans's grid location
    // returns true if the location changed
    public bool CalcGridPosition() {

        // loops through the surrounding grid locations and checks to see which one is closest and updates row and column
        float closest_dist = 1000;
        int col = gridLoc.col;
        int row = gridLoc.row;
        int newCol=0;
        int newRow=0;
        for (int i=-1; i <= 1; i++) {

            for (int j = -1; j <= 1; j++) {

                //check for updates to the player's grid location
                Vector3 cell_loc = WorldController.Instance.GetCellLocation(col+j, row+i);
                float dist = Vector3.Distance(cell_loc, transform.position);
                if (dist < closest_dist) {

                    closest_dist = dist;
                    newCol = col+j;
                    newRow = row+i;
                }
            }
        }
        bool changed = (newCol != gridLoc.col || newRow != gridLoc.row);
        gridLoc.col = newCol;
        gridLoc.row = newRow;
        return changed;

    }

    // updates the human's interactable grid location and tile based on look angle
    // returns true if the location changed
    public bool CalcInteractableGridPos() {

        int locCol = 0;
        int locRow = 0;
        if (lookAngle >= 112.5 && lookAngle < 247.5)
        {
            locRow = 1;
        }
        else if (lookAngle >= 292.5 || lookAngle < 67.5)
        {
            locRow = -1;
        }
        if (lookAngle >= 22.5 && lookAngle < 157.5)
        {
            locCol = 1;
        }
        else if (lookAngle >= 202.5 && lookAngle < 338.5)
        {
            locCol = -1;
        }
        int newCol = gridLoc.col + locCol;
        int newRow = gridLoc.row + locRow;
        bool changed = (newCol != interactGridLoc.col || newRow != interactGridLoc.row);
        interactGridLoc.col = gridLoc.col + locCol;
        interactGridLoc.row = gridLoc.row + locRow;

        interactableTile = WorldController.Instance.GetInteractableAt(interactGridLoc.col, interactGridLoc.row);

        return changed;
    }


    // felt cute, might delete later
    public void Eat(float food) {
        hunger += food;
        if (hunger > maxHunger) {
            hunger = maxHunger;
        }
    }

    [Serializable]
    private struct CharacterInfo {
        public float Health;
        public float Hunger;
        public string InventoryInfo;
    }

    public override IEntity ObjectFromString(string info) {
        CharacterInfo humanInfo = JsonUtility.FromJson<CharacterInfo>(info);
        health = humanInfo.Health;
        hunger = humanInfo.Hunger;
        inventory.ObjectFromString(humanInfo.InventoryInfo);

        return this;
    }

    public override string ObjectToString() {
        CharacterInfo info;
        info.Health = health;
        info.Hunger = hunger;
        info.InventoryInfo = inventory.ObjectToString();

        return JsonUtility.ToJson(info);
    }
}