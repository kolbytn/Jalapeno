using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Plant
{

    public Animator Animator;

    protected override void Start() {
        base.Start();
    }

    protected override void UpdateAnimation(){
        Animator.SetInteger("Stage", growthStage);
    }

    protected override void SetGrowthStageTimes() {
        growthStageTimes = new float[] { 0.1f, 0.1f, 0.1f, 0.1f};
    }

    protected override void UpdateStageChange() {
        if (growthStage == 3) { 
            int col = gridLoc.col + Random.Range(-3, 3); 
            int row = gridLoc.row + Random.Range(-3, 3); 
            WorldController.Instance.AddPlant(WorldResources.Grass, col, row);
        }
    }

    protected override int TotalGrowthStages() {
        return 4;
    }


    public override void Interact(Character user){
        if (growthStage == 2) {
            user.GiveItem(new GrassSeed(1));
            user.GiveItem(new Grain(1));
            SetGrowthStage(3);
        }
    }

    public override string ObjectToString(){
        return "";
    }

    public override IEntity ObjectFromString(string info){
        return null;
    }
}
