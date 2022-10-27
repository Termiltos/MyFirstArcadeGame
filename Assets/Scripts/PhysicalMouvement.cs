using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalMouvement : MonoBehaviour
{
    public float mass;
    public Vector3 acceleration;
    public Vector3 speed;
    public Vector3 position;
    public Vector3 forceToAdd;

    private GameObject physicalForceObject;
    private PhysicalForce physicalForce;

    void Awake()
    {
        physicalForceObject = GameObject.FindGameObjectWithTag("ForceManipulator");
        physicalForce = physicalForceObject.GetComponent<PhysicalForce>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(gameObject.tag == "Ennemy")
        {
            gameObject.GetComponent<EnnemiesMouvement>().FixDirection();
        }

        position = gameObject.transform.position;
        acceleration = (forceToAdd + physicalForce.gravity) / mass;
        speed += acceleration * Time.fixedDeltaTime;
        position += speed * Time.fixedDeltaTime;
        gameObject.transform.position = position;

        forceToAdd = Vector3.zero;
    }

    public void ForceToAdd(Vector3 forceAdded)
    {
        forceToAdd += forceAdded;
    }

    public void UpdateSpeedOnCollision(Vector3 speedAdded)
    {
        speed += speedAdded;
    }
}
