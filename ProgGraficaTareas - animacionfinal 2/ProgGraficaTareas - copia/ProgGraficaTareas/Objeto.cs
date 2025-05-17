using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgGraficaTareas
{
    public class Objeto
    {


        public Shader shader;

        public String name { get; set; }

        public Punto origen { get; set; }

        public Matrix4 model = Matrix4.Identity;

        public Matrix4 rot = Matrix4.Identity;
        public Dictionary<string, Parte> vertices { get; set; }
        public Objeto(Shader shader, String nombre, Punto origen)
        {
            this.shader = shader;
            this.name = nombre;
            this.vertices = new Dictionary<string, Parte>();// cuando se crea el objeto solo con el chaser y el nombre
            this.origen = origen;
        }

        public Objeto(Shader shader, String nombre, Dictionary<string, Parte> listparte)
        {
            this.shader = shader;
            this.name = nombre;
            this.vertices = new Dictionary<string, Parte>();

            foreach (KeyValuePair<string, Parte> k in listparte)
            {
                this.vertices.Add(k.Key, k.Value);
            }
        }

        public Objeto()
        {
            this.name = "";
            this.vertices = new Dictionary<string, Parte>();// cuando se crea el objeto solo con el chaser y el nombre
        }
        public String getName()
        {
            return this.name;
        }
        public void setShader(Shader shader)
        {
            this.shader = shader;
            foreach (KeyValuePair<string, Parte> k in this.vertices)
            {
                k.Value.setShader(shader);
            }
        }
        public Parte get(String key)
        {
            foreach (KeyValuePair<string, Parte> k in this.vertices)
            {
                if (k.Key == key) { return k.Value; }
            }
            return this.vertices.First().Value;
        }
        public void add(string key, Parte parte)
        {
            vertices.Add(key, parte);
        }
        public void ArrayverticeTostring()
        {

            String listcaras = "";
            foreach (KeyValuePair<string, Parte> k in vertices)
            {
                listcaras += k.Value.ArrayverticeTostring() + " ";

            }
            Console.WriteLine(listcaras);

        }




        public void dibujar(Punto origen, Matrix4 modell)
        {
            Punto origennew = origen.sum(origen ,this.origen);

           // this.model =  ;

            foreach (KeyValuePair<string, Parte> k in vertices)
            {
                k.Value.dibujar(origennew,rot * this.model * Matrix4.CreateTranslation(this.origen.x, this.origen.y, this.origen.z) * modell);
            }

        }
        public void escalar(float x, float y, float z)
        {
            this.model = this.model *Matrix4.CreateScale(x, y, z);
        }
        public void trasladar(float x, float y, float z)
        {
            this.model = this.model * Matrix4.CreateTranslation(x, y, z);
        }
        public void trasladarXYZ(float x, float y, float z)
        {
            this.model = Matrix4.CreateTranslation(x, y, z);
        }
        public void rotar(float a, float x, float y, float z)
        {
            Console.WriteLine( "objeto" + this.origen.x.ToString() + "x , " + this.origen.y.ToString() + "y , " + this.origen.z.ToString() + "z , ");

            if (x > 0) rot = rot * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(a));

            if (y > 0) rot = rot * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(a));

            if (z > 0) rot = rot * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(a));

        }
    }
}