using System.Linq.Expressions;

namespace Application.UnitTests
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

        public static bool IsExpressionForProperty<T, TProperty>(this Expression<Func<T, bool>> expr, Expression<Func<T, TProperty>> propertyExpression)
        {
            var body = expr.Body as BinaryExpression;
            var left = body?.Left as MemberExpression;
            var property = (propertyExpression.Body as MemberExpression)?.Member.Name;
            return left?.Member.Name == property;
        }
    }
}
