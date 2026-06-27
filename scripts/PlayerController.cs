using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export]
    public float Speed = 220f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Zero;

        if (Input.IsKeyPressed(Key.A) || Input.IsKeyPressed(Key.Left))
            direction.X -= 1f;

        if (Input.IsKeyPressed(Key.D) || Input.IsKeyPressed(Key.Right))
            direction.X += 1f;

        if (Input.IsKeyPressed(Key.W) || Input.IsKeyPressed(Key.Up))
            direction.Y -= 1f;

        if (Input.IsKeyPressed(Key.S) || Input.IsKeyPressed(Key.Down))
            direction.Y += 1f;

        if (direction.Length() > 1f)
            direction = direction.Normalized();

        Velocity = direction * Speed;
        MoveAndSlide();
    }
}
