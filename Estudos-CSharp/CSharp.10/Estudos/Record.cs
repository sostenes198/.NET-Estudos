using System.Text.Json.Serialization;

namespace CSharp._10.Estudos;

/*
 * A partir do C# 9, você usa a palavra-chave record para definir um tipo de referência que fornece funcionalidade interna para encapsular dados.
 * O C# 10 permite que a sintaxe record class como um sinônimo esclareça um tipo de referência e record struct defina um tipo de valor com funcionalidade semelhante.
 * Você pode criar tipos de registro com propriedades imutáveis usando parâmetros posicionais ou sintaxe de propriedade padrão.
 * Os dois exemplos a seguir demonstram os tipos de referência record (ou record class):
 */
/*
 Igualdade de valor
Se você não substituir nem sobrecarregar métodos de maneira igual, o tipo que você declarar definirá como a igualdade é definida:

Para tipos class, dois objetos serão iguais quando se referirem ao mesmo objeto na memória.
Para tipos struct, dois objetos são iguais se forem do mesmo tipo e armazenarem os mesmos valores.
Para tipos record, incluindo record struct e readonly record struct, dois objetos são iguais se forem do mesmo tipo e armazenarem os mesmos valores.
A definição de igualdade para um record struct é a mesma de um struct. A diferença é que, para um struct, a implementação está em ValueType.Equals(Object) 
e depende da reflexão. Para registros, a implementação é sintetizada pelo compilador e usa os membros de dados declarados.

A igualdade de referência é necessária para alguns modelos de dados. Por exemplo, o Entity Framework Core depende da igualdade de referência para garantir
que ele use apenas uma instância de um tipo de entidade para o que é conceitualmente uma entidade. Por esse motivo, os registros e os structs de registro
 não são apropriados para uso como tipos de entidade no Entity Framework Core.
 */

// parâmetros posicionais
public record Person(string FirstName, string LastName)
{
    public string[] PhoneNumbers { get; init; } = Array.Empty<string>();
}

// parâmetros  sintaxe de propriedade
public record Person2
{
    public /*required*/ string FirstName { get; init; }

    public /*required*/ string LastName { get; init; }
};

// record struct:
public readonly record struct Point(double X, double Y);

public record struct Point2
{
    public double X { get; init; }

    public double Y { get; init; }

    public double Z { get; init; }
}

// Você também pode criar registros com propriedades e campos mutáveis:
public record Person3
{
    public /*required*/ string FirstName { get; set; }

    public /*required*/ string LastName { get; set; }
};

public record Person4([property: JsonPropertyName("firstName")] string FirstName,
    [property: JsonPropertyName("lastName")] string LastName);