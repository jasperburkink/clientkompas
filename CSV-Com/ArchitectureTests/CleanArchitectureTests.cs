using Domain.Common;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureTests
{
    public class CleanArchitectureTests
    {
        private const string DomainProjectName = "Domain";
        private const string ApplicationProjectName = "Application";
        private const string InfrastructureProjectName = "Infrastructure";

        private static Assembly DomainAssembly => Utilities.GetAssemblyByName(DomainProjectName);

        private static Assembly ApplicationAssembly => Utilities.GetAssemblyByName(ApplicationProjectName);

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