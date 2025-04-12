using System.Collections.Generic;

namespace OpenTKProject
{
    public class Parte3D
    {
        public List<Face3D> Faces { get; set; } = new List<Face3D>();

        public void Draw()
        {
            foreach (var face in Faces)
                face.Draw();
        }
    }
}
