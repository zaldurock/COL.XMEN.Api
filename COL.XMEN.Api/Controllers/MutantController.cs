using COL.XMEN.Application;
using COL.XMEN.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace COL.XMEN.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    public class MutantController : ControllerBase
    {
        private readonly ILogger<MutantController> _logger;
        private readonly IMutantService _mutantService;

        public MutantController(ILogger<MutantController> logger, IMutantService mutantService)
        {
            _logger = logger;
            _mutantService = mutantService;
        }

        /// <summary>
        /// Endpoint to check DNA is mutant
        /// </summary>
        /// <param name="mutantDNA"></param>
        /// <returns>if create or no</returns>
        [HttpPost]
        public async Task<IActionResult> IsMutant(Mutant mutantDNA)
        {
            var mutant = await _mutantService.ValidateIsMutant(mutantDNA);

            if (mutant.isMutant)
            {
                return Created(string.Empty, mutant);
            }
            else
            {
                return StatusCode(403, string.Empty);
            }
        }

        /// <summary>
        /// Endpont report
        /// </summary>
        /// <returns>the specific report</returns>
        [HttpGet]
        [Route("stats")]
        public async Task<MutantStats> Stats()
        {
            var mutant = await _mutantService.StatsMutant();

            return mutant;
        }
    }
}
