using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Scene
{
    private List<CustomShape3D> shapes = new List<CustomShape3D>();

    public void AddShape(CustomShape3D shape)
    {
        shapes.Add(shape);
    }

    public void Render(Camera camera)
    {
        var projection = camera.GetProjectionMatrix();
        var view = camera.GetViewMatrix();

        foreach (var shape in shapes)
        {
            shape.Render(projection, view);
        }
    }

    public void SaveToFile(string path)
    {
        var serializableShapes = new List<SerializableShape3D>();
        foreach (var shape in shapes)
        {
            serializableShapes.Add(shape.ToSerializable());
        }

        var json = JsonSerializer.Serialize(serializableShapes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public void LoadFromFile(string path)
    {
        shapes.Clear();

        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var serializableShapes = JsonSerializer.Deserialize<List<SerializableShape3D>>(json);

        foreach (var s in serializableShapes)
        {
            var shape = new CustomShape3D(s.Vertices.ToArray(), s.Indices.ToArray());
            shape.ModelMatrix = CustomShape3D.ConvertMatrix4x4ToOpenTK(s.ModelMatrix);
            AddShape(shape);
        }
    }
}
