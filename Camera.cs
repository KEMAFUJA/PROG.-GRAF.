using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    public Vector3 Position { get; private set; }
    public Vector3 Front { get; private set; } = -Vector3.UnitZ;
    public Vector3 Up { get; private set; } = Vector3.UnitY;
    public float Yaw { get; private set; } = -90f;
    public float Pitch { get; private set; } = 0f;
    public float Speed { get; set; } = 2.5f;
    public float Sensitivity { get; set; } = 0.1f;

    private bool firstMouse = true;
    private Vector2 lastMousePos;

    public Camera(Vector3 position)
    {
        Position = position;
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + Front, Up);
    }

    public void ProcessKeyboard(KeyboardState input, float deltaTime)
    {
        float velocity = Speed * deltaTime;
        if (input.IsKeyDown(Keys.W))
            Position += Front * velocity;
        if (input.IsKeyDown(Keys.S))
            Position -= Front * velocity;
        if (input.IsKeyDown(Keys.A))
            Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * velocity;
        if (input.IsKeyDown(Keys.D))
            Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * velocity;
    }

    public void ProcessMouse(float xOffset, float yOffset)
    {
        xOffset *= Sensitivity;
        yOffset *= Sensitivity;

        Yaw += xOffset;
        Pitch -= yOffset;
        Pitch = MathHelper.Clamp(Pitch, -89f, 89f);

        UpdateCameraVectors();
    }

    private void UpdateCameraVectors()
    {
        Vector3 front;
        front.X = MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
        front.Y = MathF.Sin(MathHelper.DegreesToRadians(Pitch));
        front.Z = MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch));
        Front = Vector3.Normalize(front);
    }
}
