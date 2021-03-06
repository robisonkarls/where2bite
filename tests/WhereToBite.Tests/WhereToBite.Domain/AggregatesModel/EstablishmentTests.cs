﻿using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using WhereToBite.Domain.AggregatesModel.EstablishmentAggregate;
using Xunit;
namespace WhereToBite.Tests.WhereToBite.Domain.AggregatesModel
{
    
    public class EstablishmentTests
    {
        private readonly Establishment _establishment;

        public EstablishmentTests()
        {
            _establishment =  new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                Point.Empty);
        }

        [Fact]
        public void ShouldCreateInstance()
        {
            Assert.NotNull(_establishment);
        }

        [Fact]
        public void ShouldAddInspection()
        {
            var inspections = new[] { new Inspection("Pass", DateTime.Now)};
            
            _establishment.AddNewInspections(inspections);
            
            Assert.Equal(inspections, _establishment.Inspections);
        }

        [Fact]
        public void ShouldOnlyAddNewInspection()
        {
            var inspections = new List<Inspection>
            {
                new("Pass", DateTime.Now.AddHours(-2))
            };
            
            _establishment.AddNewInspections(inspections);
            
            inspections.Add(new Inspection("Pass", DateTime.Now));

            _establishment.AddNewInspections(inspections);
            
            Assert.Equal(2, _establishment.Inspections.Count);
            Assert.Equal(inspections, _establishment.Inspections);
        }
        
        [Fact]
        public void ShouldReturnEmtpyCollectionIfThereIsNoInspection()
        {
            var establishment =  new Establishment(1,
                "test",
                "Restaurant",
                string.Empty,
                "Pass",
                Point.Empty);

            var actual = establishment.GetLastInspection();
            
            Assert.Null(actual);
        }
    }
}