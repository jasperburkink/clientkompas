﻿using System.Runtime.Serialization;
using Application.Clients.Dtos;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Client), typeof(ClientDto))]
        [InlineData(typeof(WorkingContract), typeof(ClientWorkingContractDto))]
        [InlineData(typeof(EmergencyPerson), typeof(EmergencyPersonDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(type)!;
            }

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}
