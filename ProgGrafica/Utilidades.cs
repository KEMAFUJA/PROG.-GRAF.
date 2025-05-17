using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace ProgGraficaTareas
{
    public class Utilidades
    {
        public Escena objectt;
        public Utilidades() { }

        public void saveJson(String ruta ,Object objectt) {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = null,
                WriteIndented = true 
            };
            try
            {
                string json = JsonSerializer.Serialize(objectt, options);
                File.WriteAllText(ruta, json);
                Console.WriteLine("se guardo el archivo JSON");
            }
            catch (Exception ex)
            {
                Console.WriteLine("no se pudo guardar el archivo");
            }
        }
        public OB getObjectFromJson<OB>(String ruta)
        {   OB data = default(OB);    
            try
            {
                string jsonDes = File.ReadAllText(ruta);
                data = JsonSerializer.Deserialize<OB>(jsonDes);
                return data;
            }catch (Exception ex)
            {
                Console.WriteLine("no se pudo obtener el archivo");
                throw;
            }
        }

    }
}
