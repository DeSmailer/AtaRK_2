using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATARK.Models;
using ATARK.Models.Entity;
using ATARK.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATARK.Controllers.BL
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessLogicController : ControllerBase
    {
        private readonly IRepository repository;

        public BusinessLogicController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpPut("{CWSIId}")]
        public async Task<IActionResult> RedistributeFish(int CWSIId)
        {
            BusinessLogic businessLogic = new BusinessLogic(repository, CWSIId);
            businessLogic.Execute();

            return this.Ok();
        }
    }
}
