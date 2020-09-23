using System;
using System.Linq.Expressions;

namespace Estudos.Exame.Capitulo2.GenerateCodeAtRunTime.ExpressionTree
{
    public class ExpressionTreeStudy
    {
        public static void TestExpressionTree()
        {
            // Build Expression Tree 
            // Expression<Func<int , int>> square = num => num * num;

            // Parâmetro da expression num
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            
            // Operação a ser executada com o parametro
            BinaryExpression squareOperation = Expression.Multiply(numParam, numParam);

            Expression<Func<int, int>> square = Expression.Lambda<Func<int, int>>(squareOperation, numParam);
            
            // Compile three para fazer um método executável, assinado por um delegate
            var doSquare = square.Compile();
            Console.WriteLine($"Square of: {doSquare(2)}");
        }

        public static void TestExpressionVisitor()
        {
            Expression<Func<int , int>> square = num => num * num;
            var multiplyToAddExpressionVisitor = new MultiplyToAddExpressionVisitor();
            Expression<Func<int, int>> addExpression = (Expression<Func<int, int>>) multiplyToAddExpressionVisitor.Modify(square);
            var doAdd = addExpression.Compile();
            Console.WriteLine($"Double of 4: {doAdd(2)}");
        }
    }
}