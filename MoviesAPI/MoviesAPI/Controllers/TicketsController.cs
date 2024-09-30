using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoviesAPI.Models;
using MoviesAPI.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly ITicketsRepository _TicketsRepository;
        public TicketsController(ILogger<TicketsController> logger, ITicketsRepository TicketsRepository)
        {
            _logger = logger;
            _TicketsRepository = TicketsRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var result = await _TicketsRepository.GetTicketsAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTickets(int id)
        {
            var item = await _TicketsRepository.GetTicketsAsync(id);
            if(item == null) 
                return NotFound();
            else
                return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult> PostTicket(CreateTicket newTicket)
        {
            if(ModelState.IsValid)
            {
                

                var newId = await _TicketsRepository.CreateTicketAsync(newTicket);
                return Ok(newId);
            }
            else return BadRequest();
        }
        [HttpPut]
        public async Task<ActionResult> PutTicket([FromForm] int key, [FromForm] string values)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _TicketsRepository.GetTicketsForUpdateAsync(key);
                if (existingItem == null)
                {
                    return NotFound();
                }
                _logger.LogInformation($"Azuriranje na tiket:[{existingItem.User_id}]");
                    var numRecords = await _TicketsRepository.UpdateTicketAsync(key, existingItem);

                    return Ok(numRecords);
                
            }
            else return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteTicket([FromForm] int key)
        {
            var existingItem = await _TicketsRepository.GetTicketsAsync(key);

            if (existingItem is null)
            {
                return NotFound();
            }

            try
            {
                var numRecords = await _TicketsRepository.DeleteTicketAsync(key);

                return Ok(numRecords);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
    
   