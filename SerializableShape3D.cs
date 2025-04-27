using System.Collections.Generic;
using System.Numerics;

public class SerializableShape3D
{
    public List<float> Vertices { get; set; }
    public List<uint> Indices { get; set; }
    public Matrix4x4 ModelMatrix { get; set; }
}
