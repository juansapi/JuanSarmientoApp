using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using JuanSarmientoApp.Models;

namespace JuanSarmientoApp.Controllers
{
    public class PersonaController : Controller
    {
        private readonly AppDbContext _context;

        public PersonaController(AppDbContext context)
        {
            _context = context;
        }

        //  Listar todas las personas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persona.ToListAsync());
        }

        //  Formulario para crear una nueva persona
        public IActionResult Create()
        {
            ViewBag.Habilidades = _context.Habilidad.ToList();
            ViewBag.Competencias = _context.Competencia.ToList();
            return View();
        }

        //  Guardar nueva persona   
        [HttpPost]
        public IActionResult Create(Persona persona, List<Educacion> Educacion, List<ExperienciaLaboral> ExperienciaLaboral, List<int> HabilidadesSeleccionadas, List<int> CompetenciasSeleccionadas)
        {

            bool existePersona = _context.Persona.Any(p => p.CorreoElectronico == persona.CorreoElectronico);

            if (existePersona)
            {
                TempData["ErrorMessage"] = "Ya existe una persona registrada con este correo electrónico.";
                return RedirectToAction("Index"); // Redirigir nuevamente al formulario
            }

            //  Guardar solo Persona primero
            _context.Persona.Add(persona);
            _context.SaveChanges(); // Se genera PersonaID            

            foreach (var edu in Educacion)
            {
                bool existe = _context.Educacion.Any(e =>
                    e.PersonaID == persona.PersonaID &&
                    e.Institucion == edu.Institucion &&
                    e.TituloObtenido == edu.TituloObtenido &&
                    e.FechaGraduacion == edu.FechaGraduacion);

                if (!existe) // Solo guardar si no existe
                {
                    edu.PersonaID = persona.PersonaID;
                    _context.Educacion.Add(edu);
                }
            }

            foreach (var exp in ExperienciaLaboral)
            {
                bool existe = _context.ExperienciaLaboral.Any(e =>
                    e.PersonaID == persona.PersonaID &&
                    e.Empresa == exp.Empresa &&
                    e.Cargo == exp.Cargo &&
                    e.FechaInicio == exp.FechaInicio);

                if (!existe) // Solo guardar si no existe
                {
                    exp.PersonaID = persona.PersonaID;
                    _context.ExperienciaLaboral.Add(exp);
                }
            }

            //  Guardar Habilidades y Competencias
            if (HabilidadesSeleccionadas != null)
            {
                foreach (var habilidadID in HabilidadesSeleccionadas.Distinct())
                {
                    _context.PersonaHabilidad.Add(new PersonaHabilidad
                    {
                        PersonaID = persona.PersonaID,
                        HabilidadID = habilidadID
                    });
                }
            }

            if (CompetenciasSeleccionadas != null)
            {
                foreach (var competenciaID in CompetenciasSeleccionadas.Distinct())
                {
                    _context.PersonaCompetencia.Add(new PersonaCompetencia
                    {
                        PersonaID = persona.PersonaID,
                        CompetenciaID = competenciaID
                    });
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Persona creada exitosamente.";
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .Include(p => p.PersonaHabilidades).ThenInclude(ph => ph.Habilidad) // Cargar Habilidades
                .Include(p => p.PersonaCompetencias).ThenInclude(pc => pc.Competencia) // Cargar Competencias
                .Include(p => p.Educacion) // Cargar Educación
                .Include(p => p.ExperienciaLaboral) // Cargar Experiencia Laboral
                .FirstOrDefaultAsync(m => m.PersonaID == id);

            if (persona == null)
            {
                return NotFound();
            }

            return View(persona); // Retornar la vista con la persona encontrada
        }


        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }            

            var persona = await _context.Persona
                                .Include(p => p.Educacion)
                                .Include(p => p.ExperienciaLaboral)
                                .Include(p => p.PersonaHabilidades)
                                    .ThenInclude(ph => ph.Habilidad)
                                .Include(p => p.PersonaCompetencias)
                                    .ThenInclude(pc => pc.Competencia)
                                .FirstOrDefaultAsync(p => p.PersonaID == id);

            if (persona == null)
            {
                return NotFound();
            }

            ViewBag.Educaciones = await _context.Educacion.ToListAsync();
            ViewBag.ExperienciasLaborales = await _context.ExperienciaLaboral.ToListAsync();
            ViewBag.Habilidades = await _context.Habilidad.ToListAsync();
            ViewBag.Competencias = await _context.Competencia.ToListAsync();            
            return View(persona);
        }       

        [HttpPost]
        public async Task<IActionResult> Edit(int PersonaID, string Nombre, string Direccion, string Telefono, string CorreoElectronico,
                                       List<Educacion> Educacion, List<ExperienciaLaboral> ExperienciaLaboral, List<int> HabilidadesSeleccionadas, List<int> CompetenciasSeleccionadas)
        {
            var persona = await _context.Persona
                .Include(p => p.Educacion)
                .Include(p => p.ExperienciaLaboral)
                .Include(p => p.PersonaHabilidades)
                .Include(p => p.PersonaCompetencias)
                .FirstOrDefaultAsync(p => p.PersonaID == PersonaID);

            if (persona == null)
            {
                return NotFound();
            }

            //  Actualizar los datos principales de la persona
            persona.Nombre = Nombre;
            persona.Direccion = Direccion;
            persona.Telefono = Telefono;
            persona.CorreoElectronico = CorreoElectronico;

            ////  Actualizar Educación (Eliminar las antiguas y agregar las nuevas)
            //_context.Educacion.RemoveRange(persona.Educacion);
            //foreach (var edu in Educacion)
            //{
            //    if (!string.IsNullOrEmpty(edu.Institucion) && !string.IsNullOrEmpty(edu.TituloObtenido))
            //    {
            //        edu.PersonaID = PersonaID;
            //        _context.Educacion.Add(edu);
            //    }
            //}

            ////  Actualizar Experiencia Laboral (Eliminar las antiguas y agregar las nuevas)
            //_context.ExperienciaLaboral.RemoveRange(persona.ExperienciaLaboral);
            //foreach (var exp in ExperienciaLaboral)
            //{
            //    if (!string.IsNullOrEmpty(exp.Empresa) && !string.IsNullOrEmpty(exp.Cargo))
            //    {
            //        exp.PersonaID = PersonaID;
            //        _context.ExperienciaLaboral.Add(exp);
            //    }
            //}

            //  Actualizar Educación
            var educacionesExistentes = persona.Educacion.ToList();
            foreach (var edu in Educacion)
            {
                if (edu.EducacionID > 0) // Si ya existía, actualizarla
                {
                    var eduExistente = educacionesExistentes.FirstOrDefault(e => e.EducacionID == edu.EducacionID);
                    if (eduExistente != null)
                    {
                        eduExistente.Institucion = edu.Institucion;
                        eduExistente.TituloObtenido = edu.TituloObtenido;
                        eduExistente.FechaGraduacion = edu.FechaGraduacion;
                    }
                }
                else // Si es nueva, agregarla
                {
                    edu.PersonaID = PersonaID;
                    _context.Educacion.Add(edu);
                }
            }

            // Eliminar registros que no llegaron en la lista actualizada
            var idsEducacionRecibidos = Educacion.Select(e => e.EducacionID).ToList();
            var educacionesParaEliminar = educacionesExistentes.Where(e => !idsEducacionRecibidos.Contains(e.EducacionID)).ToList();
            _context.Educacion.RemoveRange(educacionesParaEliminar);

            //  Actualizar Experiencia Laboral
            var experienciasExistentes = persona.ExperienciaLaboral.ToList();
            foreach (var exp in ExperienciaLaboral)
            {
                if (exp.ExperienciaID > 0) // Si ya existía, actualizarla
                {
                    var expExistente = experienciasExistentes.FirstOrDefault(e => e.ExperienciaID == exp.ExperienciaID);
                    if (expExistente != null)
                    {
                        expExistente.Empresa = exp.Empresa;
                        expExistente.Cargo = exp.Cargo;
                        expExistente.FechaInicio = exp.FechaInicio;
                        expExistente.FechaFin = exp.FechaFin;
                        expExistente.Responsabilidades = exp.Responsabilidades;
                    }
                }
                else // Si es nueva, agregarla
                {
                    exp.PersonaID = PersonaID;
                    _context.ExperienciaLaboral.Add(exp);
                }
            }

            // Eliminar registros de experiencia que no llegaron en la lista actualizada
            var idsExperienciaRecibidos = ExperienciaLaboral.Select(e => e.ExperienciaID).ToList();
            var experienciasParaEliminar = experienciasExistentes.Where(e => !idsExperienciaRecibidos.Contains(e.ExperienciaID)).ToList();
            _context.ExperienciaLaboral.RemoveRange(experienciasParaEliminar);

            //  Actualizar Habilidades (Eliminar las antiguas y agregar las nuevas)
            persona.PersonaHabilidades.Clear();
            if (HabilidadesSeleccionadas != null)
            {
                foreach (var habilidadId in HabilidadesSeleccionadas)
                {
                    persona.PersonaHabilidades.Add(new PersonaHabilidad { PersonaID = PersonaID, HabilidadID = habilidadId });
                }
            }

            //  Actualizar Competencias (Eliminar las antiguas y agregar las nuevas)
            persona.PersonaCompetencias.Clear();
            if (CompetenciasSeleccionadas != null)
            {
                foreach (var competenciaId in CompetenciasSeleccionadas)
                {
                    persona.PersonaCompetencias.Add(new PersonaCompetencia { PersonaID = PersonaID, CompetenciaID = competenciaId });
                }
            }

            //  Guardar los cambios en la BD
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Datos actualizados correctamente.";            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int PersonaID)
        {
            var persona = await _context.Persona
                .Include(p => p.Educacion)
                .Include(p => p.ExperienciaLaboral)
                .Include(p => p.PersonaHabilidades)
                .Include(p => p.PersonaCompetencias)
                .FirstOrDefaultAsync(p => p.PersonaID == PersonaID);

            if (persona == null)
            {
                return NotFound();
            }

            //  Eliminar las relaciones antes de eliminar la persona
            _context.Educacion.RemoveRange(persona.Educacion);
            _context.ExperienciaLaboral.RemoveRange(persona.ExperienciaLaboral);
            _context.PersonaHabilidad.RemoveRange(persona.PersonaHabilidades);
            _context.PersonaCompetencia.RemoveRange(persona.PersonaCompetencias);

            _context.Persona.Remove(persona); //  Finalmente, eliminar la persona
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Persona eliminada exitosamente.";
            return RedirectToAction("Index");
        }

    }
}
