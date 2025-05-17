using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;


namespace ProgGraficaTareas.animacion
{
    public class Animacion
    {
        public String nombre { get; set; }
        public float time { get; set; }
        public float speed { get; set; }

        public bool run;

        public bool stop;

        public Escena escenaani;
        public Dictionary<String, Accion> acciones { get; set; }

        public Animacion(String nomb, float time, float speed, Escena escenaani)
        {
            this.nombre = nomb;
            this.time = time;
            this.speed = speed;
            this.run = false;
            this.stop = false;
            this.acciones = new Dictionary<string, Accion>();
            this.escenaani = escenaani;
        }

        public void add(String key, Accion accion)
        {
            this.acciones.Add(key, accion);
        }

        public void Animar(Escena escena)
        {
            //escena.trasladar(0.001f, 0.001f, 0.001f);
            escena.rotar(5, 1.0f, 0.0f, 0.0f);
        }

        public void IniciarAnimacion()
        {
            foreach (KeyValuePair<string, Accion> k in acciones)
            {
                Accion accion = k.Value;

                if (accion.parte != null)
                {
                    AnimarByInstru2(accion.parte, accion);
                }
                else if (accion.objeto != null)
                {
                    AnimarByInstruObjeto(accion.objeto, accion);
                }
                else
                {
                    Console.WriteLine($"[WARN] Acción '{accion.nombre}' no tiene Parte ni Objeto.");
                }

                Thread.Sleep(30);
                Console.WriteLine("hilo corriendo desde actions");

                if (stop) { break; }
            }
        }


        public void IniciarAnimacionsigsag(Objeto auto, int distancia, int angulodir, string direccion)
        {

            //linea recta
            for (int i = 0; i < 200; i++)
            {
                auto.origen.x += 0.01f; // Sumar 0.2 en X
                auto.trasladarXYZ(auto.origen.x, auto.origen.y, auto.origen.z); // Mover al nuevo punto
                Thread.Sleep(30); // Opcional, para visualizar el movimiento
               
                auto.get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
                auto.get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);
            }

           // Thread.Sleep(1000);


            //---------- hace una circunferencia de 180 grados-----------------
            float R = 5f; // Radio de la curva
            float x = auto.origen.x; // Debería ser 0
            float z = auto.origen.z; // Debería ser 0
            float y = auto.origen.y;

            // Centro del círculo a la derecha del objeto (eje X positivo)
            float xc = x ;
            float zc = z + R;

            int pasos = 180 *2 ;  //porque avanza de a 0.5 grados , para 180 grados - 180*2
            float deltaThetaGrados = 0.5f;
            float anguloActualGrados = 270f; // Empieza desde 180° para ir de atrás hacia adelante en el sentido horario

            float xnuevo = 0f;
            float znuevo= 0f;
                
            auto.get("r22").rotar(-45, 0.0f, 1.0f, 0.0f);
            auto.get("r33").rotar(-45, 0.0f, 1.0f, 0.0f);

            for (int i = 0; i < pasos; i++)
            {
                float rad = anguloActualGrados * MathF.PI / 180f;

                xnuevo = xc + R * MathF.Cos(rad);
                znuevo = zc + R * MathF.Sin(rad);

                auto.trasladarXYZ(xnuevo, y, znuevo);
                auto.rotar(-0.6f, 0.0f, 1.0f, 0.0f);
                auto.get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
                auto.get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);

                anguloActualGrados += 0.5f; // sentido horario
                Thread.Sleep(30);
            }
            auto.get("r22").rotar(45, 0.0f, 1.0f, 0.0f);
            auto.get("r33").rotar(45, 0.0f, 1.0f, 0.0f);






            //linea recta de vuelta
            for (int i = 0; i < 300; i++)
            {
                xnuevo -= 0.01f; // Sumar 0.2 en X
                auto.trasladarXYZ(xnuevo, auto.origen.y, znuevo); // Mover al nuevo punto
                Thread.Sleep(30); // Opcional, para visualizar el movimiento
                auto.get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
                auto.get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);
            }
        }



        public void AnimarByInstru(Escena escena, Accion action)
        {
            String trans = action.transformacion.ToString();
            switch (trans)
            {
                case "trasladar":
                    escena.trasladar(action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acction trasladar");
                    break;
                case "rotar":
                    escena.rotar(action.angulo, action.pos.x, action.pos.y, action.pos.z);
                //    Console.WriteLine("acction rotar");
                    break;
                case "escalar":
                    escena.escalar(action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acction escalar");
                    break;
                default:
                    Console.WriteLine("default");
                    break;

            }


        }
        public void AnimarByInstru2(Parte escena, Accion action)
        {
            String trans = action.transformacion.ToString();
            switch (trans)
            {
                case "trasladar":
                    escena.trasladar(action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acction trasladar");
                    break;
                case "rotar":
                    escena.rotar(action.angulo, action.pos.x, action.pos.y, action.pos.z);
                 //   Console.WriteLine("acction rotar");
                    break;
                case "escalar":
                    escena.escalar(action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acction escalar");
                    break;
                default:
                    Console.WriteLine("default");
                    break;

            }


        }
        public void AnimarByInstruObjeto(Objeto obj, Accion action)
        {
            string trans = action.transformacion;

            switch (trans)
            {
                case "trasladar":
                    obj.trasladar(action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acción trasladar (objeto)");
                    break;

                case "rotar":
                    obj.rotar(action.angulo, action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acción rotar (objeto)");
                    break;

                case "escalar":
                    obj.escalar(action.pos.x, action.pos.y, action.pos.z);
                    Console.WriteLine("acción escalar (objeto)");
                    break;

                default:
                    Console.WriteLine("acción desconocida (objeto)");
                    break;
            }
        }
    }
}
