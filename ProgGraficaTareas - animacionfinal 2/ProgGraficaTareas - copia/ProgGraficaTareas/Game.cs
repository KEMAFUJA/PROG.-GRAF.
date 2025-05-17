using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.IO;
using OpenTK.Windowing.GraphicsLibraryFramework; 
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using ProgGraficaTareas.animacion;

namespace ProgGraficaTareas
{
    public class Game : GameWindow
    {
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            this.VSync = VSyncMode.On;
        }
     
        Shader shader;
        Thread animarthread;

        bool mythreadbool = false;
        Animacion animacion1;

        private int vertexBufferObject;
        private int vertexArrayObject;
        private int elementBufferObject;

        private Matrix4 projection;
        private Matrix4 view;
        private Matrix4 model;


        Escena escena1;

        Escena escenaDeserializada;
        Escena escenaDeserializada2;
        Escena escenaprueva;

        // Parte parterotar;


        // Variables del auto (arriba de todo en la clase)
        float angulo = 0.0f;
        float posX = 0.0f;
        float posY = 0.0f;
        float posZ = 0.0f;
        float velocidad = 0.01f;


        private Camera _camera;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private double _time;

        bool animated = false;
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

            vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);

            shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");

         
           projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(20.0f), Size.X / (float)Size.Y, 0.1f, 20.0f);
         
            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
          // projection = Matrix4.CreateOrthographic(2.0f, 2.0f, 0.1f, 100.0f); // Tamaño de la proyección ortogonal en el plano XY
            model = Matrix4.Identity * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(0.0f));


            shader.SetMatrix4("model", model);
            shader.SetMatrix4("projection", projection);
            shader.SetMatrix4("view", view);

            //     shader.Use();
            //     var move = Matrix4.Identity;
            //     move = move * Matrix4.CreateTranslation(0f, 0f, 0f);
            //      shader.SetMatrix4("origen", move);

           

            //###########################################################################
            Punto origenescena = new Punto(0.0f, 0.0f, 0.0f);

            escenaprueva = new Escena(shader, "escenaprueva", origenescena);




            //cuadrado
            Punto origencara = new Punto(0.0f, 0.0f, 0.0f); // centro de masa

            // Escala al triple (3 veces más grande)
            Cara caraeqdelante2rota = new Cara(shader, "Base quad", origencara);
            caraeqdelante2rota.add("1", new Punto(-0.05f * 3, -0.1f * 3, 0.0f * 3));
            caraeqdelante2rota.add("2", new Punto(0.05f * 3, -0.1f * 3, 0.0f * 3));
            caraeqdelante2rota.add("3", new Punto(0.05f * 3, 0.1f * 3, 0.0f * 3));
            caraeqdelante2rota.add("4", new Punto(-0.05f * 3, 0.1f * 3, 0.0f * 3));
            caraeqdelante2rota.color = new Vector3(1f, 1f, 0f); // Amarillo



            Cara caraeqatras2rota = new Cara(shader, "Base quad 2", origencara);
            caraeqatras2rota.add("1", new Punto(-0.05f * 3, -0.1f * 3, -0.05f * 3));
            caraeqatras2rota.add("2", new Punto(0.05f * 3, -0.1f * 3, -0.05f * 3));
            caraeqatras2rota.add("3", new Punto(0.05f * 3, 0.1f * 3, -0.05f * 3));
            caraeqatras2rota.add("4", new Punto(-0.05f * 3, 0.1f * 3, -0.05f * 3));
            caraeqatras2rota.color = new Vector3(1f, 1f, 0f); // Amarillo


            Cara carasupp2rota = new Cara(shader, "superior", origencara);
            carasupp2rota.add("1", new Punto(0.05f * 3, 0.1f * 3, 0.0f * 3));
            carasupp2rota.add("2", new Punto(-0.05f * 3, 0.1f * 3, 0.0f * 3));
            carasupp2rota.add("4", new Punto(-0.05f * 3, 0.1f * 3, -0.05f * 3));
            carasupp2rota.add("3", new Punto(0.05f * 3, 0.1f * 3, -0.05f * 3));
            carasupp2rota.color = new Vector3(1f, 1f, 0f); // Amarillo

            Cara carainf2rota = new Cara(shader, "inferior", origencara);
            carainf2rota.add("1", new Punto(-0.05f * 3, -0.1f * 3, 0.0f * 3));
            carainf2rota.add("2", new Punto(0.05f * 3, -0.1f * 3, 0.0f * 3));
            carainf2rota.add("4", new Punto(0.05f * 3, -0.1f * 3, -0.05f * 3));
            carainf2rota.add("3", new Punto(-0.05f * 3, -0.1f * 3, -0.05f * 3));
            carainf2rota.color = new Vector3(1f, 1f, 0f); // Amarillo


            Punto origenparterota = new Punto(0.0f, 0.0f, 0.0f);               
            Parte parterotar = new Parte(shader, "parte123", origenparterota);  //Inicio 1


            parterotar.add("11", carainf2rota);
            parterotar.add("22", carasupp2rota);
            parterotar.add("33", caraeqatras2rota);
            parterotar.add("44", caraeqdelante2rota);


          
            Punto origenobjeto = new Punto(0.5f, 0.0f, 0.0f);
            Objeto objetopractica = new Objeto(shader, "objeto inicio", origenobjeto);   //Inicio solo derecha
            objetopractica.add("1", parterotar);  //Inicio

            //escenaprueva.add("1", objetopractica);   //Inicio


            // Definimos las dimensiones -----------------------TRONCO DEL ARBOL-----------------------------------
            float ancho = 0.5f;  // Ancho del tronco
            float alto = 3f;   // Alto del tronco
            float profundidad = 0.25f;  // Profundidad del tronco

            // Centro de masa (por defecto en 0,0,0)
            Punto origencaraarbol = new Punto(0.0f, 0.0f, 0.0f);

            // Cara del tronco (parte frontal del tronco)
            Cara troncoFrontal = new Cara(shader, "frente tronco", origencaraarbol);
            troncoFrontal.add("1", new Punto(-ancho / 2, -0.7f / 2, profundidad));
            troncoFrontal.add("2", new Punto(ancho / 2, -0.7f / 2, profundidad));
            troncoFrontal.add("3", new Punto(ancho / 2, alto / 2, profundidad));
            troncoFrontal.add("4", new Punto(-ancho / 2, alto / 2, profundidad));
            troncoFrontal.color = new Vector3(0.6f, 0.3f, 0.1f); // Marrón (como la corteza)

            // Cara trasera del tronco
            Cara troncoTrasero = new Cara(shader, "detras tronco", origencaraarbol);
            troncoTrasero.add("1", new Punto(-ancho / 2, -0.7f / 2, -profundidad));
            troncoTrasero.add("2", new Punto(ancho / 2, -0.7f / 2, -profundidad));
            troncoTrasero.add("3", new Punto(ancho / 2, alto / 2, -profundidad));
            troncoTrasero.add("4", new Punto(-ancho / 2, alto / 2, -profundidad));
            troncoTrasero.color = new Vector3(0.6f, 0.3f, 0.1f); // Marrón (como la corteza)

            // Cara izquierda del tronco
            Cara troncoIzquierda = new Cara(shader, "izquierda tronco", origencaraarbol);
            troncoIzquierda.add("1", new Punto(-ancho / 2, -0.7f / 2, profundidad));
            troncoIzquierda.add("2", new Punto(-ancho / 2, -0.7f / 2, -profundidad));
            troncoIzquierda.add("3", new Punto(-ancho / 2, alto / 2, -profundidad));
            troncoIzquierda.add("4", new Punto(-ancho / 2, alto / 2, profundidad));
            troncoIzquierda.color = new Vector3(0.6f, 0.3f, 0.1f); // Marrón (como la corteza)

            // Cara derecha del tronco
            Cara troncoDerecha = new Cara(shader, "derecha tronco", origencaraarbol);
            troncoDerecha.add("1", new Punto(ancho / 2, -0.7f / 2, profundidad));
            troncoDerecha.add("2", new Punto(ancho / 2, -0.7f / 2, -profundidad));
            troncoDerecha.add("3", new Punto(ancho / 2, alto / 2, -profundidad));
            troncoDerecha.add("4", new Punto(ancho / 2, alto / 2, profundidad));
            troncoDerecha.color = new Vector3(0.6f, 0.3f, 0.1f); // Marrón (como la corteza)

            // Crear la parte con las caras (tronco del árbol)
            Punto origenParterotaarbol = new Punto(0.0f, 0.0f, 0.0f);
            Parte troncoParte = new Parte(shader, "tronco del arbol", origenParterotaarbol);  // Parte principal del tronco

            troncoParte.add("frente", troncoFrontal);
            troncoParte.add("detras", troncoTrasero);
            troncoParte.add("izquierda", troncoIzquierda);
            troncoParte.add("derecha", troncoDerecha);

            // Crear el objeto completo que representa el árbol
            Punto origenObjeto = new Punto(5.0f, 0.0f, 5.0f);
            Objeto objetoArbol = new Objeto(shader, "arbol completo", origenObjeto);
            objetoArbol.add("tronco", troncoParte);  // Añadir el tronco con sus caras


            // Dimensiones de las hojas --------------------------HOJAS DEL ARBOL------------------------------#####
            float anchoHojas = 1.2f;
            float altoHojas = 1f;
            float profundidadHojas = 0.6f;

            // Centro en base al tronco
            Punto origenHojas = new Punto(0.0f, 1f, 0.0f);

            // Cara frontal de las hojas
            Cara hojasFrontal = new Cara(shader, "frente hojas", origenHojas);
            hojasFrontal.add("1", new Punto(-anchoHojas / 2, -altoHojas / 2, profundidadHojas));
            hojasFrontal.add("2", new Punto(anchoHojas / 2, -altoHojas / 2, profundidadHojas));
            hojasFrontal.add("3", new Punto(anchoHojas / 2, altoHojas / 2, profundidadHojas));
            hojasFrontal.add("4", new Punto(-anchoHojas / 2, altoHojas / 2, profundidadHojas));
            hojasFrontal.color = new Vector3(0.0f, 0.6f, 0.0f); // Verde

            // Cara trasera de las hojas
            Cara hojasTrasera = new Cara(shader, "detras hojas", origenHojas);
            hojasTrasera.add("1", new Punto(-anchoHojas / 2, -altoHojas / 2, -profundidadHojas));
            hojasTrasera.add("2", new Punto(anchoHojas / 2, -altoHojas / 2, -profundidadHojas));
            hojasTrasera.add("3", new Punto(anchoHojas / 2, altoHojas / 2, -profundidadHojas));
            hojasTrasera.add("4", new Punto(-anchoHojas / 2, altoHojas / 2, -profundidadHojas));
            hojasTrasera.color = new Vector3(0.0f, 0.6f, 0.0f);

            // Cara izquierda de las hojas
            Cara hojasIzquierda = new Cara(shader, "izquierda hojas", origenHojas);
            hojasIzquierda.add("1", new Punto(-anchoHojas / 2, -altoHojas / 2, profundidadHojas));
            hojasIzquierda.add("2", new Punto(-anchoHojas / 2, -altoHojas / 2, -profundidadHojas));
            hojasIzquierda.add("3", new Punto(-anchoHojas / 2, altoHojas / 2, -profundidadHojas));
            hojasIzquierda.add("4", new Punto(-anchoHojas / 2, altoHojas / 2, profundidadHojas));
            hojasIzquierda.color = new Vector3(0.0f, 0.6f, 0.0f);

            // Cara derecha de las hojas
            Cara hojasDerecha = new Cara(shader, "derecha hojas", origenHojas);
            hojasDerecha.add("1", new Punto(anchoHojas / 2, -altoHojas / 2, profundidadHojas));
            hojasDerecha.add("2", new Punto(anchoHojas / 2, -altoHojas / 2, -profundidadHojas));
            hojasDerecha.add("3", new Punto(anchoHojas / 2, altoHojas / 2, -profundidadHojas));
            hojasDerecha.add("4", new Punto(anchoHojas / 2, altoHojas / 2, profundidadHojas));
            hojasDerecha.color = new Vector3(0.0f, 0.6f, 0.0f);

            // Crear la parte de hojas
            Parte hojasParte = new Parte(shader, "hojas del arbol", origenHojas);
            hojasParte.add("frente", hojasFrontal);
            hojasParte.add("detras", hojasTrasera);
            hojasParte.add("izquierda", hojasIzquierda);
            hojasParte.add("derecha", hojasDerecha);

            // Añadir las hojas al objeto árbol
            objetoArbol.add("hojas", hojasParte);



            escenaprueva.add("arbol", objetoArbol);   // Añadir el objeto del árbol a la escena


            //-------------------------------------PERROOOOOOOO----------------------------

            // Dimensiones del cuerpo del perro 
            float anchoPerro = 0.6f;
            float altoPerro = 0.4f;
            float profundidadPerro = 0.1f;

            // Centro de masa para el perro
            Punto origenCaraPerro = new Punto(0.0f, 0.0f, 0.0f);

            // Colores marrones (igual que el tronco)
            Vector3 colorMarron = new Vector3(0.6f, 0.3f, 0.1f);

            // Cara frontal del perro
            Cara perroFrontal = new Cara(shader, "frente perro", origenCaraPerro);
            perroFrontal.add("1", new Punto(-anchoPerro / 2, -altoPerro / 2, profundidadPerro));
            perroFrontal.add("2", new Punto(anchoPerro / 2, -altoPerro / 2, profundidadPerro));
            perroFrontal.add("3", new Punto(anchoPerro / 2, altoPerro / 2, profundidadPerro));
            perroFrontal.add("4", new Punto(-anchoPerro / 2, altoPerro / 2, profundidadPerro));
            perroFrontal.color = colorMarron;

            // Cara trasera del perro
            Cara perroTrasera = new Cara(shader, "detras perro", origenCaraPerro);
            perroTrasera.add("1", new Punto(-anchoPerro / 2, -altoPerro / 2, -profundidadPerro));
            perroTrasera.add("2", new Punto(anchoPerro / 2, -altoPerro / 2, -profundidadPerro));
            perroTrasera.add("3", new Punto(anchoPerro / 2, altoPerro / 2, -profundidadPerro));
            perroTrasera.add("4", new Punto(-anchoPerro / 2, altoPerro / 2, -profundidadPerro));
            perroTrasera.color = colorMarron;

            // Cara izquierda del perro
            Cara perroIzquierda = new Cara(shader, "izquierda perro", origenCaraPerro);
            perroIzquierda.add("1", new Punto(-anchoPerro / 2, -altoPerro / 2, profundidadPerro));
            perroIzquierda.add("2", new Punto(-anchoPerro / 2, -altoPerro / 2, -profundidadPerro));
            perroIzquierda.add("3", new Punto(-anchoPerro / 2, altoPerro / 2, -profundidadPerro));
            perroIzquierda.add("4", new Punto(-anchoPerro / 2, altoPerro / 2, profundidadPerro));
            perroIzquierda.color = colorMarron;

            // Cara derecha del perro
            Cara perroDerecha = new Cara(shader, "derecha perro", origenCaraPerro);
            perroDerecha.add("1", new Punto(anchoPerro / 2, -altoPerro / 2, profundidadPerro));
            perroDerecha.add("2", new Punto(anchoPerro / 2, -altoPerro / 2, -profundidadPerro));
            perroDerecha.add("3", new Punto(anchoPerro / 2, altoPerro / 2, -profundidadPerro));
            perroDerecha.add("4", new Punto(anchoPerro / 2, altoPerro / 2, profundidadPerro));
            perroDerecha.color = colorMarron;

            Cara perroSuperior = new Cara(shader, "superior cuerpo", origenCaraPerro);
            perroSuperior.add("1", new Punto(-anchoPerro / 2, altoPerro / 2, profundidadPerro));
            perroSuperior.add("2", new Punto(anchoPerro / 2, altoPerro / 2, profundidadPerro));
            perroSuperior.add("3", new Punto(anchoPerro / 2, altoPerro / 2, -profundidadPerro));
            perroSuperior.add("4", new Punto(-anchoPerro / 2, altoPerro / 2, -profundidadPerro));
            perroSuperior.color = colorMarron;

            

            // Crear parte del perro (su cuerpo base)
            Parte perroCuerpo = new Parte(shader, "cuerpo perro", origenCaraPerro);
            perroCuerpo.add("frente", perroFrontal);
            perroCuerpo.add("detras", perroTrasera);
            perroCuerpo.add("izquerda", perroIzquierda);
            perroCuerpo.add("derecha", perroDerecha);
            perroCuerpo.add("superior", perroSuperior);


            // Crear objeto completo del perro
            Punto origenPerro = new Punto(5.0f, 0.0f, 0.0f);  // Ubicación del perro en la escena
            Objeto objetoPerro = new Objeto(shader, "perro completo", origenPerro);
            objetoPerro.add("cuerpo", perroCuerpo);

            //-------------------------------------CABEZA DEL PERROOOO------------------------------------------

            // Dimensiones de la cabeza del perro (más pequeña que el cuerpo)
            float anchoCabeza = 0.3f;
            float altoCabeza = 0.7f;
            float profundidadCabeza = 0.1f;

            // Color marrón más claro
            Vector3 colorMarronClaro = new Vector3(0.8f, 0.5f, 0.2f);

            // Posición de la cabeza: justo encima del cuerpo
            Punto origenCabeza = new Punto(0.0f, 0, 0.0f);

            // Cara frontal de la cabeza
            Cara cabezaFrontal = new Cara(shader, "frente cabeza", origenCabeza);
            cabezaFrontal.add("1", new Punto(-anchoCabeza / 2, altoPerro / 2, profundidadCabeza));
            cabezaFrontal.add("2", new Punto(anchoCabeza / 2, altoPerro / 2, profundidadCabeza));
            cabezaFrontal.add("3", new Punto(anchoCabeza / 2, altoCabeza / 2, profundidadCabeza));
            cabezaFrontal.add("4", new Punto(-anchoCabeza / 2, altoCabeza / 2, profundidadCabeza));
            cabezaFrontal.color = colorMarronClaro;

            // Cara trasera
            Cara cabezaTrasera = new Cara(shader, "detras cabeza", origenCabeza);
            cabezaTrasera.add("1", new Punto(-anchoCabeza / 2, altoPerro / 2, -profundidadCabeza));
            cabezaTrasera.add("2", new Punto(anchoCabeza / 2, altoPerro / 2, -profundidadCabeza));
            cabezaTrasera.add("3", new Punto(anchoCabeza / 2, altoCabeza / 2, -profundidadCabeza));
            cabezaTrasera.add("4", new Punto(-anchoCabeza / 2, altoCabeza / 2, -profundidadCabeza));
            cabezaTrasera.color = colorMarronClaro;

            // Cara izquierda
            Cara cabezaIzquierda = new Cara(shader, "izquierda cabeza", origenCabeza);
            cabezaIzquierda.add("1", new Punto(-anchoCabeza / 2, altoPerro / 2, profundidadCabeza));
            cabezaIzquierda.add("2", new Punto(-anchoCabeza / 2, altoPerro / 2, -profundidadCabeza));
            cabezaIzquierda.add("3", new Punto(-anchoCabeza / 2, altoCabeza / 2, -profundidadCabeza));
            cabezaIzquierda.add("4", new Punto(-anchoCabeza / 2, altoCabeza / 2, profundidadCabeza));
            cabezaIzquierda.color = colorMarronClaro;

            // Cara derecha
            Cara cabezaDerecha = new Cara(shader, "derecha cabeza", origenCabeza);
            cabezaDerecha.add("1", new Punto(anchoCabeza / 2, altoPerro / 2, profundidadCabeza));
            cabezaDerecha.add("2", new Punto(anchoCabeza / 2, altoPerro / 2, -profundidadCabeza));
            cabezaDerecha.add("3", new Punto(anchoCabeza / 2, altoCabeza / 2, -profundidadCabeza));
            cabezaDerecha.add("4", new Punto(anchoCabeza / 2, altoCabeza / 2, profundidadCabeza));
            cabezaDerecha.color = colorMarronClaro;

            Cara cabezaSuperior = new Cara(shader, "superior cabeza", origenCabeza);
            cabezaSuperior.add("1", new Punto(-anchoCabeza / 2, altoCabeza / 2, profundidadCabeza));
            cabezaSuperior.add("2", new Punto(anchoCabeza / 2, altoCabeza / 2, profundidadCabeza));
            cabezaSuperior.add("3", new Punto(anchoCabeza / 2, altoCabeza / 2, -profundidadCabeza));
            cabezaSuperior.add("4", new Punto(-anchoCabeza / 2, altoCabeza / 2, -profundidadCabeza));
            cabezaSuperior.color = colorMarronClaro;


            Punto origenParteCabeza = new Punto(0.35f, 0, 0.0f);
            // Crear la parte de la cabeza
            Parte cabezaParte = new Parte(shader, "cabeza perro", origenParteCabeza);
            cabezaParte.add("frente", cabezaFrontal);
            cabezaParte.add("detras", cabezaTrasera);
            cabezaParte.add("izquierda", cabezaIzquierda);
            cabezaParte.add("derecha", cabezaDerecha);
            cabezaParte.add("superior", cabezaSuperior);

            // Añadir la cabeza al objeto perro
            objetoPerro.add("cabeza", cabezaParte);



            // Añadir el perro a la escena
            escenaprueva.add("perro", objetoPerro);






            //----------------------------------PISTA DEL AUTO------------------
            Punto origenPista = new Punto(0.0f, 0.0f, 0.0f);
            Cara pista = new Cara(shader, "pista", origenPista);

            // Vertices (Y = 0 para estar en el plano XZ)

            float longitud = 3.0f;

            pista.add("1", new Punto(-longitud, 0.0f, -1f)); //lado detras izquierdo
            pista.add("2", new Punto(longitud + 5, 0.0f, -1f));  //lado delante izquierdo
            pista.add("3", new Punto(longitud + 5, 0.0f, 1f));    //lado delante derecho
            pista.add("4", new Punto(-longitud, 0.0f, 1f));   // lado detras derecho

            // Color: azul claro
            pista.color = new Vector3(0.3f, 0.3f, 1.0f);

            // Crear Parte y agregar la cara
            Punto origenPartePista = new Punto(0.0f, -0.30f, 0.0f);
            Parte partePista = new Parte(shader, "pista", origenPartePista);
            partePista.add("caraPista", pista);

            // Crear Objeto y agregar la parte
            Punto origenObjetoPista = new Punto(0.0f, 0.0f, 0.0f);
            Objeto objetoPista = new Objeto(shader, "pista", origenObjetoPista);
            objetoPista.add("partePista", partePista);

            // Insertar en el escenario
            escenaprueva.add("pista", objetoPista);




            // Crear la rueda, para las 4 ruedas
            Punto centroDeLaRueda = new Punto(0.0f, 0.0f, 0.0f); // Centro de la rueda
            Punto centroDeLaRueda22 = new Punto(0.0f, 0.0f, 0.0f); // Centro de la rueda22
            Punto centroDeLaRueda33 = new Punto(0.0f, 0.0f, 0.0f); // Centro de la rueda33
            Punto centroDeLaRueda44 = new Punto(0.0f, 0.0f, 0.0f); // Centro de la rueda44
            // Datos de la rueda
            float radio = 0.3f; // Radio de la rueda
            int numLados = 9; // 9 aristas
            float centroX = 0f, centroY = 0f, centroZ = 0f; // Centro de la rueda

            // Crear la cara de la rueda
            Cara caraRueda = new Cara(shader, "Cara Rueda", centroDeLaRueda);
            caraRueda.color = new Vector3(0f,0f,0f);

            Cara caraRueda2 = new Cara(shader, "Cara Rueda2", centroDeLaRueda22);
            caraRueda2.color = new Vector3(0f, 0f, 0f);

            Cara caraRueda3 = new Cara(shader, "Cara Rueda3 ", centroDeLaRueda33);
            caraRueda3.color = new Vector3(0f, 0f, 0f);

            Cara caraRueda4 = new Cara(shader, "Cara Rueda 4 ", centroDeLaRueda44);
            caraRueda4.color = new Vector3(0f, 0f, 0f);

            // Generar los puntos para la rueda
            for (int i = 0; i < numLados; i++)
            {
                float angulo = MathHelper.TwoPi / numLados * i; // Ángulo para cada punto
                float x = centroX + radio * (float)Math.Cos(angulo);
                float y = centroY + radio * (float)Math.Sin(angulo);
                float z = centroZ; // Todo en el mismo plano Z

                // Agregar cada punto a la cara de la rueda
                caraRueda.add(i.ToString(), new Punto(x, y, z));
                caraRueda2.add(i.ToString(), new Punto(x, y, z));
                caraRueda3.add(i.ToString(), new Punto(x, y, z));
                caraRueda4.add(i.ToString(), new Punto(x, y, z));
            }

            // Ahora puedes agregar esta cara de la rueda a una parte u objeto, como lo hiciste con la pelota
            Punto centroDeLaRueda1 = new Punto(-0.5f, 0.0f, 0.0f); // Centro de la rueda 11
            Parte parteRueda = new Parte(shader, "Parte Rueda", centroDeLaRueda1);
            parteRueda.add("1", caraRueda);


            Punto centroDeLaRueda2 = new Punto(0.8f, 0.0f, 0.0f); // Centro de la rueda 22
            Parte parteRueda2 = new Parte(shader, "Parte Rueda", centroDeLaRueda2);
            parteRueda2.add("1", caraRueda2);

            Punto centroDeLaRueda3 = new Punto(0.8f, 0.0f, -0.5f); // Centro de la rueda 33  delantera izquierda
            Parte parteRueda3 = new Parte(shader, "Parte Rueda", centroDeLaRueda3);
            parteRueda3.add("1", caraRueda3);

            Punto centroDeLaRueda4 = new Punto(-0.5f, 0.0f, -0.5f); // Centro de la rueda 44 trasera izquierda
            Parte parteRueda4 = new Parte(shader, "Parte Rueda", centroDeLaRueda4);
            parteRueda4.add("1", caraRueda4);


            // Crear un objeto para la rueda
            Punto origenRueda = new Punto(0.0f, 0.0f, 0.0f); // Posición de la rueda en el espacio
            Objeto objetoRueda = new Objeto(shader, "Objeto Rueda", origenRueda);


            // Agregar la parte de la rueda al objeto
            objetoRueda.add("1", parteRueda);
            objetoRueda.add("2", parteRueda2);

            // Finalmente, agregar el objeto de la rueda a tu escena
          //  escenaprueva.add("auto", objetoRueda); // Rueda agregada a la escena



            // Variables para tamaño general
           
            //-----AUTO---------------
            
            Punto origencaraauto = new Punto(0.0f, 0.0f, 0.0f);

            float x1 = origencaraauto.x - 1f; //punto trasero inferior
            float x2 = origencaraauto.x + 1f;  // punto delantero inferior
            float y1 = origencaraauto.y + 0.55f; // altura del capod delantero
            float y2 = origencaraauto.y + 1f;     // altura de la parte superior trasera

            float inicioVentanaX = origencaraauto.x -0.3f;
            float inicioVentanaY = origencaraauto.y + 0.55f;
            float largoVentana = 0.5f;
            float altoVentana = 0.35f;


            // Origen general


            // --------- Caras principales del auto --------- //
            // Cara principal del auto (perfil)
            Cara perfilAuto = new Cara(shader, "Perfil Auto", origencaraauto);
            perfilAuto.add("1", new Punto(x1, origencaraauto.y, 0.0f));                  // Parte trasera inferior
            perfilAuto.add("2", new Punto(x2, origencaraauto.y, 0.0f));              // Parte delantera inferior
            perfilAuto.add("3", new Punto(x2, y1, 0.0f));        // Frente altura de capod
            perfilAuto.add("4", new Punto(x2 - 0.5f, y2, 0.0f)); // Curva techo
            perfilAuto.add("5", new Punto(x1, y2, 0.0f));               // Trasero superior

            Punto origencaraautoizq = new Punto(0.0f, 0.0f, -0.5f);
            // Cara principal del auto (perfil izquierdo)
            Cara perfilAutoizq = new Cara(shader, "Perfil Auto", origencaraautoizq);
            perfilAutoizq.add("1", new Punto(x1, origencaraautoizq.y, 0.0f));                  // Parte trasera inferior
            perfilAutoizq.add("2", new Punto(x2, origencaraautoizq.y, 0.0f));              // Parte delantera inferior
            perfilAutoizq.add("3", new Punto(x2, y1, 0.0f));        // Frente , altura de capod
            perfilAutoizq.add("4", new Punto(x2 - 0.5f, y2, 0.0f)); // Curva techo
            perfilAutoizq.add("5", new Punto(x1, y2, 0.0f));        // Trasero superior



            //parabrisas delantero del auto
            Punto origencaraautoparabrisas = new Punto(0.0f, 0.0f, 0.0f);
            Cara perfilAutoparabrisas = new Cara(shader, "Perfil Auto techo", origencaraautoparabrisas);
            perfilAutoparabrisas.color = new Vector3(0f, 0f, 0f);
            // Rojo: 1.0     Verde: 1.0     Azul: 0.0
            perfilAutoparabrisas.add("1", new Punto(x2 - 0.5f, y2, 0.0f)); //parabrisas superior derecha
            perfilAutoparabrisas.add("2", new Punto(x2, y1, 0.0f));        // Frente , altura de capod derecha
            perfilAutoparabrisas.add("3", new Punto(x2, y1, -0.5f));        // Frente , altura de capod izquierda
            perfilAutoparabrisas.add("4", new Punto(x2 - 0.5f, y2, -0.5f)); //parabrisas superior derecha

            //capod delantero del auto
            Punto origencaraautocapod = new Punto(0.0f, 0.0f, 0.0f);
            Cara perfilAutocapod = new Cara(shader, "Perfil Auto techo", origencaraautocapod);
            perfilAutocapod.color = new Vector3(0f, 0f, 0.3f);
            // Rojo: 1.0     Verde: 1.0     Azul: 0.0

            perfilAutocapod.add("1", new Punto(x2, y1, 0.0f));        // Frente , altura de capod derecha
            perfilAutocapod.add("2", new Punto(x2, y1, -0.5f));        // Frente , altura de capod izquierda
            perfilAutocapod.add("3", new Punto(x2, origencaraautoizq.y, -0.5f));              // Parte delantera inferior izquierda
            perfilAutocapod.add("4", new Punto(x2, origencaraautoizq.y, 0.0f));              // Parte delantera inferior derecha




            //parte de atras del auto
            Punto origencaraautodetras = new Punto(0.0f, 0.0f, 0.0f);
            Cara perfilAutodetras = new Cara(shader, "Perfil Auto detras", origencaraautodetras);
            perfilAutodetras.color = new Vector3(0f, 0f, 0.3f);
                                        // Rojo: 1.0     Verde: 1.0     Azul: 0.0
            perfilAutodetras.add("1", new Punto(x1, origencaraauto.y, 0.0f)); // Parte trasera inferior derecha
            perfilAutodetras.add("2", new Punto(x1, y2, 0.0f)); // Parte trasera superior derecha
            perfilAutodetras.add("3", new Punto(x1, y2, -0.5f));        // Trasero superior izquierda
            perfilAutodetras.add("4", new Punto(x1, origencaraautoizq.y, -0.5f));                  // Parte trasera inferior izquierda

            //techo del auto
            Punto origencaraautotecho = new Punto(0.0f, 0.0f, 0.0f);
            Cara perfilAutotecho = new Cara(shader, "Perfil Auto techo", origencaraautotecho);
            perfilAutotecho.color = new Vector3(0f, 0f, 0.1f);
            // Rojo: 1.0     Verde: 1.0     Azul: 0.0
            perfilAutotecho.add("1", new Punto(x1, y2, 0.0f)); // Parte trasera superior derecha
            perfilAutotecho.add("2", new Punto(x2 - 0.5f, y2, 0.0f)); // Curva techo
            perfilAutotecho.add("3", new Punto(x2 - 0.5f, y2, -0.5f)); // Curva techo
            perfilAutotecho.add("4", new Punto(x1, y2, -0.5f));                   // Parte trasera inferior izquierda




            // Cara de la ventana
            Cara ventanaAuto = new Cara(shader, "Ventana Auto", origencaraauto);
            ventanaAuto.color = new Vector3(0f, 0f, 0f);
            ventanaAuto.add("1", new Punto(inicioVentanaX, inicioVentanaY, 0.01f));
            ventanaAuto.add("2", new Punto(inicioVentanaX + largoVentana + 0.5f, inicioVentanaY, 0.01f));
            ventanaAuto.add("3", new Punto(inicioVentanaX + largoVentana, inicioVentanaY + altoVentana, 0.01f));
            ventanaAuto.add("4", new Punto(inicioVentanaX, inicioVentanaY + altoVentana, 0.01f));
            // Cara de la ventana izquierda
            Cara ventanaAutoizq = new Cara(shader, "Ventana Auto", origencaraautoizq);
            ventanaAutoizq.color = new Vector3(0f, 0f, 0f);
            ventanaAutoizq.add("1", new Punto(inicioVentanaX, inicioVentanaY, -0.01f));
            ventanaAutoizq.add("2", new Punto(inicioVentanaX + largoVentana + 0.5f, inicioVentanaY, -0.01f));
            ventanaAutoizq.add("3", new Punto(inicioVentanaX + largoVentana, inicioVentanaY + altoVentana, -0.01f));
            ventanaAutoizq.add("4", new Punto(inicioVentanaX, inicioVentanaY + altoVentana, -0.01f));
           

            // --------- Parte que une las caras 
            Punto origenparte = new Punto(0.0f, 0.0f, 0.0f);
            Parte parteAuto = new Parte(shader, "Parte Auto", origenparte);

            // Agregar las caras a la parte
           parteAuto.add("11", perfilAuto);
        //    parteAuto.add("22", ventanaAuto);
           parteAuto.add("33", perfilAutoizq);
            parteAuto.add("44", ventanaAutoizq);

            //ventana parte independiente
            Punto origenparteventana = new Punto(0.0f, 0.0f, 0.0f);
            Parte parteAutoventana = new Parte(shader, "parteventana", origenparteventana);

            parteAutoventana.add("ventana", ventanaAuto);


            Punto origenpartecuerpo = new Punto(0.0f, 0.0f, 0.0f);
            Parte parteAutotechos = new Parte(shader, "Parte Auto techos", origenpartecuerpo);

            //--------Techo , detras, capod, parabisasa 
            parteAutotechos.add("tapa detras", perfilAutodetras);
            parteAutotechos.add("techo", perfilAutotecho);
            parteAutotechos.add("parabrisas", perfilAutoparabrisas);
            parteAutotechos.add("capod delantero", perfilAutocapod);

            // -------- Ruedas
            Punto origenobjetoauto = new Punto(0.0f, 0.0f, 0.0f);
            Objeto objetoAuto = new Objeto(shader, "Objeto Auto", origenobjetoauto);

            // Agregar parte al objeto
            objetoAuto.add("1", parteAuto);   // pefilauto izq, derecha
            objetoAuto.add("r11", parteRueda);  // rueda trasera derecha
            objetoAuto.add("r22", parteRueda2); // rueda delantera derecha
            objetoAuto.add("r33", parteRueda3); // Rueda  delantera izquierda
            objetoAuto.add("r44", parteRueda4);  //rueda  trasera izquierda

            objetoAuto.add("ventana", parteAutoventana);


            objetoAuto.add("techos", parteAutotechos);

            // --------- Agregar a escena --------- //
            escenaprueva.add("6", objetoAuto);






            // --------------------------------------------SERIALIZADO Y DESSERIALIZADO -------------------------------------------------------//


            
           Utilidades utils = new Utilidades();
           //serializarobjeto
           string rutaArchivo1 = "../../../escenaauto2025.json";
           utils.saveJson(rutaArchivo1, escenaprueva);

            


            // Deserializar objeto
            Utilidades utils2 = new Utilidades();
            string rutaArchivoDes = "../../../escenaauto2025.json"; // Ruta del archivo JSON
            this.escenaDeserializada = utils2.getObjectFromJson<Escena>(rutaArchivoDes);
            Console.WriteLine(escenaDeserializada);

            this.escenaDeserializada.setShader(shader);




            
            
            //  Accion acto1 = new Accion("act1", escenaDeserializada2.get("1").get("1"), "trasladar", 5, new Punto(0.01f, 0.01f, 0.01f));
            //   Accion acto2 = new Accion("act2", escenaDeserializada2.get("1").get("1"),"trasladar", 5, new Punto(0.01f, 0.01f, 0.01f));

            //  this.animacion1 = new Animacion("ani1", 10f, 20f,escenaDeserializada2);

           // this.animacion1 = new Animacion("ani1", 10f, 20f, escenaprueva);
            //   this.animacion1.add("1", acto1);
            //   this.animacion1.add("2", acto2);


              Accion acto1 = new Accion("act1", escenaprueva.get("arbol").get("hojas"),null, "rotar", 60, new Punto(0.0f, 1.0f, 0.0f));
              Accion acto2 = new Accion("act2", escenaprueva.get("perro").get("cabeza"),null, "rotar", 60, new Punto(0.0f, 1.0f, 0.0f));
              Accion acto3 = new Accion("act3", escenaprueva.get("perro").get("cabeza"),null, "rotar", 60, new Punto(0.0f, 1.0f, 0.0f));


            Accion acto4 = new Accion("act4",null,  escenaprueva.get("perro"), "rotar", 60, new Punto(0.0f, 1.0f, 0.0f)); // es el perro

            Accion acto5 = new Accion("act5",null, escenaprueva.get("6"), "rotar", 60, new Punto(0.0f, 1.0f, 0.0f)); // es el auto 

            Accion acto6 = new Accion("act6", null, escenaprueva.get("arbol"), "rotar", 60, new Punto(0.0f, 1.0f, 0.0f)); // es el arbol


            Accion acto7 = new Accion("act7", null, escenaprueva.get("perro"), "trasladar", 0, new Punto(1.0f, 0.0f, 0.0f)); // es el perro

            Accion acto8 = new Accion("act8", null, escenaprueva.get("6"), "trasladar", 0, new Punto(1.0f, 0.0f, 0.0f)); // es el auto 

            Accion acto9 = new Accion("act9", null, escenaprueva.get("arbol"), "trasladar", 0, new Punto(1.0f, 1.0f, 0.0f)); // es el arbol





            //  this.animacion1 = new Animacion("ani1", 10f, 20f,escenaDeserializada2);
/*
            this.animacion1 = new Animacion("perro arbol giran", 10f, 20f, escenaprueva);
            this.animacion1.add("1", acto1);
            this.animacion1.add("2", acto2);
            this.animacion1.add("3", acto3);
            this.animacion1.add("4", acto4);
            this.animacion1.add("5", acto5);
            this.animacion1.add("6", acto6);
            this.animacion1.add("7", acto7);
            this.animacion1.add("8", acto8);
            this.animacion1.add("9", acto9);
            */

            // Animación vacía
            this.animacion1 = new Animacion("secuencia personalizada", 30f, 20f, escenaprueva);

            // Paso 1-3: Auto se traslada 3 veces + árbol rota entre cada uno
            for (int i = 1; i <= 50; i++)
            {
                // Auto se traslada en X
                Accion moverAuto = new Accion($"auto_move_{i}", null, escenaprueva.get("6"), "trasladar", 0, new Punto(0.075f, 0.0f, 0.0f));
                this.animacion1.add($"auto_move_{i}", moverAuto);

                // Árbol rota en Y
                Accion rotarArbol = new Accion($"arbol_rot_{i}", null, escenaprueva.get("arbol"), "rotar", 10, new Punto(0.0f, 1.0f, 0.0f));
                this.animacion1.add($"arbol_rot_{i}", rotarArbol);
            }

            // Paso 4: Árbol gira antes de movimiento del perro
            Accion arbolAntesPerro = new Accion("arbol_rot_antes_perro", null, escenaprueva.get("arbol"), "rotar", 30, new Punto(0.0f, 1.0f, 0.0f));
            this.animacion1.add("arbol_rot_antes_perro", arbolAntesPerro);

            // Paso 5: Perro se traslada en Y
            //  Accion moverPerro = new Accion("perro_move", null, escenaprueva.get("perro"), "trasladar", 0, new Punto(0.0f, 0.0f, 3.0f));
            //    this.animacion1.add("perro_move", moverPerro);

            Accion moverPerrogirrar = new Accion($"perro_move500", null, escenaprueva.get("perro"), "rotar", -90, new Punto(0.0f, 1.0f, 0.0f));

            this.animacion1.add($"perro_move500", moverPerrogirrar);

            for (int i = 1; i <= 40; i++)
            {
                // Auto se traslada en X
                Accion moverPerro = new Accion($"perro_move{i}", null, escenaprueva.get("perro"), "trasladar", 0, new Punto(0.0f, 0.0f, 0.1f));
             
                this.animacion1.add($"perro_move{i}", moverPerro);

                // Árbol rota en Y
                Accion rotarArbol = new Accion($"arbol_rot_des{i}", null, escenaprueva.get("arbol"), "rotar", 30, new Punto(0.0f, 1.0f, 0.0f));
                this.animacion1.add($"arbol_rot_des{i}", rotarArbol);
            }

            // Paso 6: Perro rota 80° sobre Y
            Accion rotarPerro = new Accion("perro_rot", null, escenaprueva.get("perro"), "rotar", 80.0f, new Punto(0.0f, 1.0f, 0.0f));
            this.animacion1.add("perro_rot", rotarPerro);

            

            for (int i = 1; i <= 50; i++)
            {
                // Auto se traslada en X
                Accion moverAuto = new Accion($"auto_move_sig{i}", null, escenaprueva.get("6"), "trasladar", 0, new Punto(0.1f, 0.0f, 0.0f));
                this.animacion1.add($"auto_move_sig{i}", moverAuto);

                // Árbol rota en Y
                Accion rotarArbol = new Accion($"arbol_rota_sig{i}", null, escenaprueva.get("arbol"), "rotar", 30, new Punto(0.0f, 1.0f, 0.0f));
                this.animacion1.add($"arbol_rota_sig{i}", rotarArbol);
            }

            // Parámetros del arco
            float radiogiro = 2.0f;
            int pasos = 100;
            // angulo inicial
            float gradosInicio = 270.0f;
            float deltaGrados = 180.0f / pasos;

            for (int i = 1; i <= pasos; i++)
            {
                float thetaGrados = gradosInicio + i * deltaGrados;
                float thetaRadianes = thetaGrados * (float)(Math.PI / 180.0f); // conversión

                float x = radiogiro * (float)Math.Cos(thetaRadianes);
                float z = radiogiro * (float)Math.Sin(thetaRadianes);

                float prevThetaRadianes = (thetaGrados - deltaGrados) * (float)(Math.PI / 180.0f);
                float xDelta = i == 0 ? x : x - radiogiro * (float)Math.Cos(prevThetaRadianes);
                float zDelta = i == 0 ? z : z - radiogiro * (float)Math.Sin(prevThetaRadianes);

                Console.WriteLine("rotar en U " + "x , " + xDelta + "z , " + zDelta);


                Accion moverAutoU = new Accion($"auto_U_{i}", null, escenaprueva.get("6"), "trasladar", 0, new Punto(xDelta, 0.0f, zDelta));
                this.animacion1.add($"auto_U_{i}", moverAutoU);

                Accion RotarAuto = new Accion($"auto_U_girarauto{i}", null, escenaprueva.get("6"), "rotar", -1.9f, new Punto(0.0f, 1.0f, 0.0f)); // es el auto 
                this.animacion1.add($"auto_U_girarauto{i}", RotarAuto);
                // Árbol rota cada paso también
                Accion rotarArbolU = new Accion($"arbol_U_{i}", null, escenaprueva.get("arbol"), "rotar", 40, new Punto(0.0f, 1.0f, 0.0f));
                this.animacion1.add($"arbol_U_{i}", rotarArbolU);
            }



            string rutaArchivoAnimacion = "../../../animacio1.json";
            utils.saveJson(rutaArchivoAnimacion, animacion1);

            _camera = new Camera(Vector3.UnitZ, Size.X / (float)Size.Y);

        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
          base.OnRenderFrame(args);
            _time += 0.0 * args.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.BindVertexArray(vertexArrayObject);
            shader.Use();


           var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
           shader.SetMatrix4("model", model);
           shader.SetMatrix4("view", _camera.GetViewMatrix());
           shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
          

         
           //this.escenaDeserializada.dibujar();

         
           escenaprueva.dibujar();


            Context.SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
           

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0); 
            base.OnUnload();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            
            base.OnUpdateFrame(e);
           
            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.0f;
            const float sensitivity = 0.2f;



            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time;


            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.T) && !mythreadbool)
            {
           //     Director dir = new Director();
            //    dir.add(this.animacion1);
            //    dir.start();

                Directors dir2 = new Directors();
               dir2.Add(this.animacion1);
               dir2.Start();
            }

            if (input.IsKeyDown(Keys.Y))
            {
               // escenaprueva.get("arbol").get("hojas").rotar(1, 0.0f, 1.0f, 0.0f);
              // escenaprueva.get("arbol").rotar(1, 0.0f, 1.0f, 0.0f);


              //  escenaprueva.get("perro").get("cabeza").rotar(1, 0.0f, 1.0f, 0.0f);
                escenaprueva.get("perro").rotar(1, 0.0f, 1.0f, 0.0f);
            }

            //"r11"/  // rueda trasera derecha
            //"r22",/ // rueda delantera derecha
            //"r33" // Rueda  delantera izquierda
            //"r44",  //rueda  trasera izquierda
            if (input.IsKeyPressed(Keys.Z)) // Solo una vez cuando presionas
            {
                escenaprueva.get("6").get("r22").rotar(45, 0.0f, 1.0f, 0.0f);
                escenaprueva.get("6").get("r33").rotar(45, 0.0f, 1.0f, 0.0f);
            }
            if (input.IsKeyReleased(Keys.Z)) // Cuando sueltas
            {
                escenaprueva.get("6").get("r22").rotar(-45, 0.0f, 1.0f, 0.0f); // ¡Rotar al revés!
                escenaprueva.get("6").get("r33").rotar(-45, 0.0f, 1.0f, 0.0f);
            }
            if (input.IsKeyDown(Keys.Z))
            {
           
               escenaprueva.get("6").get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
               escenaprueva.get("6").get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);

                escenaprueva.get("6").rotar(1, 0.0f, 1.0f, 0.0f);
              

                angulo -= 1.0f; // -1 grado
            }

            if (input.IsKeyDown(Keys.O)) // Solo una vez cuando presionas
            {
              //  escenaprueva.get("6").get("ventana").rotar(1, 0.0f, 1.0f, 0.0f);
                                                              // X    Y      Z

                escenaprueva.get("6").get("ventana").trasladar(0.1f, 0f, 0f);

             //   escenaprueva.get("6").get("ventana").escalar(1.2f, 1.0f, 1.0f);


            }
            if (input.IsKeyDown(Keys.P)) // Solo una vez cuando presionas
            {
                escenaprueva.get("6").get("ventana").rotar(25, 0.0f, 1.0f, 0.0f);
                // X    Y      Z

            }

            if (input.IsKeyPressed(Keys.X)) // Solo una vez cuando presionas
            {
                escenaprueva.get("6").get("r22").rotar(-45, 0.0f, 1.0f, 0.0f);
                escenaprueva.get("6").get("r33").rotar(-45, 0.0f, 1.0f, 0.0f);
            }
            if (input.IsKeyReleased(Keys.X)) // Cuando sueltas
            {
                escenaprueva.get("6").get("r22").rotar(45, 0.0f, 1.0f, 0.0f); // ¡Rotar al revés!
                escenaprueva.get("6").get("r33").rotar(45, 0.0f, 1.0f, 0.0f);
            }
            if (input.IsKeyDown(Keys.X))
            {
              //  escenaprueva.get("auto").get("2").rotar(5, 0.0f, 0.0f, 1.0f);
                escenaprueva.get("6").get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
                escenaprueva.get("6").get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);
                escenaprueva.get("6").rotar(-1, 0.0f, 1.0f, 0.0f);
                angulo += 1.0f; // +1 grado
            }

            if (input.IsKeyDown(Keys.C))
            {
               
                escenaprueva.get("6").trasladar(0.01f, 0.0f, 0.0f);
                escenaprueva.get("6").get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
                escenaprueva.get("6").get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);
             
            }
            // Mover a la derecha
            if (input.IsKeyDown(Keys.Right))
            {
                escenaprueva.get("6").escalar(1.01f,1.01f,1.01f);
            }

            // Mover a la izquierda
            if (input.IsKeyDown(Keys.Left))
            {
                escenaprueva.get("6").escalar(0.95f, 0.95f, 0.95f);
            }

            if (input.IsKeyDown(Keys.L))
            {
               // escenaprueva.get("auto").get("2").rotar(5, 0.0f, 0.0f, 1.0f);
               // escenaprueva.get("auto").get("1").rotar(5, 0.0f, 0.0f, 1.0f);
                //  escenaprueva.get("auto").trasladar(0.01f, 0.0f, 0.0f);
                //  escenaprueva.get("6").trasladar(0.01f, 0.0f, 0.0f);

                Console.WriteLine("objet " + angulo + " grados");

                float anguloRad = MathHelper.DegreesToRadians(angulo);

                // Movimiento corregido: usar Sin para X, Cos para Z
                float movZ = velocidad * (float)Math.Sin(anguloRad);
                float movX = velocidad * (float)Math.Cos(anguloRad);

                // Actualizar posición
                posX += movX;
                posZ += movZ;

                // Mover el auto
              //  escenaprueva.get("auto").trasladar(movX, 0.0f, movZ);
                escenaprueva.get("6").trasladar(movX, 0.0f, movZ);
                escenaprueva.get("6").get("r11").rotar(-5, 0.0f, 0.0f, 1.0f);
                escenaprueva.get("6").get("r44").rotar(-5, 0.0f, 0.0f, 1.0f);

            }


            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }
    }
}
