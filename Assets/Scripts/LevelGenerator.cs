using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    [HideInInspector] public float screenWidth;
    [HideInInspector] public float screenHeight;

    public GameObject player;
    public int nbOfLifes;
    public List<GameObject> ennemies = new List<GameObject>();
    public List<GameObject> powerUps = new List<GameObject>();
    public List<GameObject> powerUpInGame = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject bulletHell;
    public List<GameObject> bulletList = new List<GameObject>();
    public float scoreNumber;
    public float ennemyFrequency;

    [SerializeField] private GameObject pileOfBullet;
    [SerializeField] private GameObject pileOfEnnemies;
    [SerializeField] private Color colorOfSpawn;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text lives;
    [SerializeField] private float respawnTime;

    private float minSpawnRadius;
    private float MaxSpawnRadius;
    private GameObject ennemiesContainer;
    private bool isDeath;
    private float timerOfDeath;
    private PlayerControls playerControls;
    private float timerFrequancy;
    private float initialEnnemyFrequency;

    void Awake()
    {
        initialEnnemyFrequency = ennemyFrequency;
        timerOfDeath = 0f;
        scoreNumber = 0f;
        score.text = "" + scoreNumber;
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x;
        screenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y;
        minSpawnRadius = (screenWidth + screenHeight) / 1.2f;
        MaxSpawnRadius = minSpawnRadius + minSpawnRadius / 1.5f;
        playerControls = Instantiate<GameObject>(player, transform).GetComponent<PlayerControls>();
        InvokeRepeating("EnnemyInstantiator", 1f, ennemyFrequency);
        InvokeRepeating("PowerUpInstantiator", 10f, 8f);
    }

    void Start()
    {
        bulletHell = Instantiate<GameObject>(pileOfBullet);
        ennemiesContainer = Instantiate<GameObject>(pileOfEnnemies);
    }

    void Update()
    {
        timerFrequancy += Time.deltaTime;
        if(GameObject.FindGameObjectWithTag("Player") && timerFrequancy >= 10f && ennemyFrequency >= 0.3f)
        {
            ennemyFrequency -= 0.2f;
            timerFrequancy = 0f;
            InvokeRepeating("EnnemyInstantiator", 1f, ennemyFrequency);
        }

        if (!GameObject.FindGameObjectWithTag("Player") && isDeath && nbOfLifes > 0)
        {
            timerOfDeath += Time.deltaTime;
            if (timerOfDeath > respawnTime)
            {
                playerControls = Instantiate<GameObject>(player, transform).GetComponent<PlayerControls>();
                ennemyFrequency = initialEnnemyFrequency;
                InvokeRepeating("EnnemyInstantiator", 1f, ennemyFrequency);
                InvokeRepeating("PowerUpInstantiator", 10f, 8f);
                isDeath = false;
                timerOfDeath = 0f;
                timerFrequancy = 0f;
            }
        }

        ClearDeadPrefabs();
        if (enemyList != null || powerUpInGame != null)
        {
            if (bulletList != null)
            {
                CheckIfCollisionWithBullet();
            }
        } 
    }

    void FixedUpdate()
    {
       
    }

    private void EnnemyInstantiator()
    {
        GameObject ennemy = Instantiate<GameObject>(ennemies[Random.Range(0, ennemies.Count)], ennemiesContainer.transform);
        ennemy.transform.position = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, MaxSpawnRadius);
        enemyList.Add(ennemy);
    }

    private void ClearDeadPrefabs()
    {
        if(enemyList.Count != 0)
        {
            for(int foe = 0; foe < enemyList.Count; ++foe)
            {
                Vector3 pos = enemyList[foe].transform.position;
                if(enemyList[foe].GetComponent<EnnemiesMouvement>().isKillable && (Mathf.Abs(pos.x) > screenWidth || Mathf.Abs(pos.y) > screenHeight))
                {
                    Destroy(enemyList[foe]);
                    enemyList.Remove(enemyList[foe]);
                }
            }
        }

        if(bulletList.Count != 0)
        {
            for(int bullet = 0; bullet < bulletList.Count; ++bullet)
            {
                Vector3 pos = bulletList[bullet].transform.position;
                PhysicalCollider collider = bulletList[bullet].GetComponent<PhysicalCollider>();
                if (Mathf.Abs(pos.x) > screenWidth || Mathf.Abs(pos.y) > screenHeight || !collider.isBullet)
                {
                    Destroy(bulletList[bullet], 1f);
                    bulletList.Remove(bulletList[bullet]);
                }
            }
        }

        if(!GameObject.FindGameObjectWithTag("Player") && !isDeath)
        {
            CancelInvoke();
            OnPlayerDeath();
            --nbOfLifes;
            lives.text = "Lives: " + nbOfLifes;
        }
    }

    private void CheckIfCollisionWithBullet()
    {
        foreach (GameObject bullet in bulletList)
        {
            foreach (GameObject ennemy in enemyList)
            {
                PhysicalCollider collider = bullet.GetComponent<PhysicalCollider>();
                PhysicalCollider collider2 = ennemy.GetComponent<PhysicalCollider>();
                Vector3 dist = ennemy.transform.position - bullet.transform.position;
                if (dist.magnitude <= collider.radius + collider2.radius)
                {
                    if(collider.isBullet)
                    {
                        collider.ImpulseCollisionCalculator(ennemy, dist.normalized);
                        collider.isBullet = false;
                        if(collider2.elementOfImpact < 1f)
                        {
                            collider2.elementOfImpact += 0.25f;
                        }
                    }   
                }
            }

            for (int i = 0; i < powerUpInGame.Count; ++i)
            {
                PhysicalCollider collider = bullet.GetComponent<PhysicalCollider>();
                PhysicalCollider collider2 = powerUpInGame[i].GetComponent<PhysicalCollider>();
                Vector3 dist = powerUpInGame[i].transform.position - bullet.transform.position;
                if (dist.magnitude <= collider.radius + collider2.radius)
                {
                    if (powerUpInGame[i].CompareTag("PowerUp"))
                    {
                        playerControls.powerUp = true;
                        playerControls.powerTimer = 0f;
                    }
                    else
                    {
                        playerControls.speedUp = true;
                        playerControls.speedUpBullet = 3f;
                        playerControls.speedTimer = 0f;
                    }

                    Destroy(powerUpInGame[i]);
                    powerUpInGame.Remove(powerUpInGame[i]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    { 
        Gizmos.color = colorOfSpawn;
        Gizmos.DrawWireSphere(transform.position, minSpawnRadius);
        Gizmos.DrawWireSphere(transform.position, MaxSpawnRadius);
    }

    private void OnPlayerDeath()
    {
        if (enemyList.Count != 0)
        {
            for (int foe = 0; foe < enemyList.Count; ++foe)
            {
                Destroy(enemyList[foe]);
            }
        }

        if (bulletList.Count != 0)
        {
            for (int bullet = 0; bullet < bulletList.Count; ++bullet)
            {   
                Destroy(bulletList[bullet]);  
            }
        }

        enemyList.Clear();
        bulletList.Clear();
        isDeath = true;
    }

    private void PowerUpInstantiator()
    {
        GameObject p = Instantiate<GameObject>(powerUps[Random.Range(0, powerUps.Count)]);
        p.transform.position = new Vector3(Random.Range(-screenWidth + 2, screenWidth - 2), Random.Range(-screenHeight + 2, screenHeight - 2));
        powerUpInGame.Add(p);
    }
}
