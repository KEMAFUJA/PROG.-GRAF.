using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgGraficaTareas
{
    public class Escena
    {


        public Shader shader;

        public String name { get; set; }

        public Punto origen { get; set; }

        public Matrix4 model = Matrix4.Identity;

        public Matrix4 rot = Matrix4.Identity;



        public Dictionary<string, Objeto> vertices { get; set; }
        public Escena(Shader shader, String nombre, Punto origen)
        {
            this.shader = shader;
            this.name = nombre;
            this.vertices = new Dictionary<string, Objeto>();// cuando se crea el objeto solo con el chaser y el nombre
            this.origen = origen;
        }

        public Escena()
        {
            this.name = "";
            this.vertices = new Dictionary<string, Objeto>();// cuando se crea el objeto solo con el chaser y el nombre
        }

        public void setShader(Shader shader) { 
        this.shader= shader;
            foreach (KeyValuePair<string, Objeto> k in this.vertices)
            {
                k.Value.setShader(shader);
            }
        }

        public Escena(Shader shader, String nombre, Dictionary<string, Objeto> listparte)
        {
            this.shader = shader;
            this.name = nombre;
            this.vertices = new Dictionary<string, Objeto>();

            foreach (KeyValuePair<string, Objeto> k in listparte)
            {
                this.vertices.Add(k.Key, k.Value);
            }
        }
        public String getName() { 
        return this.name;
        }
        public Objeto get(String key)
        {
            foreach (KeyValuePair<string, Objeto> k in this.vertices)
            {
                if (k.Key == key) { return k.Value; }
            }
            return this.vertices.First().Value;
        }

        public void add(string key, Objeto parte)
        {
            vertices.Add(key, parte);
        }
        public void ArrayverticeTostring()
        {
            foreach (KeyValuePair<string, Objeto> k in vertices)
            {
                k.Value.ArrayverticeTostring();

            }
            Console.WriteLine("Lista de objetos-partes-caras");

        }




        public void dibujar()
        {
           
            foreach (KeyValuePair<string, Objeto> k in vertices)
            {
                k.Value.dibujar(this.origen, rot * this.model * Matrix4.CreateTranslation(this.origen.x, this.origen.y, this.origen.z) );
            }

        }
        public void escalar(float x, float y, float z)
        {
            this.model = this.model *  Matrix4.CreateScale(x, y, z);
        }
        public void trasladar(float x, float y, float z)
        {
            this.model = this.model   * Matrix4.CreateTranslation(x, y, z);
        }
        public void trasladarXYZ(float x, float y, float z)
        {
            this.model = Matrix4.CreateTranslation(x, y, z);
        }
        public void rotartodos(float a, float x, float y, float z)
        {
            foreach (KeyValuePair<string, Objeto> k in vertices)
            {
                k.Value.rotar(a, x, y, z);
            }
        }
        public void rotar(float a, float x, float y, float z)
        {
            Console.WriteLine( "escena" + this.origen.x.ToString() + "x , " + this.origen.y.ToString() + "y , " + this.origen.z.ToString() + "z , ");

            if (x > 0) rot = rot * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(a));

            if (y > 0) rot = rot * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(a));

            if (z > 0) rot = rot * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(a));

        }
    }
}