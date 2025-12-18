using APICatalogo.Context;
using APICatalogo.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
   [Route("[controller]")]
   [ApiController] 
   public class CategoriasController : ControllerBase
   {

      private readonly AppDbContext _context;

      public CategoriasController(AppDbContext context)
      {
         _context = context;
      }

      [HttpGet("produtos")]
      public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
      {
         try
         {
            return _context.Categorias.Include(p => p.Produtos).Where(c => c.CategoriaId <= 5).AsNoTracking().ToList();
         }
         catch (Exception)
         {
            return StatusCode(StatusCodes.Status500InternalServerError,
               "Ocorreu um problema ao tratar a sua solicitação.");
         }
      }

      [HttpGet]
      public ActionResult<IEnumerable<Categoria>> Get()
      {
         try
         {
            //throw new DataMisalignedException();
            return _context.Categorias.AsNoTracking().ToList();
         }
         catch (Exception)
         {
            return StatusCode(StatusCodes.Status500InternalServerError, 
               "Ocorreu um problema ao tratar a sua solicitação.");
         }
      }

      [HttpGet("{id:int}", Name="ObterCategoria")]
      public ActionResult<Categoria> Get(int id)
      {
         try
         {
            var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
               NotFound($"Categoria id = {id} não encontrada...");
            }
            return categoria;
         }
         catch (Exception)
         {
            return StatusCode(StatusCodes.Status500InternalServerError,
               "Ocorreu um problema ao tratar a sua solicitação.");
         }
      }

      [HttpPost]
      public ActionResult Post(Categoria categoria)
      {
         try
         {
            if (categoria is null)
            {
               return BadRequest("Dados inválidos");
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
         }
         catch (Exception)
         {
            return StatusCode(StatusCodes.Status500InternalServerError,
               "Ocorreu um problema ao tratar a sua solicitação.");
         }
      }

      [HttpPut("{id:int}")]
      public ActionResult Put(int id, Categoria categoria)
      {
         try
         {
            if (id != categoria.CategoriaId)
            {
               return BadRequest("Dados inválidos");
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
         }
         catch (Exception)
         {
            return StatusCode(StatusCodes.Status500InternalServerError,
               "Ocorreu um problema ao tratar a sua solicitação.");
         }
      }

      [HttpDelete("{id:int}")]
      public ActionResult Delete(int id)
      {
         try
         {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
               return NotFound($"Categoria com id = {id} não encontrada...");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
         }
         catch (Exception)
         {
            return StatusCode(StatusCodes.Status500InternalServerError,
               "Ocorreu um problema ao tratar a sua solicitação.");
         }
      }
   }
}
