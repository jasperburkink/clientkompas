using System.Reflection;
using Domain.Common;
using NetArchTest.Rules;

namespace ArchitectureTests
{
    public class DomainLayerTests
    {
        private static Assembly DomainAssembly => typeof(ValueObject).Assembly;

        [Fact]
        public void ValueObjects_ShouldBe_Immutable()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .Should()
                .BeImmutable()
                .GetResult();

            Assert.True(result.IsSuccessful, GetFailingTypes(result));
        }

        private string GetFailingTypes(TestResult result)
        {
            return result.FailingTypeNames != null ?
                string.Join(", ", result.FailingTypeNames) :
                string.Empty;
        }
    }
}
