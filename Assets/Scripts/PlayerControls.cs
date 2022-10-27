using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject smallBullet;
    public GameObject bigBullet;
    public Vector3 visorPosition;
    public bool activeVisor;
    public bool powerUp;
    public bool speedUp;
    public float speedUpBullet;
    public float powerTimer;
    public float speedTimer;

    [SerializeField] private Color colorOfVisor;
    [SerializeField] private float baseSpeedOfBullet;
    [SerializeField] private float frequencyOfBullet;
    [SerializeField] private LevelGenerator theLevel;
    [SerializeField] private GameObject onDeath;


    private Vector3 directionOfPlayer;
    private float timerOfBullet;
    private GameObject bulletPile;
    private float ratioOfSpeed;

    void Awake()
    {
        speedUpBullet = 1f;
        thePlayer = gameObject;
        ratioOfSpeed = bigBullet.GetComponent<PhysicalMouvement>().mass / smallBullet.GetComponent<PhysicalMouvement>().mass;
    }

    void Start()
    {
        theLevel = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        bulletPile = theLevel.bulletHell;
        timerOfBullet = 0f;
    }

    
    void Update()
    {
        timerOfBullet += Time.deltaTime;
        visorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        visorPosition = new Vector3(visorPosition.x, visorPosition.y, 0f);
        directionOfPlayer = (visorPosition - gameObject.transform.position).normalized;

        rotatePlayer();

        if (activeVisor)
        {
            CastVisor();
        } 
        
        if(Input.GetKey(KeyCode.Space) && timerOfBullet >= frequencyOfBullet / speedUpBullet)
        {
            Shoot(powerUp, speedUp);
            timerOfBullet = 0f;
        }

        if(speedUp)
        {
            speedTimer += Time.deltaTime;
            if (speedTimer > 10f)
            {
                speedUp = false;
                speedTimer = 0f;
                speedUpBullet = 1f;
            }
        }

        if(powerUp)
        {
            powerTimer += Time.deltaTime;
            if (powerTimer > 10f)
            {
                powerUp = false;
                powerTimer = 0f;
            }
        }

        CheckIfCollisionWithPlayer();
    }

    private void rotatePlayer()
    {
        gameObject.transform.up = directionOfPlayer;
    }

    private void CastVisor()
    {
        Debug.DrawRay(gameObject.transform.position, visorPosition - gameObject.transform.position, colorOfVisor);
    }

    private void Shoot(bool power, bool speed)
    {
        PhysicalMouvement playerBullet;
        if (power)
        {
            playerBullet = Instantiate<GameObject>(bigBullet, bulletPile.transform).GetComponent<PhysicalMouvement>();
            playerBullet.ForceToAdd(directionOfPlayer * baseSpeedOfBullet * ratioOfSpeed);
        }
        else
        {
            playerBullet = Instantiate<GameObject>(smallBullet, bulletPile.transform).GetComponent<PhysicalMouvement>();
            playerBullet.ForceToAdd(directionOfPlayer * baseSpeedOfBullet);
        }

        theLevel.bulletList.Add(playerBullet.gameObject);
    }

    private void CheckIfCollisionWithPlayer()
    {
        if(theLevel.enemyList != null)
        {
            foreach(GameObject ennemy in theLevel.enemyList)
            {
                PhysicalCollider thePlayer = GetComponent<PhysicalCollider>();
                PhysicalCollider theEnnemy = ennemy.GetComponent<PhysicalCollider>();
                if ((gameObject.transform.position - ennemy.transform.position).magnitude <= theEnnemy.radius + thePlayer.radius)
                {
                    Instantiate<GameObject>(onDeath);
                    Destroy(gameObject);
                }
            }
        }
    }
}
