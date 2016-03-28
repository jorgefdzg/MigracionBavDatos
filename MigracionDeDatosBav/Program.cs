﻿using System;
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
        }
        static List<ClsEstudioSocioEconomico> CreaListaEstudiosSocioEconomicos(List<ClsVivienda> listaVivienda, List<ClsTitular> listaTitular)
        {
            List<ClsEstudioSocioEconomico> listaEstudioSocioEconomico = new List<ClsEstudioSocioEconomico>();
            int contadorestudio=0;
            foreach (ClsTitular titular in listaTitular) 
            {
                ClsEstudioSocioEconomico estudio = new ClsEstudioSocioEconomico();
                ClsVivienda vivienda = listaVivienda.FirstOrDefault(x => x.IFE == titular.IFETitular);
                contadorestudio+=1;
                estudio.NumeroEstudio = contadorestudio;
                estudio.Programa = "MESA BAV";
                estudio.NombreGrupo = "MESA BAV";
                estudio.FechaLevantamiento = ParseaYRetornaFecha(titular.FechaIngreso);
                estudio.Estado = "Veracruz";
                estudio.Municipio =ParseaMunicipio(titular.Municipio);
                estudio.Localidad = titular.Colonia;
            }
            return listaEstudioSocioEconomico;
        }
        static string ParseaColonia(string municipio, string colonia) {
            if (colonia.Contains("COYOL")) {
                return "El Coyol";
            }
            if (colonia.Contains("VOLCANES")) {
                return "Veracruz";
            }
            if (colonia.Contains("VIRGINIA")) {
                return "Veracruz";
            }
            if (colonia.Contains("BUENAVISTA")) {
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
            if (colonia.Contains("")) { 
            
            }

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
        static DateTime ParseaYRetornaFecha(string fecha) {
            if (fecha == "")
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
            ClsFamiliares registroFamiliares = new ClsFamiliares();
            List<ClsFamiliares> listaFamiliares = new List<ClsFamiliares>();
            int contadormalos=0;
            while (!render.EndOfStream) {
                var line = render.ReadLine();
                var values = line.Split(',').ToArray();
                if (values.Length <= 15)
                {
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
            ClsTitular registroTitular = new ClsTitular();
            List<ClsTitular> listaTitular = new List<ClsTitular>();
            var contadormalos = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var values = line.Split(',').ToArray();
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
            ClsVivienda registroVivienda = new ClsVivienda();
            List<ClsVivienda> listaVivienda = new List<ClsVivienda>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').ToArray();
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
