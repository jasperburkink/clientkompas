using System.Linq.Expressions;
using Domain.Authentication.Constants;
using TestData;

namespace Application.FunctionalTests
{
    public static class Utils
    {
        public static void SetPrivate<T, TValue>(this T instance, Expression<Func<T, TValue>> propertyExpression, TValue value)
        {
            instance.GetType().GetProperty(GetName(propertyExpression)).SetValue(instance, value, null);
        }

        private static string GetName<T, TValue>(Expression<Func<T, TValue>> exp)
        {
            var body = exp.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static string GeneratePassword()
        {
            return FakerConfiguration.Faker.Internet.Password(AuthenticationUserConstants.PASSWORD_MINLENGTH) + "Az1!";
        }
    }
}
