﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using WhereToBite.Api.Infrastructure.Mappers;
using WhereToBite.Api.Model;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;

namespace WhereToBite.Api.Endpoints
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class EstablishmentsController : ControllerBase
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly ILogger<EstablishmentsController> _logger;
        private readonly IDomainMapper _domainMapper;

        public EstablishmentsController([NotNull] IEstablishmentRepository establishmentRepository, 
            [NotNull] ILogger<EstablishmentsController> logger,
            [NotNull] IDomainMapper domainMapper)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domainMapper = domainMapper ?? throw new ArgumentNullException(nameof(domainMapper));
        }
        
        // POST api/v1/[controller]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<EstablishmentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetEstablishmentsInRadiusAsync(
            [FromBody] NearbyRequest nearbyRequest, 
            CancellationToken cancellationToken = default)
        {
            if (nearbyRequest == null)
            {
                throw new ArgumentNullException(nameof(nearbyRequest));
            }

            if (nearbyRequest.Radius <= 0)
            {
                return BadRequest("Radius must be greater than 0");
            }

            var center = new Point(nearbyRequest.Longitude, nearbyRequest.Latitude);
            var radius = nearbyRequest.Radius;
            
            _logger.LogInformation($"Searching Establishments in a radius of {radius} FROM {center}");

            var establishments = await _establishmentRepository.GetAllWithinRadiusAsync(radius, center, cancellationToken);

            var mappedEstablishments = _domainMapper.MapEstablishmentResponses(establishments);

            return Ok(mappedEstablishments.Any() ? mappedEstablishments : Enumerable.Empty<EstablishmentResponse>());
        }
        
        // POST api/v1/[controller]/{establishmentId}/inspections
        [HttpGet]
        [Route("{establishmentId:int}/inspections")]
        [ProducesResponseType(typeof(IEnumerable<InspectionResponse>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetInspectionsAsync(int establishmentId, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Getting Inspections for EstablishmentId ${establishmentId}");

            var inspections = await _establishmentRepository.GetInspectionsAsync(establishmentId, cancellationToken);

            return Ok(inspections.Any() ? _domainMapper.MapInspectionResponses(inspections) : Enumerable.Empty<InspectionResponse>());
        }
    }
}