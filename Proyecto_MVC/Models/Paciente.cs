using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace G4_Proyecto.Models
{
    public class Paciente : IComparable
    {
        [Required(ErrorMessage = "Rellene el campo")]
        [Display(Name = "Nombre completo del paciente")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Rellene el campo del DPI")]
        [Range(1000000000000, 9999999999999, ErrorMessage = "El Id es invalido")]
        [Display(Name = "ID")]
        public long ID { get; set; }

        [Required(ErrorMessage = "Rellene el campo de Edad")]
        [Range(0, 110, ErrorMessage = "La edad es invalida")]
        [Display(Name = "Edad")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "Número de telefono es invalido")]
        [Display(Name = "Telefono")]
        [Range(10000000, 99999999, ErrorMessage = "Número de telefono es invalido")]
        public int Telefono { get; set; }

        [Required(ErrorMessage = "Rellene el campo de la ultima Consulta")]
        [DataType(DataType.Date, ErrorMessage = "Eliga una fecha")]
        [Display(Name = "Ultima Consulta")]
        public DateTime Lconsulta { get; set; }

        [DataType(DataType.Date)]       
        [Display(Name = "Proxima Consulta")]
        public DateTime Pconsulta { get; set; }

        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [Display(Name = "Diagnóstico")]
        public string Diagnostico { get; set; }


        public Comparison<Paciente> InsertarPorDPI = delegate (Paciente Paciente1, Paciente Paciente2)
        {
            return Paciente1.ID.CompareTo(Paciente2.ID);
        };

        public Comparison<Paciente> InsertarPorNombre = delegate (Paciente Paciente1, Paciente Paciente2)
        {
            return Paciente1.Nombre.CompareTo(Paciente2.Nombre);
        };

        public Comparison<Paciente> Fecha = delegate (Paciente Paciente1, Paciente Paciente2)
        {
            return Paciente1.Pconsulta.CompareTo(Paciente2.Pconsulta);
        };


        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
