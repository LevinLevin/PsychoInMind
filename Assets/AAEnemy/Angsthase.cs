using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Angsthase : MonoBehaviour
{
    //vektoren
    private Vector3 dirToPlayer;
    private Vector3 newPos;

    private NavMeshAgent _agent;

    private GameObject Player;

    public float EnemyDistanceRun = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance > 50f)
        {
            gameObject.SetActive(false);
        }

        if (distance < EnemyDistanceRun)
        {
            dirToPlayer = transform.position + Player.transform.position;

            newPos = transform.position + dirToPlayer;

            _agent.SetDestination(newPos);
        }
    }
}
