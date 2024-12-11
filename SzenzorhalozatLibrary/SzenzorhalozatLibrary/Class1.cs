using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Xml;

namespace SzenzorhalozatLibrary
{
    public class Sensor
    {
        public string Name { get; set; } // Szenzor neve

        private static Random rng = new Random(); // Véletlenszám-generátor

        public Sensor(string name)
        {
            Name = name;
        }

        // Véletlen mérési adat generálása
        public int GenerateData()
        {
            return rng.Next(0, 101);
        }
    }

    public class SensorData
    {
        public string SensorName { get; set; } // Szenzor neve
        public int Value { get; set; } // Mért érték
    }

    // Egyedi esemény adatokat tartalmazó osztály
    public class SensorDataEventArgs : EventArgs
    {
        public string SensorName { get; }
        public int Value { get; }

        public SensorDataEventArgs(string sensorName, int value)
        {
            SensorName = sensorName;
            Value = value;
        }
    }

    public class SensorNetwork
    {
        private List<Sensor> sensors = new List<Sensor>(); // Szenzorok listája
        private List<SensorData> dataLog = new List<SensorData>(); // Mért adatok naplózása

        // Esemény a mért adatok fogadására
        public event EventHandler<SensorDataEventArgs> DataReceived;

        // Új szenzor hozzáadása a hálózathoz
        public void AddSensor(Sensor sensor)
        {
            sensors.Add(sensor);
        }

        // Mért adatok generálása
        public void GenerateData()
        {
            foreach (var sensor in sensors)
            {
                int value = sensor.GenerateData();
                dataLog.Add(new SensorData { SensorName = sensor.Name, Value = value });
                DataReceived?.Invoke(this, new SensorDataEventArgs(sensor.Name, value)); // Esemény kiváltása
            }
        }

        // Adatok mentése XML fájlba
        public void SaveToXml(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<SensorData>));
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, dataLog);
            }
        }

        // Adatok mentése JSON fájlba
        public void SaveToJson(string fileName)
        {
            string json = JsonConvert.SerializeObject(dataLog, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        // LINQ: Küszöbérték feletti adatok lekérdezése
        public IEnumerable<int> GetHighValues(int threshold)
        {
            return dataLog.Where(d => d.Value > threshold).Select(d => d.Value);
        }

        // LINQ: Átlagos érték kiszámítása
        public double GetAverageValue()
        {
            return dataLog.Average(d => d.Value);
        }

        // LINQ: Adattal rendelkező szenzorok nevei
        public IEnumerable<string> GetSensorsWithData()
        {
            return dataLog.Select(d => d.SensorName).Distinct();
        }
    }
}

