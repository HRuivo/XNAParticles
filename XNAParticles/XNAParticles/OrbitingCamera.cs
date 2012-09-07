using Microsoft.Xna.Framework;

public class OrbitingCamera
{
    public Matrix Projection { get; private set; }
    public Matrix View { get; private set; }
    public Matrix ViewProj { get; private set; }

    public Vector3 Position { get; set; }

    public float Pitch { get; set; }
    public float Yaw { get; set; }

    public float NearPlane { get; set; }
    public float FarPlane { get; set; }
    public float AspectRatio { get; set; }
    public float FOV { get; set; }


    private Vector3 target;
    public Vector3 Target
    {
        get
        {
            return target = Position +
                Vector3.Transform(
                    Vector3.Forward,
                    Matrix.CreateRotationX(Pitch) *
                    Matrix.CreateRotationY(Yaw));
        }
        set { target = value; }
    }

    private Vector3 upVector;
    public Vector3 UpVector
    {
        get
        {
            return upVector = Vector3.Transform(
                Vector3.Up,
                Matrix.CreateRotationX(Pitch) *
                Matrix.CreateRotationY(Yaw));
        }
        set { upVector = value; }
    }

    private Vector3 sideVector;
    public Vector3 SideVector
    {
        get
        {
            return sideVector = Vector3.Transform(
                Vector3.Right,
                Matrix.CreateRotationX(Pitch) *
                Matrix.CreateRotationY(Yaw));
        }
        set { SideVector = value; }
    }

    private Vector3 forward;
    public Vector3 Forward
    {
        get
        {
            return forward = Vector3.Transform(
                Vector3.Forward,
                Matrix.CreateRotationX(Pitch) *
                Matrix.CreateRotationY(Yaw));
        }
    }

    public Matrix CameraRotation
    {
        get { return Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw); }
    }

    public Vector3 Offset { get; set; }



    public OrbitingCamera(float aspectRatio)
    {
        this.Position = Vector3.Zero;
        this.Target = Vector3.Zero;
        this.UpVector = Vector3.Up;

        this.NearPlane = 1.0f;
        this.FarPlane = 10000.0f;
        this.FOV = MathHelper.PiOver4;
        this.AspectRatio = aspectRatio;

        UpdateProjectionMatrix();
    }

    public void Update(float dt)
    {
        Vector3 cameraRotatedTarget = Vector3.Transform(Vector3.Forward, CameraRotation);
        Vector3 cameraFinalTarget = Position + cameraRotatedTarget;

        Vector3 cameraRotatedUpVector = Vector3.Transform(Vector3.Up, CameraRotation);
        Vector3 cameraFinalUpVector = Position + cameraRotatedUpVector;

        Position = Vector3.Transform(Offset, CameraRotation);

        Target = cameraFinalTarget;
        UpVector = cameraRotatedUpVector;

        UpdateViewMatrix();
        UpdateProjectionMatrix();
    }

    public void UpdateViewMatrix()
    {
        this.View =
            Matrix.CreateLookAt(
                Position, Target, UpVector);

        ViewProj = View * Projection;
    }

    public void UpdateProjectionMatrix()
    {
        this.Projection =
            Matrix.CreatePerspectiveFieldOfView(
                FOV, AspectRatio, NearPlane, FarPlane);

        ViewProj = View * Projection;
    }
}
