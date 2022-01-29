using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication_Identity_2.Controllers.Api
{
    [ApiController]

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class BicyclesController : Controller
    {
        private readonly BicycleContext context;

        public BicyclesController(BicycleContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult<IEnumerable<Bicycle>>> Get()
        {
            return await context.Bicycles.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Bicycle>> Get(int id)
        {
            Bicycle bike = await context.Bicycles.FirstOrDefaultAsync(x => x.BicycleId == id);
            if (bike == null)
            {
                return NotFound();
            }
            return bike;
        }
        [HttpPost]
        public async Task<ActionResult<Bicycle>> Post( Bicycle bike)
        {
            
            context.Bicycles.Add(bike);
            await context.SaveChangesAsync();
            return Ok(bike);
        }
        [HttpPut]
        public async Task<ActionResult<Bicycle>> Put(Bicycle bike)
        {
            if (bike == null)
            {
                return BadRequest();
            }
            context.Update(bike);
            await context.SaveChangesAsync();
            return Ok(bike);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bicycle>> Delete(int id)
        {
            Bicycle bike = await context.Bicycles.FirstOrDefaultAsync(x => x.BicycleId == id);
            if (bike == null)
            {
                return NotFound();
            }
            context.Bicycles.Remove(bike);
            await context.SaveChangesAsync();
            return Ok(bike);
        }
    }
}
