using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProgGraficaTareas
{
    public class Parte
    {


        public Shader shader;

        public String name { get; set; }

        public Punto origen { get; set; }

        public Matrix4 model = Matrix4.Identity;
        public Matrix4 rot = Matrix4.Identity;
        public Dictionary<string, Cara> vertices { get; set; }

        public Parte(Shader shader, String nombre, Punto origen)
        {
            this.shader = shader;
            this.name = nombre;
            this.vertices = new Dictionary<string, Cara>();// cuando se crea el objeto solo con el shader y el nombre
            this.origen = origen;
        }

        public Parte(Shader shader, String nombre, Dictionary<string, Cara> listcara)
        {
            this.shader = shader;
            this.name = nombre;
            this.vertices = new Dictionary<string, Cara>();

            foreach (KeyValuePair<string, Cara> k in listcara)
            {
                this.vertices.Add(k.Key, k.Value);
            }
        }

        public Parte()
        {
            this.name = "";
            this.vertices = new Dictionary<string, Cara>();// cuando se crea el objeto solo con el chaser y el nombre
        }

        public void add(string key, Cara cara)
        {
            vertices.Add(key, cara);
        }

        public String getName() {
            return this.name;
        }
        public void setShader(Shader shader)
        {
            this.shader = shader;
            foreach (KeyValuePair<string, Cara> k in this.vertices)
            {
                k.Value.setShader(shader);
            }
        }
      

        public String ArrayverticeTostring()
        {

            String listcaras = "";
            foreach (KeyValuePair<string, Cara> k in vertices)
            {
                listcaras += k.Value.getName() + " ";
               
            }
            Console.WriteLine(listcaras);
            return listcaras;
        }


        public void dibujar(Punto origen , Matrix4 modell)
        { 
            Punto origennew = origen.sum(origen, this.origen);

            foreach (KeyValuePair<string, Cara> k in vertices)
            {
                k.Value.dibujar(origennew,rot *  this.model * Matrix4.CreateTranslation(this.origen.x, this.origen.y, this.origen.z) * modell);
            }
         
        }

        public void escalar(float x, float y, float z)
        {
            this.model = this.model* Matrix4.CreateScale(x, y, z);
        }
        public void trasladar(float x, float y, float z)
        {
            this.model = this.model* Matrix4.CreateTranslation(x, y, z);
        }
        public void trasladarXYZ(float x, float y, float z)
        {
            this.model = Matrix4.CreateTranslation(x, y, z); 
        }
        public void rotar(float a ,float x,float y,float z)
        {
           // Console.WriteLine( "parte" + this.origen.x.ToString() + "x , " + this.origen.y.ToString() + "y , " + this.origen.z.ToString() + "z , ");

            if (x > 0) rot = rot * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(a));

            if (y > 0) rot = rot * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(a));

            if (z > 0) rot = rot * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(a));

        }

    }
}
