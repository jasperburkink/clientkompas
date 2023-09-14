using Application.Clients.Queries;
using Application.Clients.Queries.GetClients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.Clients.Queries
{
    public class GetClientsTests : IClassFixture<BaseTestFixture>
    {
        private readonly BaseTestFixture testFixture;

        public GetClientsTests(BaseTestFixture testFixture) 
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
