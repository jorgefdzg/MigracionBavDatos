using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionDeDatosBav
{
    public class ClsEstructuraFamiliar
    {
        public int NumeroEstudio { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string FechaDeNacimiento { get; set; }
        public string Genero { get; set; }
        public string EntidadDeNacimiento { get; set; }
        public string EstadoCivil { get; set; }
        public string Parentesco { get; set; }
        public int NoParentesco { get; set; }
    }
}
