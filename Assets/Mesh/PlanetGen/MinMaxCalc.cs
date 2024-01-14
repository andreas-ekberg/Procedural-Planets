using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxCalc
{
    public float Min {get; private set;}    
    public float Max {get; private set;}    

    public MinMaxCalc(){
        Min = 14.8f;
        Max = float.MinValue;


    }

    public void AddValue(float v){
        if(v > Max){
            Max = v;
            //Debug.Log(Max);
        }
    }
}
