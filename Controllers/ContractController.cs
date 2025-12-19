using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using signalR.Context;

namespace signalR.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ContractController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("Contract")]
        public async Task<IActionResult> ValidateContract(string contractName)
        {
            var entity = _context.Contracts.FirstOrDefault(c => c.ContractName == contractName);
            if (entity == null)
                return NotFound();

            return Ok();
        }
    }
}
