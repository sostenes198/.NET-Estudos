using System.Runtime.CompilerServices;

Person person1 = new("Nancy", "Davolio") { PhoneNumbers = new string[1] };
Console.WriteLine(person1);
// output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }

Person person2 = person1 with { FirstName = "John" };
Console.WriteLine(person2);
// output: Person { FirstName = John, LastName = Davolio, PhoneNumbers = System.String[] }
Console.WriteLine(person1 == person2); // output: False

person2 = person1 with { PhoneNumbers = new string[1] };
Console.WriteLine(person2);
// output: Person { FirstName = Nancy, LastName = Davolio, PhoneNumbers = System.String[] }
Console.WriteLine(person1 == person2); // output: False

person2 = person1 with { };
Console.WriteLine(person1 == person2); // output: True

Console.WriteLine(ManipuladoresDeCadeiaDeCaracteresInterpolada.Str2);

// Atribuição e declaração na mesma desconstrução
var point = new Point(10, 5);

// assignment:
// int x1 = 0;
// int y1 = 0;
// (x1, y1) = point;

// OU 
double x = 0;
(x, double y) = point;


// Diagnóstico de atributo CallerArgumentExpression
static void Validate(bool condition, [CallerArgumentExpression("condition")] string? message=null)
{
    if (!condition)
    {
        throw new InvalidOperationException($"Argument failed validation: <{message}>");
    }
}

Validate(false, "ASHUDHA");