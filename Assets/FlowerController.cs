using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerController : MonoBehaviour
{

    float honeyAvailable = 30;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public float TakeHoney() {
        float honeyToGive = honeyAvailable;
        honeyAvailable = 0;
        return honeyToGive;
    }
}
