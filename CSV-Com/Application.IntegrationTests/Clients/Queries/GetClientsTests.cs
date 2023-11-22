namespace Application.IntegrationTests.Clients.Queries
{
    public class GetClientsTests : IClassFixture<BaseTestFixture>
    {
        private readonly BaseTestFixture _testFixture;

        public GetClientsTests(BaseTestFixture testFixture)
        {
            _testFixture = testFixture;
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
