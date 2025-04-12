using System.Collections.Generic;

namespace OpenTKProject
{
    public class Scene
    {
        public List<Objeto3D> Objetos { get; set; } = new List<Objeto3D>();

        public void Draw()
        {
            foreach (var obj in Objetos)
                obj.Draw();
        }
    }
}
