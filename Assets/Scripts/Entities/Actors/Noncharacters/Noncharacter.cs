using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noncharacter : Actor {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }

    public override string ObjectToString() {
        return "";
    }
}
