using APICatalogo.Context;
using APICatalogo.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
   [Route("api/[controller]")] // /api/produtos
   [ApiController]
   public class ProdutosController : ControllerBase
   {
      private readonly AppDbContext _context;

      public ProdutosController(AppDbContext context)
      {
         _context = context;
      }

      // /api/produtos
      [HttpGet]
      public async Task<ActionResult<IEnumerable<Produto>>> Get()
      {
         var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
         if(produtos is null)
         {
            return NotFound("Produtos não encontrados...");
         }
         return produtos;
      }

      [HttpGet("primeiro")] // /api/produtos/primeiro
      [HttpGet("/primeiro")] // /primeiro
      //[HttpGet("{valor:alpha:length(5)}")]
      public async Task<ActionResult<Produto>> GetPrimeiro()
      {
         var produto = await _context.Produtos.FirstOrDefaultAsync();
         if (produto is null)
         {
            return NotFound("Nenhum produto no catálogo...");
         }
         return produto;
      }

      //{nome=Caderno}
      [HttpGet("{id:int:min(1)}/{name=Caderno}", Name="ObterProduto")]
      public ActionResult<Produto> Get([FromQuery]int id, string name, [BindRequired] string nome)
      {
         var parametro = nome;

         var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
      
         if(produto is null)
         {
            return NotFound("Produto não encontrado...");
         }
         return produto;
      }

      [HttpPost]
      public ActionResult Post(Produto produto)
      {
         if (produto is null)
            return BadRequest();

         _context.Produtos.Add(produto);
         _context.SaveChanges();

         return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
      }

      [HttpPut("{id:int}")]
      public ActionResult Put(int id, Produto produto)
      {
         if(id != produto.ProdutoId)
         {
            return BadRequest();
         }

         _context.Entry(produto).State = EntityState.Modified;
         _context.SaveChanges();

         return Ok(produto);
      }

      [HttpDelete("{id:int}")]
      public ActionResult Delete(int id)
      {
         var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
         //var produto = _context.Produtos.Find(id);

         if(produto is null)
         {
            return NotFound("Produto não localizado...");
         }

         _context.Produtos.Remove(produto);
         _context.SaveChanges();

         return Ok(produto);
      }
   }
}
