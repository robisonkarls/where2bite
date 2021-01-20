using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.Endpoints
{
    [Route("api/v1/[controller]")]
    public class EstablishmentsController : ControllerBase
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILogger<EstablishmentsController> _logger;

        public EstablishmentsController([NotNull] IEstablishmentRepository establishmentRepository, [NotNull] ILogger<EstablishmentsController> logger)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        // GET api/v1/[controller]/establishments
        [HttpPost]
        [Route("establishments")]
        [ProducesResponseType(typeof(IEnumerable<Establishment>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEstablishmentsInRadiusAsync(
            [FromBody] EstablishmentInRadiusRequest establishmentInRadiusRequest, 
            CancellationToken cancellationToken = default)
        {
            if (establishmentInRadiusRequest == null)
            {
                throw new ArgumentNullException(nameof(establishmentInRadiusRequest));
            }

            if (establishmentInRadiusRequest.Radius <= 0)
            {
                return BadRequest("Radius must be greater than 0");
            }
            _logger.LogInformation(
                $"Searching Establishments in a radius of {establishmentInRadiusRequest.Radius}",
                new Point(establishmentInRadiusRequest.Longitude, establishmentInRadiusRequest.Latitude));

            var establishments = await _establishmentRepository.GetAllWithinRadiusAsync(
                establishmentInRadiusRequest.Radius,
                new Point(establishmentInRadiusRequest.Longitude, establishmentInRadiusRequest.Latitude),
                cancellationToken);

            return Ok(establishments.Any() ? establishments : Enumerable.Empty<Establishment>());
        }
    }
}