using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionDeDatosBav
{
    public class ClsEstudioSocioEconomico
    {
        public int NumeroEstudio { get; set; }
        public string Programa { get; set; }
        public string NombreGrupo { get; set; }
        public DateTime FechaLevantamiento { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public string Vialidad { get; set; }
        public int NumeroExterior { get; set; }
        public int NumeroInterior { get; set; }
        public string CodigoPostal { get; set; }
        public string TipoVialidad { get; set; }
        public string TipoAsentamiento { get; set; }
        public string ServicioLuz { get; set; }
        public string ServicioDrenaje { get; set; }
        public string ServicoBaño { get; set; }
        public string ServicioCombustible { get; set; }
        public string ServicioAgua { get; set; }
        public string Tenencia { get; set; }
        public string TipoDeCasa { get; set; }
        public string Muros { get; set; }
        public string Techo { get; set; }
        public string Piso { get; set; }
        public int NumeroDeCuartos { get; set; }
        public int CuartosParaDormir { get; set; }
        public string TipoDeApoyo { get; set; }
        public string FrecuenciaDeApoyo { get; set; }
        public string DuracionDelApoyo { get; set; }
        public string IFEAsociativo { get; set; }
    }
}
