using System;
using Quiz.Controllers;


namespace Quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            var item = new TcpController(1028);
            Console.WriteLine("Hello World!");
        }
    }
}
