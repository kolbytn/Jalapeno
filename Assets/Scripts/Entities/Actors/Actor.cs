using UnityEngine;
using UnityEngine.AI;

public abstract class Actor : WorldEntity {

    protected Animator animator;
    protected Rigidbody2D rigidbody2d;
    protected NavMeshAgent navAgent;

    protected new void Start() {
        base.Start();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }
}
