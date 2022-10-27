using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject particule;
    private List<GameObject> particules = new List<GameObject>();
    private float spaceBetween;

    void Start()
    {
        spaceBetween = Mathf.PI / 8f;
        for (int i = 0; i < 16; i++)
        {
            particules.Add(Instantiate<GameObject>(particule));
        }

        SetParticuleVelocity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetParticuleVelocity()
    {
        foreach(GameObject particule in particules)
        {
            Vector2 forceToAdd = new Vector2(Mathf.Sin(spaceBetween), Mathf.Cos(spaceBetween));
            PhysicalMouvement physicalMouvement = particule.GetComponent<PhysicalMouvement>();
            physicalMouvement.speed = forceToAdd * 10f;
            spaceBetween += Mathf.PI / 8f;
            Destroy(particule, 1f);
        }
        Destroy(gameObject, 1f);
    }
}
