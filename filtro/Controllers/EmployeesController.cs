using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using filtro.Models;
using filtro.Data;

namespace filtro.Controllers
{
    public class EmployeesController : Controller
    {
          private readonly DBContext _dbcontext;
          private readonly IWebHostEnvironment _hostingEnviroment;

        public EmployeesController(DBContext context, IWebHostEnvironment hostingEnviroment)
        {
            _dbcontext = context;
            _hostingEnviroment = hostingEnviroment;
        }

        /////////////// BUSCAR Y LISTAR ///////////////
        public async Task<IActionResult> Index(string buscar)
        {
              var employs = from employ in _dbcontext.Employees select employ;
            if (!string.IsNullOrEmpty(buscar))
            {
                employs = employs.Where(e => e.Names.Contains(buscar) || e.Area.Contains(buscar) || e.CivilStatus.Contains(buscar) || e.About.Contains(buscar) );
            }

            return View(await employs.ToListAsync());
        }
        /////////////// CREAR ///////////////
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Employ employ, IFormFile archivo, IFormFile hojaVida)
        {
            var rutaDestino = Path.Combine( _hostingEnviroment.WebRootPath,"images/",archivo.FileName);
            var rutaDestino2 = Path.Combine( _hostingEnviroment.WebRootPath,"documents/",hojaVida.FileName);

            using (var destino = new FileStream(rutaDestino, FileMode.Create))
            {
                await archivo.CopyToAsync(destino);
            }
            using (var hojacvida = new FileStream(rutaDestino2, FileMode.Create))
            {
                await hojaVida.CopyToAsync(hojacvida);
            }
            employ.Cv=hojaVida.FileName; 

            employ.ProfilePicture = "/images/"+ archivo.FileName;

            _dbcontext.Employees.Add(employ);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("index");
        }
        /////////////// EDITAR ///////////////
        public async Task<IActionResult> Edit(int? id )
        {
            return View(await _dbcontext.Employees.FirstOrDefaultAsync(e => e.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employ employ,IFormFile archivo, IFormFile hojaVida)
        {
            var rutaDestino = Path.Combine( _hostingEnviroment.WebRootPath,"images/",archivo.FileName);
            var rutaDestino2 = Path.Combine( _hostingEnviroment.WebRootPath,"documents/",hojaVida.FileName);

            using (var destino = new FileStream(rutaDestino, FileMode.Create))
            {
                await archivo.CopyToAsync(destino);
            }
            using (var hojacvida = new FileStream(rutaDestino2, FileMode.Create))
            {
                await hojaVida.CopyToAsync(hojacvida);
            }
            employ.Cv=hojaVida.FileName; 

            employ.ProfilePicture = "/images/"+ archivo.FileName;

            _dbcontext.Employees.Update(employ);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
         /////////////// ELIMINAR ///////////////
        public async Task<IActionResult> Delete(int? id)
        {
            var employ = await _dbcontext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            _dbcontext.Employees.Remove(employ);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction("index"); 
        }
        /// DETALLES ///////////////
        public async Task<IActionResult> Details(int? id)
        {
            return View( await _dbcontext.Employees.FindAsync(id));
        }
    }
}