using UnityEngine;

public class Dog : Noncharacter {

    private readonly float goalDistance = 3;

    protected new void Start() {
        base.Start();

        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
    }

    private void Update() {

        float distX = WorldController.Instance.Player.GetLocationX() - GetLocationX();
        float distY = WorldController.Instance.Player.GetLocationY() - GetLocationY();
        Vector2 distance = new Vector2(distX, distY);

        if (distance.magnitude > goalDistance) {
            navAgent.isStopped = false;
            navAgent.SetDestination(new Vector2(WorldController.Instance.Player.GetLocationX(), WorldController.Instance.Player.GetLocationY()));
        }
        else {
            navAgent.isStopped = true;
            rigidbody2d.velocity = new Vector2(0, 0);
        }

        animator.SetFloat("Speed", navAgent.velocity.magnitude);
        animator.SetFloat("MoveX", navAgent.velocity.x);
        animator.SetFloat("MoveY", navAgent.velocity.y);
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }

    public override string ObjectToString() {
        return "";
    }
}
