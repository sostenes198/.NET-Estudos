using System;
using System.Text.RegularExpressions;

namespace Estudos.Exame.Capitulo2.EvaluateRegularExpressionToValidateInput
{
    public class MatchMultipleSpacesRegexExpression
    {
        public static void Test()
        {
            var input = "Rob        mary        David    Jenry    Chris  Imogen Rodney";
            var regularExpressionToMatch = " +";
            var patternToReplace = ";";
            var replaced = Regex.Replace(input, regularExpressionToMatch, patternToReplace);
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Replaced: {replaced}");
        }
    }
}