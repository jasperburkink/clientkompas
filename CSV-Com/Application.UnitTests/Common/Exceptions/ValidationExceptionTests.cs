﻿using Application.Common.Exceptions;

namespace Application.UnitTests.Common.Exceptions
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void DefaultConstructorCreatesAnEmptyErrorDictionary()
        {
            var actual = new ValidationException().Errors;

            actual.Keys.Should().BeEmpty();
        }

        [Fact]
        public void SingleValidationFailureCreatesASingleElementErrorDictionary()
        {
            var failures = new List<ValidationFailure>
            {
                new("Age", "must be over 18"),
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().ContainSingle().Which.Should().Be("Age");
            actual["Age"].Should().ContainSingle().Which.Should().Be("must be over 18");
        }

        [Fact]
        public void MulitpleValidationFailureForMultiplePropertiesCreatesAMultipleElementErrorDictionaryEachWithMultipleValues()
        {
            var failures = new List<ValidationFailure>
            {
                new ("Age", "must be 18 or older"),
                new ("Age", "must be 25 or younger"),
                new ("Password", "must contain at least 8 characters"),
                new ("Password", "must contain a digit"),
                new ("Password", "must contain upper case letter"),
                new ("Password", "must contain lower case letter"),
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo("Password", "Age");

            actual["Age"].Should().BeEquivalentTo(
                "must be 25 or younger",
                "must be 18 or older"
            );

            actual["Password"].Should().BeEquivalentTo(
                "must contain lower case letter",
                "must contain upper case letter",
                "must contain at least 8 characters",
                "must contain a digit"
            );
        }
    }
}
