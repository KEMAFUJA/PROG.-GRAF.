using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Camera
{
    private Vector3 position;
    private float pitch = 0f;  // Ángulo vertical
    private float yaw = -90f;  // Ángulo horizontal
    private float speed = 2.5f; // Velocidad de movimiento
    private float sensitivity = 0.1f; // Sensibilidad de rotación

    public float AspectRatio { get; set; }

    public Camera(Vector3 position, float aspect)
    {
        this.position = position;
        AspectRatio = aspect;
    }

    public Matrix4 GetViewMatrix()
    {
        // Calculamos la dirección hacia donde está mirando la cámara
        var front = new Vector3(
            MathF.Cos(MathHelper.DegreesToRadians(yaw)) * MathF.Cos(MathHelper.DegreesToRadians(pitch)),
            MathF.Sin(MathHelper.DegreesToRadians(pitch)),
            MathF.Sin(MathHelper.DegreesToRadians(yaw)) * MathF.Cos(MathHelper.DegreesToRadians(pitch))
        );

        // La vista de la cámara es siempre mirando hacia el frente del objeto
        return Matrix4.LookAt(position, position + front.Normalized(), Vector3.UnitY);
    }

    public Matrix4 GetProjectionMatrix() =>
        Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), AspectRatio, 0.1f, 100f);

    public void Update(KeyboardState keyboard, float deltaTime)
    {
        var velocity = speed * deltaTime;
        var front = GetViewMatrix().Column2.Xyz.Normalized();
        var right = Vector3.Cross(front, Vector3.UnitY).Normalized();

        // Movimiento de la cámara usando teclas WASD
        if (keyboard.IsKeyDown(Keys.Q)) position -= front * velocity;
        if (keyboard.IsKeyDown(Keys.E)) position += front * velocity;
        if (keyboard.IsKeyDown(Keys.A)) position -= right * velocity;
        if (keyboard.IsKeyDown(Keys.D)) position += right * velocity;

        // Rotación de la cámara por teclado (yaw)
        if (keyboard.IsKeyDown(Keys.Z)) yaw -= sensitivity;
        if (keyboard.IsKeyDown(Keys.X)) yaw += sensitivity;

        // Rotación de la cámara por teclado (pitch)
        if (keyboard.IsKeyDown(Keys.W)) pitch += sensitivity;
        if (keyboard.IsKeyDown(Keys.S)) pitch -= sensitivity;

        // Restringir el ángulo de pitch para evitar que la cámara pase por encima o por debajo
        if (pitch > 89.0f) pitch = 89.0f;
        if (pitch < -89.0f) pitch = -89.0f;
    }
}
