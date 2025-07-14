using System;

namespace BackendProjects.DataExporter
{
    class Program
    {
        static void Main()
        {
            var people = new List<Person>
            {
                new Person { Name = "Alice", Age = 30, Email = "alice@example.com" },
                new Person { Name = "Bob", Age = 25, Email = "bob@example.com" }
            };

            var cars = new Car[]
            {
                new Car { Make = "Toyota", Model = "Camry", Year = 2022, Price = 24999.99m },
                new Car { Make = "Honda", Model = "Accord", Year = 2021, Price = 22999.99m },
                new Car { Make = "Ford", Model = "Mustang", Year = 2023, Price = 34999.99m },
                new Car { Make = "Tesla", Model = "Model 3", Year = 2023, Price = 39999.99m }
            };

            //IDataExporter dataExporter = DataExporterFactory.CreateDataExporter("json");
            /*
            IDataExporter dataExporter = DataExporterFactory.CreateDataExporter("csv");
            Console.WriteLine(dataExporter.Export<Person>(people));
            Console.WriteLine(dataExporter.Export<Car>(cars));
            */
            DataExporterHelper.ExportToFile<Person>(people, "people.json", "people.csv");
            DataExporterHelper.ExportToFile<Car>(cars, "cars.json", "cars.csv", "cars.xml");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }   
}