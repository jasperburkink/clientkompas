using Application.Clients.Commands.CreateClient;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.DriversLicence.Commands
{
    public class CreateDriversLicenceCommandtest : IClassFixture<BaseTestFixture>
    {
        private readonly BaseTestFixture testFixture;

        public CreateDriversLicenceCommandtest(BaseTestFixture testFixture)
        {
            this.testFixture = testFixture;
        }

        /*
         * TODO: For now the IntergrationTests are not yet working.
         */
        //[Fact]
        //public async Task ShouldReturnClients()
        //{
        //    await testFixture.RunAsDefaultUserAsync();

        //    var query = new GetClientsQuery();

        //    var result = await testFixture.SendAsync(query);

        //    result.Should().NotBeEmpty();
        //}
    
    }
}
