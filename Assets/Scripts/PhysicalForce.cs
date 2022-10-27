using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalForce : MonoBehaviour
{
    public Vector3 gravity = new Vector3(0f, -9.81f, 0f);
    public bool noGravity;

    void Awake()
    {
        if(noGravity)
        {
            gravity = Vector3.zero;
        }  
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    

}
