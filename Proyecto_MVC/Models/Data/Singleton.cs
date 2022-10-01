using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibliotecaGenerica;

namespace G4_Proyecto.Models.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public List<Paciente> ListaPaciente;
        public List<Paciente> Encontrado;
        public List<Paciente> PacientePorDia;

        public List<Paciente> Limpieza;
        public List<Paciente> AuxLista;
        public List<Paciente> Ortodoncia;
        public List<Paciente> Caries;
        public List<Paciente> Otros;

        public ArbolAVL<Paciente> DPIPacienteAVL;
        public ArbolAVL<Paciente> NombrePacienteAVL;
        
        public ArbolAVL<Paciente> Fecha1;



        private Singleton()
        {
            ListaPaciente = new List<Paciente>();
            Encontrado = new List<Paciente>();
            PacientePorDia = new List<Paciente>();

            DPIPacienteAVL = new ArbolAVL<Paciente>();
            NombrePacienteAVL = new ArbolAVL<Paciente>();

            Fecha1 = new ArbolAVL<Paciente>();

            Limpieza = new List<Paciente>();
            AuxLista = new List<Paciente>();
            Ortodoncia = new List<Paciente>();
            Caries = new List<Paciente>();
            Otros = new List<Paciente>();
        }


        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}
