using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Exceptions;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            // return Ok();
            try
            {
                var tarefa = _context.Set<Tarefa>().FirstOrDefault(x => x.Id.Equals(id))
                    ?? throw new NotFoundException($"Tarefa de Id: {id} não encontrada.");
                
                return Ok(tarefa);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            var tarefas = _context.Set<Tarefa>().ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            try
            {
                var tarefa = _context.Set<Tarefa>().FirstOrDefault(x => x.Titulo.Equals(titulo))
                    ?? throw new NotFoundException($"Tarefa com o titulo: {titulo} não encontrada.");

                return Ok(tarefa);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            try
            {
                var tarefa = _context.Set<Tarefa>().FirstOrDefault(x => x.Data.Equals(data))
                    ?? throw new NotFoundException($"Tarefa com a data: {data} não encontrada.");

                return Ok(tarefa);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            try
            {
                var tarefa = _context.Set<Tarefa>().FirstOrDefault(x => x.Status == status)
                    ?? throw new NotFoundException($"Tarefa com o status: {status} não encontrada.");

                return Ok(tarefa);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            try
            {
                if (tarefa.Data == DateTime.MinValue)
                    throw new BadRequestException("A data da tarefa não pode ser vazia");
                
                _context.Add(tarefa);
                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            try
            {
                _ = _context.Set<Tarefa>().AsNoTracking().FirstOrDefault(x => x.Id.Equals(id))
                    ?? throw new NotFoundException($"Tarefa com status: {id} não encontrada.");

                if (tarefa.Data == DateTime.MinValue)
                    throw new BadRequestException("A data da tarefa não pode ser vazia");

                tarefa.Id = id;

                _context.Entry(tarefa).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok();
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            try
            {
                var tarefaBanco = _context.Set<Tarefa>().Find(id)
                    ?? throw new ArgumentNullException($"Tarefa com status: {id} não encontrada.");
                
                _context.Remove(tarefaBanco);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}