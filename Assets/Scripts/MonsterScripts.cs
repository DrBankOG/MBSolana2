using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterScripts : MonoBehaviourPunCallbacks, IPunObservable
{

    public SphereCollider sc;
    public NavMeshAgent nma;
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
    PhotonView pv;
    public bool WasAwake;
    public manager managerScript;

    public float StartingHealth;
    public int monsterID;

    public bool EnemiesAllDead;
    public PlacementsScript CardReference;
    

    private void Awake()
    {
        EnemiesAllDead = false;
        WasAwake = true;
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        nma = GetComponent<NavMeshAgent>();
        pv = GetComponent<PhotonView>();
        ms = null;
        originalRadius = sc.radius;
        //canStart = false;
        if (!canStart)
            GetComponent<Animator>().Play(IdleNormalName);
        else
        {
            GetComponent<Animator>().Play(IdleCombatName);
        }
        ResetMonster();
        //StartingHealth = managerScript.HPC;
    }


    public void ResetMonster()
    {
        //if (!photonView.IsMine) return;
        try
        {
            if (!WasAwake) return;

            EnemiesAllDead = false;
            isDead = false;
            sc.enabled = true;
            nma.enabled = true;
            this.enabled = true;
            nma.isStopped = false;
            ms = null;
            attackDamage = managerScript.ADC;
            health = 100 + managerScript.HPC;
        }
        catch
        {
            print("Err MonsterScript 1");
        }

    }

    void Update()
    {
        // print("Can Start: " + canStart);
        // print("Is Dead: " + isDead);
        //if (photonView.IsMine)
        //{
        if (!canStart) return;
        if (isDead) return;

        //Expand collider
        if (ms == null)
        {
            if (sc.radius < 10)
            {
                GetComponent<Animator>().Play(IdleCombatName);
                sc.radius += 0.1f;

            }
            else
            {
                EnemiesAllDead = true;
                nma.isStopped = true;
                if (managerScript != null)
                {
                    managerScript.finished = true;
                }
            }
        }
        else
        {
            if (EnemiesAllDead) return;
            Vector3 walkpoint = transform.position - ms.transform.position;

            //print("Walk Point: " + walkpoint);

            if (walkpoint.magnitude < 2.5f)
            {
                //print("Stopped");
                nma.isStopped = true;
                Attack();
            }
            else
            {
                //if (EnemiesAllDead) return;
                //GetComponent<Animator>().Play("Run");
                GetComponent<Animator>().Play(RunName);
                nma.isStopped = false;
                nma.SetDestination(ms.transform.position);
                transform.LookAt(ms.transform);

            }

            if (ms.isDead)
            {
                ms = null;
                //GetComponent<Animator>().Play("IdleCombat");
                GetComponent<Animator>().Play(IdleCombatName);
                nma.isStopped = true;
            }
        }
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        print("Triggered");
        print("Name: " + other.name);
        //if (!canStart) return;
        if (ms == null)
        {
            ms = other.GetComponent<MonsterScripts>();
            nma.isStopped = false;
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
        if(ri%2==0)
            GetComponent<Animator>().Play(Attack1Name);
        else
            GetComponent<Animator>().Play(Attack2Name);
    }

    [PunRPC]
    public void ReduceHealth(float d)
    {
        if (ms == null) return;
        ms.health += -d;
        if (ms.health <= 0)
        {
            ms.DeadEvent();
        }
    }

    public void DeadEvent()
    {
        isDead = true;
        GetComponent<Animator>().Play(DeathName);
        sc.enabled = false;
        nma.enabled = false;
        this.enabled = false;
    }

    public void AttackEvent(float s)
    {
        if (photonView.IsMine)
        {
            
            float s2 = Random.Range(s + attackDamage, s + attackDamage + 7.5f);
            pv.RPC("ReduceHealth", RpcTarget.AllBuffered, s2);
            //ms.ReduceHealth(s2);
        }
    }

    public void RandNum()
    {
        ri = Random.Range(1, 51);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync health
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(attackDamage);
           // stream.SendNext(managerScript);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            attackDamage = (float)stream.ReceiveNext();
            //managerScript = (manager)stream.ReceiveNext();
        }
    }
}
