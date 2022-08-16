using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoMonstersFight : MonoBehaviour
{
    public SphereCollider sc;
    public float health;
    public float attackDamage;
    public MonsterScripts ms;
    float originalRadius;
    public static bool canStart;
    public int ri;
    public Rigidbody rb;
    public bool isDead;

    public string IdleNormalName;
    public string IdleCombatName;
    public string RunName;
    public string Attack1Name;
    public string Attack2Name;
    public string DeathName;
    public bool WasAwake;
    public manager managerScript;

    public float StartingHealth;
    public int monsterID;

    public bool EnemiesAllDead;
    public PlacementsScript CardReference;

    public float time;

    private void Awake()
    {
       
    }

    void Update()
    {
        time += -Time.deltaTime;
        if (time <= 0)
        {
            GetComponent<Animator>().Play(DeathName);
            this.enabled = false;

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        print("Triggered");
        print("Name: " + other.name);
        //if (!canStart) return;
        if (ms == null)
        {
            ms = other.GetComponent<MonsterScripts>();
            sc.radius = originalRadius;
            EnemiesAllDead = false;
            if (managerScript != null)
            {
                managerScript.finished = false;
            }
        }
    }

    void Attack()
    {
        transform.LookAt(ms.transform);
        if (ri % 2 == 0)
            GetComponent<Animator>().Play(Attack1Name);
        else
            GetComponent<Animator>().Play(Attack2Name);
    }

    public void DeadEvent()
    {
        isDead = true;
        GetComponent<Animator>().Play(DeathName);
        sc.enabled = false;
        this.enabled = false;
    }


}
