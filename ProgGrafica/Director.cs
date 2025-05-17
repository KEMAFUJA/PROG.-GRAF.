using ProgGraficaTareas.animacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgGraficaTareas
{
    public class Director
    {
        public List<Animacion> animaciones { get; set; }
        public bool pausa;
        public bool detener;

        Thread animarthread;
        public Director() {
            this.animaciones =new List<Animacion>();
            this.pausa = false;
            this.detener = false;
        }

        public void add( Animacion ani) {
            animaciones.Add(ani);
        }

        public void start() {

                this.animarthread = new Thread(() => dirAccion());
                this.animarthread.Start();
              
        }

        public void dirAccion() {
            int time = 0;
            int j = 0;
            int i = animaciones.Count();
            while (true) {
                if (animaciones[j].time > 0 && !animaciones[j].run) {
                  
                    animaciones[j].run = true;
                  
                 //   animaciones[j].IniciarAnimacionsigsag(animaciones[j].escenaani.get("6"), 5, 45, "izquierda");

                    animaciones[j].IniciarAnimacion();
                }
                Thread.Sleep(1000);
                time++;
              //  Console.WriteLine("tiempo de animacion " +time.ToString());


                if (time >= animaciones[j].time) {
                    animaciones[j].stop=true;
                }
                j++;  
                if (j >= i) { j = 0; }
              
            }

        }

    }
}
