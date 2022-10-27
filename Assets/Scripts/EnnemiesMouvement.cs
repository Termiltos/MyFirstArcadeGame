using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnnemiesMouvement : MonoBehaviour
{
    public bool isKillable;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float pointsGiven;

    private Vector3 directionOfFall;
    private GameObject thePlayer;
    private PhysicalMouvement mouvementOfEnnemy;
    private GameObject theLevel;
    private LevelGenerator levelGenerator;
    private TMP_Text score;


    void Awake()
    {
        mouvementOfEnnemy = GetComponent<PhysicalMouvement>();
        score = GameObject.Find("ScoreNumber").GetComponent<TMP_Text>();
    }

    void Start()
    {
        theLevel = GameObject.FindGameObjectWithTag("LevelGenerator");
        levelGenerator = theLevel.GetComponent<LevelGenerator>();
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        FixDirection();
    }

    public void FixDirection()
    {
        directionOfFall = (thePlayer.transform.position - transform.position).normalized;
        gameObject.transform.up = directionOfFall;
        mouvementOfEnnemy.ForceToAdd(directionOfFall * acceleration);
        mouvementOfEnnemy.speed = mouvementOfEnnemy.speed.normalized * Mathf.Clamp(mouvementOfEnnemy.speed.magnitude, 0f, maxSpeed);

        Vector3 pos = gameObject.transform.position;
        if(!isKillable && Mathf.Abs(pos.x) < levelGenerator.screenWidth && Mathf.Abs(pos.y) < levelGenerator.screenHeight)
        {
            isKillable = true;
        }
    }

    private void OnDestroy()
    {
        levelGenerator.scoreNumber += pointsGiven;
        score.text = "" + levelGenerator.scoreNumber;
    }
}
