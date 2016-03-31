using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MigracionDeDatosBav
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ClsVivienda> listaViviendaMigrar = new List<ClsVivienda>();
          listaViviendaMigrar= CreaListaVivienda();
          List<ClsTitular> listaTitularMigrar = new List<ClsTitular>();
          listaTitularMigrar = CrearListaTitular();
          List<ClsFamiliares> listaFamiliaresMigrar = new List<ClsFamiliares>();
          listaFamiliaresMigrar = CrearListaFamiliares();
          List<ClsEstudioSocioEconomico> listaEstudios = new List<ClsEstudioSocioEconomico>();
          listaEstudios = CreaListaEstudiosSocioEconomicos(listaViviendaMigrar, listaTitularMigrar);
          List<ClsEstructuraFamiliar> listaEstructura = new List<ClsEstructuraFamiliar>();
          listaEstructura = CrearListaEstructura(listaEstudios, listaTitularMigrar, listaFamiliaresMigrar);
        }
        static List<ClsEstructuraFamiliar> CrearListaEstructura(List<ClsEstudioSocioEconomico> listaEstudios, List<ClsTitular> listaTitulares, List<ClsFamiliares> listaFamiliares) {
            List<ClsEstructuraFamiliar> listaFamilia = new List<ClsEstructuraFamiliar>();
            foreach (ClsEstudioSocioEconomico estudio in listaEstudios) {
                ClsEstructuraFamiliar estructura = new ClsEstructuraFamiliar();
                ClsTitular titular = listaTitulares.FirstOrDefault(x => x.IFETitular == estudio.IFEAsociativo);
                estructura.NumeroEstudio = estudio.NumeroEstudio;
                estructura.Nombre = titular.NombreTitular;
                estructura.PrimerApellido = titular.ApellidoTitular;
                estructura.FechaDeNacimiento = CalculaFechaDeNacimiento(titular.FechaDeNacimiento, titular.EdadTitular);
                estructura.Genero = ParseaSexo(titular.Sexo);
                estructura.EntidadDeNacimiento = BuscaEntidadDeNacimiento(titular.CURPTitular);
                List<ClsFamiliares> familiares = listaFamiliares.FindAll(x => x.IFEFamiliares == estudio.IFEAsociativo);
                foreach (ClsFamiliares familia in familiares) { 
                
                }
            }
        }
        static string ParseaSexo(string genero)
        {
            if (genero == "MASCULINO")
            {
                return "Masculino";
            }
            else {
                return "Femenino";
            }
        }

        static List<ClsEstudioSocioEconomico> CreaListaEstudiosSocioEconomicos(List<ClsVivienda> listaVivienda, List<ClsTitular> listaTitular)
        {
            List<ClsEstudioSocioEconomico> listaEstudioSocioEconomico = new List<ClsEstudioSocioEconomico>();
            int contadorestudio=0;
            foreach (ClsTitular titular in listaTitular) 
            {
                ClsEstudioSocioEconomico estudio = new ClsEstudioSocioEconomico();
                ClsVivienda vivienda = listaVivienda.FirstOrDefault(x => x.IFE == titular.IFETitular);
                if (vivienda != null)
                {
                    contadorestudio += 1;
                    estudio.NumeroEstudio = contadorestudio;
                    estudio.Programa = "MESA BAV";
                    estudio.NombreGrupo = "MESA BAV";
                    estudio.FechaLevantamiento = ParseaYRetornaFecha(titular.FechaIngreso);
                    estudio.Estado = "Veracruz";
                    estudio.Municipio = ParseaMunicipio(titular.Municipio);
                    estudio.Localidad = ParseaColonia(titular.Colonia);
                    estudio.Vialidad = ParseaNombreCalle(titular.Domicilio);
                    estudio.NumeroExterior = Convert.ToInt32(ParseaCadenaParaNumeroExterior(titular.Domicilio));
                    estudio.NumeroInterior = 0;
                    estudio.CodigoPostal = titular.CodigoPostal;
                    estudio.TipoVialidad = RandomVialidad();
                    estudio.TipoAsentamiento = RandomAsentamiento();
                    estudio.ServicioLuz = ParseaServicioLuz(vivienda.ServicioEnergia);
                    estudio.ServicioDrenaje = ParseaServicioDrenaje(vivienda.ServicioDrenaje);
                    estudio.ServicoBaño = "Descarga directa";
                    estudio.ServicioCombustible = ParseaCombustible(vivienda.Estufa);
                    estudio.ServicioAgua = ParseaServicioAgua(vivienda.ServicioAgua);
                    estudio.Tenencia = ParseaTenencia(vivienda.Economico);
                    estudio.TipoDeCasa = ParseaTipoDeCasa(vivienda.Tipo);
                    estudio.Muros = ParseaMuros(vivienda.Material);
                    estudio.Techo = ParseaTecho(vivienda.Material);
                    estudio.Piso = ParseaPiso(vivienda.ServicioPiso);
                    estudio.NumeroDeCuartos = RandomCuartos();
                    estudio.CuartosParaDormir = RandomCuartosParaDormir(estudio.NumeroDeCuartos);
                    estudio.TipoDeApoyo = "Cuota";
                    estudio.FrecuenciaDeApoyo = "Semanal";
                    estudio.DuracionDelApoyo = "1 año";
                    estudio.IFEAsociativo = titular.IFETitular;

                    listaEstudioSocioEconomico.Add(estudio);
                }
            }
            return listaEstudioSocioEconomico;
        }
        static int RandomCuartosParaDormir(int cuartos) {
            if (cuartos >= 3)
            {
                return cuartos - 2;
            }
            else
            {
                return 1;
            }
        }
        static int RandomCuartos(){
            Random rnd = new Random();
            int cuartos = rnd.Next(1, 5);
            return cuartos;
        }
        static string ParseaPiso(string serviciopiso) {
            if (serviciopiso == "MOSAICO") {
                return "Mosaico, vinil";
            }
            if (serviciopiso == "TIERRA") {
                return "Tierra";
            }
            if (serviciopiso == "CEMENTO") {
                return "Cemento o firme";
            }
            return "Tierra";
        }
        static string ParseaTecho(string material) {
            if (material == "CONCRETO")
            {
                return "Concreto, losa o viguetas";
            }
            if (material.Contains("MATERIAL"))
            {
                return "Lámina asbesto, metálica";
            }
            if (material.Contains("MADERA"))
            {
                return "Madera, teja";
            }
            if (material.Contains("ASBESTO"))
            {
                return "Concreto, losa o viguetas";
            }
            if (material.Contains("LAMINA"))
            {
                return "Lámina de cartón, desecho";
            }
            return "Concreto, losa o viguetas";
        }
        static string ParseaMuros(string material) {
            if (material == "CONCRETO") {
                return "Ladrillo, tabique";
            }
            if (material.Contains("MATERIAL")) {
                return "Lámina metálica, asbesto";
            }
            if (material.Contains("MADERA")) {
                return "Madera";
            }
            if (material.Contains("ASBESTO")) {
                return "Adobe";
            }
            if (material.Contains("LAMINA")) {
                return "Desechos, Cartón";
            }
            return "Ladrillo, tabique";
        }
        static string ParseaTipoDeCasa(string tipo) {
            if (tipo == "CASA INDEPENDIENTE") {
                return "Independiente";
            }
            if (tipo == "DEPARTAMENTO") {
                return "Vecindad";
            }
            if (tipo == "CUARTO VECINDAD") {
                return "U. habitacional";
            }
            return "Independiente";
        }
        static string ParseaTenencia(string economico) {
            if (economico == "PROPIA") {
                return "Pagándose";
            }
            if (economico == "RENTADA") {
                return "Rentada";
            }
            if (economico == "PRESTADA") {
                return "Prestada";
            }
            return "Propia";
        }

        static string ParseaServicioAgua(string servicioagua) {
            if (servicioagua == "RED PUBLICA") {
                return "Toma domiciliaria";
            }
            if (servicioagua == "POZO") {
                return "Pozo, río, lago";
            }
            if (servicioagua == "PIPA") {
                return "Pipa";
            }
            if (servicioagua == "NO TIENE") {
                return "Sin servicio";
            }

            return "Sin servicio";
        }
        static string ParseaCombustible(int estufa){
            if (estufa >= 1)
            {
                return "Gas tanque";
            }
            else {
                return "Leña o carbón sin chimenea";
            }
        }
        static string ParseaServicioDrenaje(string serviciodrenaje) {

            if (serviciodrenaje == "RED PUBLICA") {
                return "Red pública";
            }
            if (serviciodrenaje == "FOSA SEPTICA") {
                return "Fosa séptica";
            }
            if (serviciodrenaje == "NO TIENE") {
                return "No tiene drenaje";
            }
            return "No tiene drenaje";
        }
        static string ParseaServicioLuz(string servicioenergia) 
        {
            if (servicioenergia == "SI")
            {
                return "Servicio público";
            }
            else {
                return "No tienen";
            }
        
        }
        static string RandomAsentamiento() {
            string[] array = { "Barrio", "Colonia", "Unidad" };
            Random rnd = new Random();
            int indice = rnd.Next(0, 2);
            return array[indice];
        }
        static string RandomVialidad() {
            string[] array = { "Andador", "Avenida", "Calle", "Callejón", "Circunvalacion", "Ampliación" };
            Random rnd = new Random();
            int indice = rnd.Next(0, 5);
            return array[indice];
        }
        static string ParseaColonia(string colonia) {
            if (colonia.Contains("COYOL")) {
                return "El Coyol";
            }
            if (colonia.Contains("VOLCANES")) {
                return "Veracruz";
            }
            if (colonia.Contains("VIRGINIA")) {
                return "Veracruz";
            }
            if (colonia.Contains("BUENAVISTA") || colonia.Contains("BUENA VISTA")) {
                return "BuenaVista";
            }
            if (colonia.Contains("GUADALUPE")) {
                return "Colonia Guadalupe";
            }
            if (colonia.Contains("RIO MEDIO")) {
                return "Río Medio";
            }
            if (colonia.Contains("ABRIL")) {
                return "Veracruz";
            }
            if (colonia.Contains("LOPEZ MATEOS") || colonia.Contains("MATEOS"))
            {
                return "Adolfo López Mateos";
            }
            if (colonia.Contains("MEXICO")) {
                return "Colonia México";
            }
            if (colonia.Contains("MAYO")){
                return "Primero de Mayo";
            }
            if (colonia.Contains("PUENTE")) {
                return "Fraccionamiento Puente Moreno";
            }
            if (colonia.Contains("ALEMAN")) {
                return "Colonia Miguel Alemán";
            }
            if (colonia.Contains("LAURELES")) {
                return "Los Laureles";
            }
            if(colonia.Contains("VEGAS")){
                return "Las Vegas";   
            }
            if (colonia.Contains("TARIMOYA")) {
                return "Tarimoya";
            }
            if (colonia.Contains("PINOS")) {
                return "Fraccionamiento Geovillas los Pinos";
            }
            if (colonia.Contains("HIDALGO")) {
                return "Miguel Hidalgo y Costilla";
            }
            if (colonia.Contains("JUAREZ")) {
                return "Colonia Benito Juárez";
            }
            if (colonia.Contains("VERACRUZANA")) {
                return "Colonia Revolucionaria Veracruzana (Fidel Herrera Beltrán)";
            }
            if (colonia.Contains("VALLE ALTO")) {
                return "Valle Alto";
            }
            if (colonia.Contains("Tejar")) {
                return "Medellín";
            }
            if (colonia.Contains("POCHOTA")) {
                return "Colonia Nueva la Pochota";
            }
            if (colonia.Contains("HEROES")) {
                return "Colonia Niños Héroes";
            }
            if (colonia.Contains("CHAPULTEPEC"))
            {
                return "Chapultepec";
            }
            if (colonia.Contains("BAJADAS")) {
                return "Ampliación las Bajadas";
            }
            if (colonia.Contains("VERGEL")) {
                return "El Vergel";
            }
            if (colonia.Contains("BRISAS")) {
                return "Colonia las Brisas";
            }
            if (colonia.Contains("REFORMA")) {
                return "Colonia la Reforma";
            }
            if (colonia.Contains("SOTAVENTO")) {
                return "Hacienda Sotavento";
            }
            if (colonia.Contains("LA HERRADURA")) {
                return "La Herradura";
            }
            if (colonia.Contains("MALIBRAN"))
            {
                return "Colonia Malibrán";
            }
            if (colonia.Contains("ALMENDROS")) {
                return "Colonia los Almendros";
            }
            if (colonia.Contains("LAZARO CARDENAS")) {
                return "Colonia Lázaro Cárdenas";
            }
            if (colonia.Contains("LOPEZ ARIAS")) {
                return "Colonia Fernando López Arias";
            }
            if (colonia.Contains("GUTIERREZ BARRIOS")) {
                return "Colonia Fernando Gutiérrez Barrios";
            }
            if (colonia.Contains("SANTA FE")) {
                return "Colinas de Santa Fe";
            }
            if (colonia.Contains("ESPERANZA")) {
                return "Colonia Esperanza";
            }
            if (colonia.Contains("ZARAGOZA")) {
                return "Colonia Ignacio Zaragoza";
            }
            if (colonia.Contains("ISIDRO")) {
                return "Colonia San Isidro";
            }
            if (colonia.Contains("NOVIEMBRE")) {
                return "Colonia Veinte de Noviembre";
            }
            if (colonia.Contains("SEPTIEMBRE")) {
                return "Dieciséis de Septiembre";
            }
            return "Veracruz";
        }
        static string ParseaMunicipio(string municipio) {
            if (municipio == "") {
                return "Veracruz";
            }
            if (municipio == "VER" || municipio == "VERACRIUZ" || municipio == "VERCARUZ" || municipio == "VERACRUZ" || municipio == "VARACRUZ" || municipio == "VERCRUZ" || municipio == "VERACRUZ9" || municipio == "VERACURZ" || municipio == "V" || municipio == "0" || municipio == "VERRACRUZ")
            {
                return "Veracruz";
            }
           
            if (municipio == "BOCA DEL RIO"|| municipio=="B") { 
            return "Boca del Río";
            }
            if (municipio == "MEDELLIN" || municipio=="MEDELLIN DE BRAVO"|| municipio== "EL TEJAR")
            {
                return "Medellín de Bravo";
            }
            if (municipio == "IGNACIO DE LA LLAVE" || municipio == "IGN. DE LA LLAVE") {
                return "Ignacio de la Llave";
            }
            if (municipio == "MATA DE PITA") {
                return "Veracruz";
            }
            if (municipio == "NARANJOS AMATLAN")
            {
                return "Naranjos Amatlán";
            }
            if (municipio == "TLALIXCOYAN" || municipio == "TLALIXCOYA,VER")
            {
                return "Tlalixcoyan";
            }
            if (municipio == "LERDO DE TEJADA" || municipio=="LERDO") {
                return "Lerdo de Tejada";
            }
            if (municipio == "MATA COCUITE")
            {
                return "Tlalixcoyan";
            }
            if (municipio == "MANLIO FABIO ALTAMIRANO") {
                return "Manlio Fabio Altamirano";
            }
            if (municipio == "INDEPENDENCIA") {
                return "Martínez de la Torre";
            }
            return municipio;
        }
        static DateTime CalculaFechaDeNacimiento(string fecha, int edad = 0)
        {
          DateTime FechaNacimiento = new DateTime();
            if (edad != 0 && fecha == "")
            {
                Random gen = new Random();
                DateTime start = new DateTime(2015, 1, 1);
                start = start.AddYears(-edad);
                int range = 300;
                return start.AddDays(gen.Next(range));
            }
            if (fecha != "") {

                FechaNacimiento = Convert.ToDateTime(fecha);
                return FechaNacimiento;
            }
            return FechaNacimiento;
        }
        static DateTime ParseaYRetornaFecha(string fecha) {
            if (fecha == "" || fecha=="0000-00-00")
            {
                Random gen = new Random();
                DateTime start = new DateTime(2015, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(gen.Next(range));
            }
            else {
                DateTime fechalevantamiento = new DateTime();
                fechalevantamiento = Convert.ToDateTime(fecha);
                fechalevantamiento = new DateTime(2015, fechalevantamiento.Month, fechalevantamiento.Day);
                return fechalevantamiento;
            }

        }
        static List<ClsFamiliares> CrearListaFamiliares() {
            var render = new StreamReader(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\Familiares.csv"));
 
            List<ClsFamiliares> listaFamiliares = new List<ClsFamiliares>();
            int contadormalos=0;
            while (!render.EndOfStream) {
                var line = render.ReadLine();
                var values = line.Split(',').ToArray();
                if (values.Length <= 15)
                {
                    ClsFamiliares registroFamiliares = new ClsFamiliares();
                    registroFamiliares.IFEFamiliares = ParseaCadena(values[0]);
                    registroFamiliares.NombreFamiliar = ParseaCadena(values[1]);
                    registroFamiliares.ApellidosFamiliar = ParseaCadena(values[2]);
                    registroFamiliares.SexoFamiliar = ParseaCadena(values[3]);
                    registroFamiliares.FechaNacimiento = ParseaFecha(values[4]);
                    registroFamiliares.EdadFamiliar = Convert.ToInt32(ParseaCadenaNumerica(values[5]));
                    registroFamiliares.AnalfabetismoFamiliar = ParseaCadena(values[6]);
                    registroFamiliares.EscolaridadFamiliar = ParseaCadena(values[7]);
                    registroFamiliares.OcupacionFamiliar = ParseaCadena(values[8]);
                    registroFamiliares.Parentesco = ParseaCadena(values[9]);
                    registroFamiliares.PesoFamiliar = Convert.ToDouble(ParseaCadenaNumerica(values[10]));
                    registroFamiliares.EstaturaFamiliar = Convert.ToDouble(ParseaCadenaNumerica(values[11]));
                    registroFamiliares.TallaFamiliar = Convert.ToInt32(ParseaCadenaNumerica(values[12]));
                    registroFamiliares.Discapacidad = ParseaCadena(values[13]);
                    registroFamiliares.IdFamiliar = Convert.ToInt32(ParseaCadenaNumerica(values[14]));
                    listaFamiliares.Add(registroFamiliares);
                }
                else {
                    contadormalos += 1;
                }
            }
            return listaFamiliares;
        }
        static List<ClsTitular> CrearListaTitular() {
            var reader = new StreamReader(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\TITULAR.csv"));
            
            List<ClsTitular> listaTitular = new List<ClsTitular>();
            var contadormalos = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var values = line.Split(',').ToArray();
                ClsTitular registroTitular = new ClsTitular();
                if (values.Length <= 27)
                {
                   
                    registroTitular.FechaIngreso = ParseaFecha(values[0]);
                    registroTitular.NombreTitular = ParseaCadena(values[1]);
                    registroTitular.ApellidoTitular = ParseaCadena(values[2]);
                    registroTitular.IFETitular = ParseaCadena(values[3]);
                    registroTitular.Sexo = ParseaCadena(values[4]);
                    registroTitular.Domicilio = ParseaCadena(values[5]);
                    registroTitular.IDColonia = Convert.ToInt32(ParseaCadenaNumerica(values[6]));
                    registroTitular.Colonia = ParseaCadena(values[7]);
                    registroTitular.Municipio = ParseaCadena(values[8]);
                    registroTitular.Estado = ParseaCadena(values[9]);
                    registroTitular.CodigoPostal = ParseaCadenaNumerica(values[10]);
                    registroTitular.Telefono = ParseaCadenaNumerica(values[11]);
                    registroTitular.Celular = ParseaCadenaNumerica(values[12]);
                    registroTitular.EdadTitular = Convert.ToInt32(ParseaCadenaNumerica(values[13]));
                    registroTitular.FechaDeNacimiento = ParseaFecha(values[14]);
                    registroTitular.EstadoCivil = ParseaCadena(values[15]);
                    registroTitular.Analfabetismo = ParseaCadena(values[16]);
                    registroTitular.Ocupacion = ParseaCadena(values[17]);
                    registroTitular.Escolaridad = ParseaCadena(values[18]);
                    registroTitular.Peso = Convert.ToDouble(ParseaCadenaNumerica(values[19]));
                    registroTitular.Estatura = Convert.ToDouble(ParseaCadenaNumerica(values[20]));
                    registroTitular.Talla = Convert.ToInt32(ParseaCadenaNumerica(values[21]));
                    registroTitular.TipoDeSangre = ParseaCadena(values[22]);
                    registroTitular.Familias = Convert.ToInt32(ParseaCadenaNumerica(values[23]));
                    registroTitular.Discapacidad = ParseaCadena(values[24]);
                    registroTitular.CURPTitular = ParseaCadena(values[26]);
                }
                else {
                    contadormalos += 1;
                }
                listaTitular.Add(registroTitular);
            }
            return listaTitular;
        }
        static List<ClsVivienda> CreaListaVivienda() {
            var reader = new StreamReader(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\VIVIENDA.csv"));
           
            List<ClsVivienda> listaVivienda = new List<ClsVivienda>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').ToArray();
                ClsVivienda registroVivienda = new ClsVivienda();
                registroVivienda.IFE = ParseaCadenaNumerica(values[0]);
                registroVivienda.Economico = ParseaCadena(values[1]);
                registroVivienda.Material = ParseaCadena(values[2]);
                registroVivienda.Tipo = ParseaCadena(values[3]);
                registroVivienda.ServicioAgua = ParseaCadena(values[4]);
                registroVivienda.ServicioPiso = ParseaCadena(values[5]);
                registroVivienda.ServicioDrenaje = ParseaCadena(values[6]);
                registroVivienda.ServicioEnergia = ParseaCadena(values[7]);
                registroVivienda.Refrigerador = Convert.ToInt32(ParseaCadenaNumerica(values[8]));
                registroVivienda.Lavadora = Convert.ToInt32(ParseaCadenaNumerica(values[9]));
                registroVivienda.Television = Convert.ToInt32(ParseaCadenaNumerica(values[10]));
                registroVivienda.Telefono = Convert.ToInt32(ParseaCadenaNumerica(values[11]));
                registroVivienda.DVD = Convert.ToInt32(ParseaCadenaNumerica(values[12]));
                registroVivienda.Microondas = Convert.ToInt32(ParseaCadenaNumerica(values[13]));
                registroVivienda.Estufa = Convert.ToInt32(ParseaCadenaNumerica(values[14]));
                registroVivienda.Computadora = Convert.ToInt32(ParseaCadenaNumerica(values[15]));
                registroVivienda.Auto = Convert.ToInt32(ParseaCadenaNumerica(values[16]));
                registroVivienda.IngresoMensual = Convert.ToDouble(ParseaCadenaNumerica(values[17]));
                registroVivienda.IDVivienda = Convert.ToInt32(ParseaCadenaNumerica(values[18]));

                listaVivienda.Add(registroVivienda);
            }
            return listaVivienda;
        
        }
        static string ParseaCadenaParaNumeroExterior(string cadena)
        {
            if (cadena == ""|| cadena =="0")
            {
                return "0";
            }
            if (cadena == "NULL")
            {
                return "0";
            }
            var respuesta = Regex.Split(cadena, @"\D+");
                if (respuesta[1] == "")
                {
                    return "0";
                }
                return respuesta[1];
  

        }

        static string ParseaCadenaNumerica(string cadena) 
        {
            if (cadena == "") {
                return "0";
            }
            if (cadena == "NULL") {
                return "0";
            }
            var respuesta=Regex.Split(cadena, @"\D+");
            if (respuesta.Length <= 3)
            {
                if (respuesta[1] == "") {
                    return "0";
                }
                return respuesta[1];
            }
            else {
                StringBuilder cadenarespuesta = new StringBuilder();
                int count = 0;
                foreach (var resp in respuesta) {
                    if (resp != "") {
                        count += 1;
                        if(count==1){
                            cadenarespuesta.Append(resp);
                        }else{
                         cadenarespuesta.Append("."+ resp);
                        }
                        
                    }
                }
                return cadenarespuesta.ToString().Trim();
            }
            
        
        }
        static string ParseaNombreCalle(string cadena)
        {
            if (cadena == "") {
                return "";
            }
            var respuesta = Regex.Split(cadena, @"\W+");
            StringBuilder cadenarespuesta = new StringBuilder();
            foreach (var resp in respuesta) {
                if (resp != "") {
                    int result;
                    if (!int.TryParse(resp, out result))
                    {
                        cadenarespuesta.Append(" " + resp);
                    }
                }
            }
            return cadenarespuesta.ToString().Trim();

        }
        static string ParseaCadena(string cadena) {
            if (cadena == "NULL" || cadena=="") {
                return "";
            }
            var respuesta = Regex.Split(cadena, @"\W+");
            if (respuesta.Length <= 3)
            {
                return respuesta[1];
            }
            else {
                StringBuilder cadenarespuesta= new StringBuilder();
                foreach (var resp in respuesta) {
                    if (resp != "") {
                        cadenarespuesta.Append(" " + resp);
                    }
                
                }
                return cadenarespuesta.ToString().Trim();
            }
            
        }
        static string ParseaFecha(string cadena) 
        {
            if (cadena == "NULL" || cadena=="") {
                return "";
            }
            var respuesta = Regex.Split(cadena, @"\W+");
            StringBuilder cadenaRespuesta = new StringBuilder();
            int count = 0;
            foreach (var resp in respuesta) {
                if (resp != "") 
                {
                    count += 1;
                    cadenaRespuesta.Append(resp);
                    if (count < 3) {
                        cadenaRespuesta.Append("-");
                    }
                }
            }
            return cadenaRespuesta.ToString().Trim();
        }

    }
}
