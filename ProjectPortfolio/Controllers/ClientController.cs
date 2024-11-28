﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class ClientController(IClientRepository repository) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> Filter()
        {
            var clients = await repository.GetAllClients();

            var result = new
            {
                draw = 1,
                recordsTotal = clients.Count(),
                recordsFiltered = clients.Count(),          
                data = clients
            };

            return Ok(result);

        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var model = new ClientModel();

            //model = id.HasValue ? await repository.GetAsync((Guid)id) : null;

            return PartialView("~/Views/Client/Edit.cshtml", model);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(ClientModel client)
        {

            //var result = client.Id == Guid.Empty ? await repository.InsertAsync(client)
            //    : await repository.UpdateAsync(client);

            //return result.Success ? Ok(result) : BadRequest(result.Error.Details ?? result.Error.Message);

            return Ok();
        }

        [HttpGet("{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await repository.DeleteAsync(id);

            return Ok();
        }

    }
}