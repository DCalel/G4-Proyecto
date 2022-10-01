using G4_Proyecto.Models.Data;
using G4_Proyecto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace G4_Proyecto.Controllers
{
    public class ClinicaController : Controller
    {
        // GET: ClinicaController
        public ActionResult Index()
        {
            //LeeArchivo();
            Singleton.Instance.ListaPaciente = Singleton.Instance.NombrePacienteAVL.Recorrido();

            return View(Singleton.Instance.ListaPaciente);
        }

        // GET: ClinicaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClinicaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var NuevoPaciente = new Paciente()
                {
                    Nombre = collection["Nombre"],
                    ID = Convert.ToInt64(collection["ID"]),
                    Edad = Convert.ToInt32(collection["Edad"]),
                    Telefono = Convert.ToInt32(collection["Telefono"]),
                    Lconsulta = Convert.ToDateTime(collection["Lconsulta"]),
                    Categoria = collection["Categoria"],
                    Diagnostico = collection["Diagnostico"],
                };

                Singleton.Instance.DPIPacienteAVL.Agregar(NuevoPaciente, NuevoPaciente.InsertarPorDPI);
                Singleton.Instance.NombrePacienteAVL.Agregar(NuevoPaciente, NuevoPaciente.InsertarPorNombre);

                GuardarDatos(collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public void LeeArchivo()
        {
            string RutaTXT = @"Pacientes.csv";
            var Archivo = new StreamReader(RutaTXT);
            {
                string info = Archivo.ReadToEnd().Remove(0, 101);
                foreach (string Fila in info.Split("\n"))
                {
                    try
                    {
                        var NuevoPaciente = new Paciente()
                        {
                            Nombre = Fila.Split(",")[0],
                            ID = Convert.ToInt64(Fila.Split(",")[1]),
                            Edad = Convert.ToInt32(Fila.Split(",")[2]),
                            Telefono = Convert.ToInt32(Fila.Split(",")[3]),
                            Lconsulta = Convert.ToDateTime(Fila.Split(",")[4]),
                            Pconsulta = Convert.ToDateTime(Fila.Split(",")[5]),
                            Diagnostico = Fila.Split(",")[6],
                            Categoria = Fila.Split(",")[7],
                        };
                        Singleton.Instance.DPIPacienteAVL.Agregar(NuevoPaciente, NuevoPaciente.InsertarPorDPI);
                        Singleton.Instance.NombrePacienteAVL.Agregar(NuevoPaciente, NuevoPaciente.InsertarPorNombre);
                    }
                    catch//Salta a esta parte en caso de que el paciente no tenga agendado un cita proxima
                    {
                        var NuevoPaciente = new Paciente()
                        {
                            Nombre = Fila.Split(",")[0],
                            ID = Convert.ToInt64(Fila.Split(",")[1]),
                            Edad = Convert.ToInt32(Fila.Split(",")[2]),
                            Telefono = Convert.ToInt32(Fila.Split(",")[3]),
                            Lconsulta = Convert.ToDateTime(Fila.Split(",")[4]),
                            Diagnostico = Fila.Split(",")[6],
                            Categoria = Fila.Split(",")[7],
                        };
                        Singleton.Instance.DPIPacienteAVL.Agregar(NuevoPaciente, NuevoPaciente.InsertarPorDPI);
                        Singleton.Instance.NombrePacienteAVL.Agregar(NuevoPaciente, NuevoPaciente.InsertarPorNombre);
                    }
                }
            }
        }

        public void GuardarDatos(IFormCollection collection)
        {
            string[] Inicio = { "Nombre,", "DPI,", "Edad,", "Telefono,", "Fecha de la última consulta,", "Fecha de próxima consulta,", "Categoría,", "Diagnóstico" };
            string[] texto = { "\n" + collection["Nombre"] + ",", collection["ID"] + ",", collection["Edad"] + ",", collection["Telefono"] + ",", collection["Lconsulta"] + ",", collection["Pconsulta"] + ",", collection["Categoria"] + ",", collection["Diagnostico"] };
            string RutaTXT = @"Pacientes.csv";

            //Crea un archivo y lo inicializa
            if (!System.IO.File.Exists(RutaTXT))
            {
                for (int i = 0; i < Inicio.Length; i++)
                {
                    System.IO.File.AppendAllText(RutaTXT, Inicio[i]);
                }
            }

            //Escribe los datos de los pacientes en el archivo
            for (int i = 0; i < texto.Length; i++)
            {
                System.IO.File.AppendAllText(RutaTXT, texto[i]);
            }
        }

        public ActionResult Buscar(string id)
        {
            var BuscarPaciente = new Paciente();
            try
            {
                if (id.All(char.IsDigit))//Comprueba si es un número
                {
                    long DPI = Convert.ToInt64(id);
                    BuscarPaciente.ID = DPI;

                    BuscarPaciente = Singleton.Instance.DPIPacienteAVL.Busqueda(BuscarPaciente, BuscarPaciente.InsertarPorDPI);
                }
                else
                {
                    BuscarPaciente.Nombre = id;

                    BuscarPaciente = Singleton.Instance.NombrePacienteAVL.Busqueda(BuscarPaciente, BuscarPaciente.InsertarPorNombre);
                }

                if (BuscarPaciente == null)
                {
                    TempData["Mensaje"] = "El Paciente No Existe o Coloco Mal Los Datos";
                    return RedirectToAction(nameof(Index));
                }

                return View(BuscarPaciente);
            }
            catch
            {
                TempData["Mensaje"] = "Coloque Datos Para Buscar";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Borrar(long id)
        {
            bool actualizar = true;
            var BorrarPaciente = new Paciente();
            BorrarPaciente.ID = id;

            BorrarPaciente = Singleton.Instance.DPIPacienteAVL.Busqueda(BorrarPaciente, BorrarPaciente.InsertarPorDPI);

            Singleton.Instance.DPIPacienteAVL.Borrar(BorrarPaciente, BorrarPaciente.InsertarPorDPI);
            Singleton.Instance.NombrePacienteAVL.Borrar(BorrarPaciente, BorrarPaciente.InsertarPorNombre);

            Singleton.Instance.ListaPaciente = Singleton.Instance.NombrePacienteAVL.Recorrido();

            for (int i = 0; i < Singleton.Instance.ListaPaciente.Count; i++)
            {
                ActualizarDatos(Singleton.Instance.ListaPaciente[i], actualizar);
                actualizar = false;
            }

            return RedirectToAction(nameof(Index));

        }

        public void ActualizarDatos(Paciente collection, bool actualizar)
        {
            string[] Inicio = { "Nombre,", "DPI,", "Edad,", "Telefono,", "Fecha de la última consulta,", "Fecha de próxima consulta,", "Categoría,", "Diagnóstico" };
            string[] texto = { "\n" + collection.Nombre + ",", collection.ID + ",", collection.Edad + ",", collection.Telefono + ",", collection.Lconsulta + ",", collection.Pconsulta + ",", collection.Categoria + ",", collection.Diagnostico };
            string RutaTXT = @"Pacientes.csv";

            if (actualizar)
            {
                System.IO.File.WriteAllText(RutaTXT, string.Empty);//Limpia el archivo
            }

            for (int i = 0; i < Inicio.Length; i++)
            {
                System.IO.File.AppendAllText(RutaTXT, Inicio[i]);
            }

            //Escribe los datos de los pacientes en el archivo
            for (int i = 0; i < texto.Length; i++)
            {
                System.IO.File.AppendAllText(RutaTXT, texto[i]);
            }
        }

        // GET:     Editar
        public ActionResult ProgramarCita(long id)
        {
            var EditarPaciente = new Paciente();
            EditarPaciente.ID = id;

            EditarPaciente = Singleton.Instance.DPIPacienteAVL.Busqueda(EditarPaciente, EditarPaciente.InsertarPorDPI);
            return View(EditarPaciente);
        }

        // POST: ArbolesController/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProgramarCita(long id, IFormCollection collection)
        {
            try
            {
                var EditarPaciente = new Paciente();
                EditarPaciente.ID = id;

                EditarPaciente = Singleton.Instance.DPIPacienteAVL.Busqueda(EditarPaciente, EditarPaciente.InsertarPorDPI);

                EditarPaciente.Categoria = collection["Categoria"];
                EditarPaciente.Diagnostico = collection["Diagnostico"];


                Singleton.Instance.PacientePorDia.Add(EditarPaciente);

                if (Singleton.Instance.PacientePorDia.Find(x => x.Pconsulta == EditarPaciente.Pconsulta) == EditarPaciente)
                {
                    if (Singleton.Instance.Fecha1.contador < 8)
                    {
                        EditarPaciente.Pconsulta = Convert.ToDateTime(collection["Pconsulta"]);
                        Singleton.Instance.Fecha1.Agregar(EditarPaciente, EditarPaciente.InsertarPorDPI);
                        Singleton.Instance.Fecha1.contador++;
                    }
                    else
                    {
                        Singleton.Instance.Fecha1.contador = 0;
                        TempData["Mensaje"] = "No hay espacio para más paciente este día";
                    }
                }
                else
                {
                    Singleton.Instance.Fecha1.contador = 0;
                }


               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }


        public ActionResult Seguimiento()
        {
            TimeSpan tiempo;
            int meses;
            Singleton.Instance.AuxLista = Singleton.Instance.NombrePacienteAVL.Recorrido();
            Singleton.Instance.Limpieza.Clear();

            for (int i = 0; i < Singleton.Instance.AuxLista.Count; i++)
            {
                tiempo = DateTime.Today.Subtract(Singleton.Instance.AuxLista[i].Lconsulta);

                meses = Convert.ToInt32(tiempo.Days);

                if (Singleton.Instance.AuxLista[i].Categoria == "Limpieza" && meses >= 180 )
                {
                    Singleton.Instance.Limpieza.Add(Singleton.Instance.AuxLista[i]);
                }
            }
            return View(Singleton.Instance.Limpieza);
        }


        public ActionResult Seguimiento_Ortodoncia()
        {
            TimeSpan tiempo;
            int meses;
            Singleton.Instance.AuxLista = Singleton.Instance.NombrePacienteAVL.Recorrido();
            Singleton.Instance.Ortodoncia.Clear();

            for (int i = 0; i < Singleton.Instance.AuxLista.Count; i++)
            {
                tiempo = DateTime.Today.Subtract(Singleton.Instance.AuxLista[i].Lconsulta);

                meses = Convert.ToInt32(tiempo.Days);

                if (Singleton.Instance.AuxLista[i].Categoria == "Ortodoncia" && meses >= 60)
                {
                    Singleton.Instance.Ortodoncia.Add(Singleton.Instance.AuxLista[i]);
                }
            }
            return View(Singleton.Instance.Ortodoncia);
        }


        public ActionResult Seguimiento_Caries()
        {
            TimeSpan tiempo;
            int meses;
            Singleton.Instance.AuxLista = Singleton.Instance.NombrePacienteAVL.Recorrido();
            Singleton.Instance.Caries.Clear();

            for (int i = 0; i < Singleton.Instance.AuxLista.Count; i++)
            {
                tiempo = DateTime.Today.Subtract(Singleton.Instance.AuxLista[i].Lconsulta);

                meses = Convert.ToInt32(tiempo.Days);

                if (Singleton.Instance.AuxLista[i].Categoria == "Caries" && meses >= 120)
                {
                    Singleton.Instance.Caries.Add(Singleton.Instance.AuxLista[i]);
                }
            }
            return View(Singleton.Instance.Caries);
        }

        public ActionResult Seguimiento_Otros()
        {
            TimeSpan tiempo;
            int meses;
            Singleton.Instance.AuxLista = Singleton.Instance.NombrePacienteAVL.Recorrido();
            Singleton.Instance.Otros.Clear();

            for (int i = 0; i < Singleton.Instance.AuxLista.Count; i++)
            {
                tiempo = DateTime.Today.Subtract(Singleton.Instance.AuxLista[i].Lconsulta);

                meses = Convert.ToInt32(tiempo.Days);

                if (Singleton.Instance.AuxLista[i].Categoria == "Otros" && meses >= 180)
                {
                    Singleton.Instance.Otros.Add(Singleton.Instance.AuxLista[i]);
                }
            }
            return View(Singleton.Instance.Otros);
        }



    }
}