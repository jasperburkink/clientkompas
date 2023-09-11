using Domain.Common;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTests
{
    public class DomainLayerTests
    {
        private static Assembly DomainAssembly => typeof(ValueObject).Assembly;

        [Fact]
        public void ValueObjects_Should_Be_Immutable()
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
