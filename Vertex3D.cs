using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class Vertex3D
    {
        public Vector3 Position { get; set; }

        public Vertex3D(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }

        public Vertex3D(Vector3 pos)
        {
            Position = pos;
        }
    }
}
