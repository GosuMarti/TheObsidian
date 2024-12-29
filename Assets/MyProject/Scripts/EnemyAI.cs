using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Le joueur à suivre
    public float detectionRange = 10f; // Distance à laquelle l'ennemi détecte le joueur

    private NavMeshAgent agent; 
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath(); // Arrête de poursuivre si le joueur est hors de portée
        }
        float currentSpeed = agent.velocity.magnitude; // Vitesse actuelle de l'agent
        animator.SetFloat("Speed", currentSpeed);

    }
}
