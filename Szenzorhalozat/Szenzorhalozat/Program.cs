using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using SzenzorhalozatLibrary;

namespace Szenzorhalozat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Szenzorhálózat szimuláció indítása...");

            // Szenzorhálózat inicializálása
            SensorNetwork network = new SensorNetwork();
            network.DataReceived += OnDataReceived; // Eseményhez metódus csatlakoztatása

            // Szenzorok hozzáadása
            network.AddSensor(new Sensor("Hőmérséklet"));
            network.AddSensor(new Sensor("Páratartalom"));
            network.AddSensor(new Sensor("Víztartály szint"));

            // Adat generálása és feldolgozása
            network.GenerateData();

            // Adatok mentése XML és JSON fájlba
            network.SaveToXml("adatok.xml"); // XML mentés
            network.SaveToJson("adatok.json"); // JSON mentés

            // LINQ lekérdezések eredményeinek megjelenítése
            Console.WriteLine("\nLINQ lekérdezések eredményei:");
            var highValues = network.GetHighValues(50); // Küszöbérték feletti adatok
            Console.WriteLine("50 feletti értékek: " + string.Join(", ", highValues));

            var average = network.GetAverageValue(); // Átlagérték
            Console.WriteLine("Átlagos érték: " + average);

            var sensorsWithData = network.GetSensorsWithData(); // Adattal rendelkező szenzorok
            Console.WriteLine("Adattal rendelkező szenzorok: " + string.Join(", ", sensorsWithData));

            Console.WriteLine("Program vége.");
        }

        // Eseménykezelő metódus az adatok fogadására
        static void OnDataReceived(object sender, SensorDataEventArgs e)
        {
            Console.WriteLine($"Adat érkezett: Szenzor={e.SensorName}, Érték={e.Value}");
        }
    }
}
