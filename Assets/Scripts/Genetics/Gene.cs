using System;
using UnityEngine;

// contains the genetic shared functionality of all genes
// inherited classes define the function of gene characteristics
public class Gene {
    
    float ideal_resources;
    float actual_resources;
    public float[] characteristics{
        get;
        private set;
    }

    public Gene(int size) {
        characteristics = new float[size];
    }

    public void Inherit(Gene parent, bool mutatable=false) {
        Array.Copy(parent.characteristics, characteristics, characteristics.Length);
        if (mutatable) {

        }
    }


}