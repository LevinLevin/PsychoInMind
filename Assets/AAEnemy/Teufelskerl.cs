using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teufelskerl : MonoBehaviour
{
    //vektoren
    private Vector3 dirToPlayer;
    private Vector3 newPos;

    private float PlayerPosition;
    public float EnemyDistanceChase = 4.0f;

    //für den schaden am Spieler
    public LayerMask whatIsPlayer;
    private bool alreadyAttacked = false;
    public bool inSightRange, inAttackRange;
    public float AttackRange;
    public float SightRange = 20f;
    private float timeBetweenAttacks = 1f;

    private NavMeshAgent _agent;
    private GameObject Player;

    public bool isChasing = false;

    bool turningRight;
    bool turningLeft;
    [SerializeField] private Vector3 rotation;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();

        StartCoroutine(Umschauen());
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        inAttackRange = Physics.CheckSphere(transform.position, AttackRange, whatIsPlayer);
        Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out RaycastHit spottetEnemy, SightRange);

        if(isChasing != false)
        {
            isChasing = false;
        }

        if(inSightRange == false && turningRight)
        {
            transform.Rotate(rotation * Time.deltaTime);
        }
        if(inSightRange == false && turningLeft)
        {
            transform.Rotate(-rotation * Time.deltaTime);
        }
        
        if(distance > 50f)
        {
            Destroy(gameObject);
        }

        if (distance < EnemyDistanceChase && spottetEnemy.transform != null && spottetEnemy.transform.tag == Player.transform.tag)
        {
            transform.LookAt(Player.transform);
            dirToPlayer = transform.position - Player.transform.position;
            newPos = transform.position - dirToPlayer;
            _agent.SetDestination(newPos);
            isChasing = true;
            inSightRange = true;
        }
        else
        {
            inSightRange = false;
        }

        if (inAttackRange && !alreadyAttacked)
        {
            AttackPlayer();
        }
    }

    public void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            //Decrease players health
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    IEnumerator Umschauen()
    {
        while(true)
        {
            int rotTime = Random.Range(3, 5); //die Zeit die es gedreht wird
            turningRight = true;
            yield return new WaitForSeconds(rotTime);
            turningRight = false;
            int waitTime = Random.Range(7, 11);
            yield return new WaitForSeconds(waitTime);
            rotTime = Random.Range(6, 10); //die Zeit die es gedreht wird
            turningLeft = true;
            yield return new WaitForSeconds(rotTime);
            turningLeft = false;
            waitTime = Random.Range(7, 11);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
