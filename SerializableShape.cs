public class SerializableShape
{
    public float[] Vertices { get; set; } = new float[0]; // Inicialización para evitar null
    public uint[] Indices { get; set; } = new uint[0]; // Inicialización para evitar null
    public SerializableMatrix ModelMatrix { get; set; } = new SerializableMatrix(); // Inicialización para evitar null
}


public class SerializableMatrix
{
    public SerializableVector4 Row0 { get; set; }
    public SerializableVector4 Row1 { get; set; }
    public SerializableVector4 Row2 { get; set; }
    public SerializableVector4 Row3 { get; set; }
}

public class SerializableVector4
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public float W { get; set; }
}
