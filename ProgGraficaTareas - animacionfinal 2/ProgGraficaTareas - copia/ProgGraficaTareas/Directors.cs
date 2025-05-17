using ProgGraficaTareas.animacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgGraficaTareas
{
    public class Directors
    {

        public List<Animacion> animaciones { get; set; }
        public bool pausa = false;
        private CancellationTokenSource? cts; // El ? es opcional si usas C# 8.0+


        public Directors()
        {
            animaciones = new List<Animacion>();
        }

        public void Add(Animacion ani)
        {
            lock (animaciones)
            {
                animaciones.Add(ani);
            }
        }

        public void Start()
        {
            cts = new CancellationTokenSource();
            Task.Run(() => EjecutarAnimacionesAsync(cts.Token));
        }

        public void Stop()
        {
            cts?.Cancel();
        }

        private async Task EjecutarAnimacionesAsync(CancellationToken token)
        {
            int j = 0;

            while (!token.IsCancellationRequested)
            {
                if (pausa)
                {
                    await Task.Delay(100, token);
                    continue;
                }

                Animacion ani;
                lock (animaciones)
                {
                    if (animaciones.Count == 0) return;
                    ani = animaciones[j];
                }

                if (ani.time > 0 && !ani.run)
                {
                    ani.run = true;
                    //  ani.IniciarAnimacionsigsag(ani.escenaani.get("6"), 5, 45, "izquierda");
                    animaciones[j].IniciarAnimacion();
                }

                await Task.Delay(1000, token); // Esperar sin bloquear

                if (++j >= animaciones.Count)
                {
                    j = 0;
                }
            }
        }
    }
}
