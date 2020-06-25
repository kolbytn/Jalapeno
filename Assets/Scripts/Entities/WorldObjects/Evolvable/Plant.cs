using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : WorldObject, IEvolvable
{
    protected Gene genes;
    public int growthStage{
        get;
        protected set;
    }
    protected GridLocation gridLoc;
    // how much time the plant lives in each stage (minutes)
    protected float[] growthStageTimes;
    protected float timer;

    // absolute time of last growth stage update
    private float lastTime;

    protected new virtual void Start() {
        base.Start();
    }

    public void Init(int col, int row) {
        gridLoc = new GridLocation();
        gridLoc.col = col;
        gridLoc.row = row;
        lastTime = Time.realtimeSinceStartup;
        SetGrowthStageTimes();
    }

    public void RandomStage() {
        SetGrowthStage(Random.Range(0, TotalGrowthStages()));
        timer = Random.Range(0.0f, growthStageTimes[growthStage]);
    }

    public void SetGrowthStage(int stage) {
        growthStage = stage;
        lastTime = Time.realtimeSinceStartup;
        timer = 0;
        UpdateAnimation();
    }

    // return true if the plant died
    public bool UpdateGrowth() {
        if (growthStage >= TotalGrowthStages()) {
            return false;
        }
        float timePassed = Time.realtimeSinceStartup - lastTime;
        timer += timePassed / 60.0f;
        // Debug.Log(timer.ToString());
        if(timer > growthStageTimes[growthStage]) {
            growthStage ++;
            timer = 0;
            if (growthStage >= TotalGrowthStages()) {
                Destroy(gameObject);
                return true;
            }
            UpdateStageChange();
        }
        lastTime = Time.realtimeSinceStartup;
        UpdateAnimation();
        return false;
    }


    
    protected virtual void UpdateAnimation() {}

    protected virtual void SetGrowthStageTimes() { }

    protected virtual int TotalGrowthStages() { return 0;}

    protected virtual void UpdateStageChange() {}

    public override void Interact(Character user) {}

    public override string ObjectToString(){
        return "";
    }

    public override IEntity ObjectFromString(string info){
        return null;
    }
}
