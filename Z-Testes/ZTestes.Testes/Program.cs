// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var acaoAnimal = new AcaoAnimal();
var animal = new Animal();
var cachorro = new Cachorro();
var gato = new Gato();
var papagio = new Papagaio();

acaoAnimal.Falar(animal);
acaoAnimal.Falar(cachorro);
acaoAnimal.Falar(gato);
acaoAnimal.Falar(papagio);

public class AcaoAnimal
{
    public void Falar(Animal animal)
    {
        animal.Falar();
    }
}

public class Animal
{
    public virtual void Falar()
    {
        Console.WriteLine("Falando genéricamente.");
    }
}

public class Cachorro : Animal
{
    public override void Falar()
    {
        Console.WriteLine("AU AU");
    }
}

public class Gato : Animal
{
    public override void Falar()
    {
        Console.WriteLine("Miauuuuu");
    }
}

public class Papagaio : Animal
{
    public override void Falar()
    {
        Console.WriteLine("BORA BIL!!!!!");
    }
}

public class Golfinho : Animal
{
    
}