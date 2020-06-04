using UnityEngine;

public class Dog : Noncharacter {

    private readonly float maxSpeed = 3;
    private readonly float goalDistance = 3;

    protected new void Start() {
        base.Start();
    }

    private void Update() {

        float distX = WorldController.Instance.Player.GetLocationX() - GetLocationX();
        float distY = WorldController.Instance.Player.GetLocationY() - GetLocationY();
        Vector2 move = new Vector2(distX, distY);

        if (move.magnitude > goalDistance) {
            move.Normalize();

            animator.SetFloat("Speed", maxSpeed);
            animator.SetFloat("MoveX", move.x);
            animator.SetFloat("MoveY", move.y);

            Vector2 newPosition = rigidbody2d.position + move * maxSpeed * Time.deltaTime;
            rigidbody2d.MovePosition(newPosition);
        }
        else {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);

            rigidbody2d.velocity = new Vector2(0, 0);
        }
    }

    public override IEntity ObjectFromString(string info) {
        return this;
    }

    public override string ObjectToString() {
        return "";
    }
}
