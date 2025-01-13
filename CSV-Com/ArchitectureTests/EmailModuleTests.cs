using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace ArchitectureTests
{
    public class EmailModuleTests(BaseTestFixture testFixture) : IClassFixture<BaseTestFixture>
    {

        private const string ApplicationProjectName = "Application";
        private const string EmailModuleProjectName = "EmailModule";

        private readonly BaseTestFixture _testFixture = testFixture;

        private Assembly ApplicationAssembly => _testFixture.GetAssemblyByName(ApplicationProjectName);

        [Fact]
        public void ApplicationNotDependendOfEmailModule()
        {
            // Act
            var result = Types
                            .InAssembly(ApplicationAssembly)
                            .Should()
                            .NotHaveDependencyOn(EmailModuleProjectName)
                            .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
