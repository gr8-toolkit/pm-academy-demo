using System;
using CovarianceAndContravarianceExample.Examples;
using CovarianceAndContravarianceExample.Models;

// <T> -> variance
// <out T> covariance
// <in T> contravariance
namespace CovarianceAndContravarianceExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Covariance & Contravariance examples");
            CovarianceExample.Foo();
            ContrvarianceExample.Foo();
            ButtonExample.Foo();
            ButtonGExample.Foo();
            SomeFactoryExample.Foo();
            Console.ReadLine();
        }
    }
}