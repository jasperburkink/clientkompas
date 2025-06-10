using System.Reflection;
using NetArchTest.Rules;

namespace ArchitectureTests
{
    public class CleanArchitectureTests(BaseTestFixture testFixture) : IClassFixture<BaseTestFixture>
    {
        private const string DomainProjectName = "Domain";
        private const string ApplicationProjectName = "Application";
        private const string InfrastructureProjectName = "Infrastructure";

        private readonly BaseTestFixture _testFixture = testFixture;

        private Assembly DomainAssembly => _testFixture.GetAssemblyByName(DomainProjectName);

        private Assembly ApplicationAssembly => _testFixture.GetAssemblyByName(ApplicationProjectName);

        [Fact]
        public void DomainNotDependendOfAnyOtherProject()
        {
            var result = Types
                .InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOnAny()
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DomainNotDependendOfApplication()
        {
            var result = Types
                .InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ApplicationProjectName)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void DomainNotDependendOfInfrastructure()
        {
            var result = Types
                .InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureProjectName)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ApplicationNotDependendOfInfrastructure()
        {
            var result = Types
                .InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(InfrastructureProjectName)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }
    }
}
