using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using filtro.Models;
using System.Reflection;
using filtro.Data;


namespace filtro.Controllers
{
    public class JobsController : Controller
    {
        private readonly DBContext _context;
        private readonly IWebHostEnvironment _hostingEnviroment;
    

        public JobsController(DBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnviroment = hostingEnvironment;
        }

        /////////////// BUSCAR Y LISTAR ///////////////
        public async Task<IActionResult> Index(string buscar)
        {
            var jobs = from job in _context.Jobs select job;
            if (!string.IsNullOrEmpty(buscar))
            {
                jobs = jobs.Where(j => j.NameCompany.Contains(buscar) || j.OfferTitle.Contains(buscar) || j.Description.Contains(buscar) /* || j.Salary.Contains(buscar) */ );
            }

            return View(await jobs.ToListAsync());
        }
        /////////////// CREAR ///////////////
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> Create(Job job,IFormFile archivo )
        {
            var rutaDestino = Path.Combine( _hostingEnviroment.WebRootPath,"images/",archivo.FileName);

            using (var destino = new FileStream(rutaDestino, FileMode.Create))
            {
                await archivo.CopyToAsync(destino);
            }
            job.LogoCompany = "/images/"+ archivo.FileName;

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /////////////// EDITAR ///////////////
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Job job,IFormFile archivo)
        {
            var rutaDestino = Path.Combine( _hostingEnviroment.WebRootPath,"images/",archivo.FileName);

            using (var destino = new FileStream(rutaDestino, FileMode.Create))
            {
                await archivo.CopyToAsync(destino);
            }
            job.LogoCompany = "/images/"+ archivo.FileName;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        /////////////// ELIMINAR ///////////////
        public async Task<IActionResult> Delete(int? id)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction("index"); 
        }

        /////////////// DETALLES ///////////////
        public async Task<IActionResult> Details(int? id)
        {
            return View( await _context.Jobs.FindAsync(id));
        }
        

    }
}