using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCollider : MonoBehaviour
{
    public float radius;
    public bool isCircle;
    public bool isBullet;
    public float elementOfImpact;

    [SerializeField] private Color colliderColor;
    
    void Start()
    {
        
    }

    
    void Update()
    {
 
    }

    private void OnDrawGizmos()
    {
        if(isCircle)
        {
            Gizmos.color = colliderColor;
            Gizmos.DrawWireSphere(transform.position, radius);
        }  
    }

    public void ImpulseCollisionCalculator(GameObject otherObject, Vector3 norm)
    {
        PhysicalMouvement mouvementA = gameObject.GetComponent<PhysicalMouvement>();
        PhysicalMouvement mouvementB = otherObject.GetComponent<PhysicalMouvement>();
        PhysicalCollider colliderB = otherObject.GetComponent<PhysicalCollider>();

        float restitutionConst = (elementOfImpact + colliderB.elementOfImpact) / 2f;
        Vector3 relativeSpeed = mouvementB.speed - mouvementA.speed;

        float Impulsion = (-(1 + restitutionConst) * Vector3.Dot(relativeSpeed, norm)) / ((1 / mouvementA.mass) + (1 / mouvementB.mass));

        mouvementA.UpdateSpeedOnCollision(-(Impulsion / mouvementA.mass) * norm);
        mouvementB.UpdateSpeedOnCollision((Impulsion / mouvementB.mass) * norm);
    }
}
