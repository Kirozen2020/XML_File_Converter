using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

/* https://www.youtube.com/watch?v=CfXZPQZpkVY */ /*read xml file*/
/* https://www.youtube.com/watch?v=cqdJvIJlILg */ /*read csv file*/

namespace Prog_V1
{
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Example for full path: C:\\Users\\user\\Downloads\\Text.txt");
            Console.Write("Enter full csv path: ");
            string csvFullPath = Console.ReadLine(); // "C:\\Users\\rozen\\OneDrive\\Рабочий стол\\Work\\VAR-SOM-MX8M-PLUS.csv";
            csvFullPath = csvFullPath.Replace("\\", "/");

            Console.Write("Enter full output path: ");
            string outputName = Console.ReadLine(); // "C:\\Users\\rozen\\Downloads\\out.xml";
            outputName = outputName.Replace("\\", "/");

            Console.Write("Enter full xml path: ");
            string xmlFullPath = Console.ReadLine(); // "C:\\Users\\rozen\\Downloads\\iMX8M-PLUS.xml";
            xmlFullPath = xmlFullPath.Replace("\\", "/");

            List<Pin> allPins = new List<Pin>();

            List<Info> names = new List<Info>();
            InitNames(names, csvFullPath);

            //init for all the pins 
            allPins = InitPins(names, xmlFullPath);

            UpdateInfo(allPins, names);

            AddByName(allPins, names);

            MergeAltOfPins(allPins, names);

            AddByCoords(allPins, names);

            SortPinsById(allPins);

            //creating the output xml file
            CreateOutput(allPins, outputName);

            Console.WriteLine("\n\nFile created successfully!");
            Console.WriteLine("Enter any key for closing this window");
            Console.ReadKey();
        }
        /// <summary>
        /// Adds the by coords.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        /// <param name="names">The names.</param>
        public static void AddByCoords(List<Pin> allPins, List<Info> names)
        {
            Pin p;
            for (int i = 0; i < names.Count; i++)
            {
                if(!FindCooeds(allPins, names[i].Id))
                {
                    Info info = names[i]; 
                    p = new Pin(info.Name, info.Name, info.Id, "none");
                    allPins.Add(p);
                }
            }
        }
        /// <summary>
        /// Finds the cooeds.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public static bool FindCooeds(List<Pin> allPins, string id)
        {
            for (int i = 0; i < allPins.Count; i++)
            {
                if (allPins[i].Coords.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Initializes the names.
        /// </summary>
        /// <param name="infos">The infos.</param>
        /// <param name="fullPath">The full path.</param>
        public static void InitNames(List<Info> infos, string fullPath)
        {
            using (var reader = new StreamReader(fullPath))
            {
                while (reader.EndOfStream == false)
                {
                    var content = reader.ReadLine().Split(',').ToList();
                    if (content[0].All(char.IsDigit))
                    {
                        Info f = new Info(content[0], content[2], content[3], content[1]);
                        infos.Add(f);
                    }
                }

            }
        }
        /// <summary>
        /// Adds the name of the by.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        /// <param name="infos">The infos.</param>
        public static void AddByName(List<Pin> allPins, IEnumerable<Info> infos)
        {
            Pin p;
            foreach (var info in infos)
            {
                if (!IsInList(allPins, info))
                {
                    p = new Pin(info.Name, info.Name, info.Id, "none");
                    allPins.Add(p);
                }
            }
        }
        /// <summary>
        /// Determines whether [is in list] [the specified pins].
        /// </summary>
        /// <param name="pins">The pins.</param>
        /// <param name="infos">The infos.</param>
        /// <returns>
        ///   <c>true</c> if [is in list] [the specified pins]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInList(List<Pin> pins, Info infos)
        {
            foreach (var pin in pins)
            {
                if (pin.Name.Equals(infos.Name))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Merges the alt of pins.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        /// <param name="names">The names.</param>
        public static void MergeAltOfPins(List<Pin> allPins, IEnumerable<Info> names)
        {
            for (int i = 0; i < allPins.Count; i++)
            {
                if (CountCoords(allPins[i].Coords, allPins) > 1)
                {
                    List<Pin> pins = GetSameCoordsPins(allPins, allPins[i].Coords);
                    
                    for (int k = 1; k < pins.Count; k++)
                    {

                        if (pins[k].CountAlts() > 0)
                        {
                            pins[0].AddAlts(pins[k].Arr);
                        }
                        else
                        {
                            string[] word = pins[k].Name.Split('_');
                            string signal = string.Empty;
                            for (int w = 1; w < word.Length-1; w++)
                            {
                                signal = signal + word[w] + "_";
                            }
                            signal = signal + word[word.Length-1];

                            Info inf = FindInfo(pins[k].Name,names);

                            ALT x = new ALT(pins[k].Name, "NoALT", signal, word[0])
                            {
                                Assy = inf.Assy,
                                Notes = inf.Note
                            };
                            pins[0].AddALT(x);
                        }
                    }
                    for (int k = 1; k < pins.Count; k++)
                    {
                        allPins.Remove(pins[k]);
                    }
                }
            }
        }
        /// <summary>
        /// Finds the information.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public static Info FindInfo(string x, IEnumerable<Info> names)
        {
            foreach (var item in names)
            {
                if(item.Name.Equals(x))
                {
                    return item;
                }
            }
            return null;//error
        }
        /// <summary>
        /// Gets the same coords pins.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        /// <param name="coords">The coords.</param>
        /// <returns></returns>
        public static List<Pin> GetSameCoordsPins(List<Pin> allPins, string coords)
        {
            List<Pin> result = new List<Pin>();
            for (int i = 0; i < allPins.Count; i++)
            {
                if (allPins[i].Coords.Equals(coords))
                {
                    result.Add(allPins[i]);
                }
            }
            return result;
        }
        /// <summary>
        /// Counts the coords.
        /// </summary>
        /// <param name="coords">The coords.</param>
        /// <param name="allPins">All pins.</param>
        /// <returns></returns>
        public static int CountCoords(string coords, List<Pin> allPins)
        {
            int count = 0;
            for (int i = 0; i < allPins.Count; i++)
            {
                if (allPins[i].Coords == coords)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// Sorts the pins by identifier.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        public static void SortPinsById(List<Pin> allPins)
        {
            for (int i = 0; i < allPins.Count - 1; i++)
            {
                for (int j = 1; j < allPins.Count - i - 1; j++)
                {
                    if (Convert.ToInt64(allPins[j + 1].Coords) > Convert.ToInt64(allPins[j].Coords))
                    {
                        Pin temp = allPins[j];
                        allPins[j] = allPins[j + 1];
                        allPins[j + 1] = temp;
                    }
                }
            }
            allPins.Reverse();
        }
        /// <summary>
        /// Updates the information.
        /// </summary>
        /// <param name="allPins">All pins.</param>
        /// <param name="names">The names.</param>
        public static void UpdateInfo(List<Pin> allPins, IEnumerable<Info> names)
        {
            foreach (var pin in allPins)
            {
                foreach (var item in names)
                {
                    if (item.Name.Equals(pin.Name))
                    {
                        pin.Coords = item.Id.ToString();
                        UpdateAlt(pin.Arr, item);
                    }
                }
            }
        }
        /// <summary>
        /// Updates the alt.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="item">The item.</param>
        public static void UpdateAlt(Node<ALT> arr, Info item)
        {
            Node<ALT> p = arr;
            while (p != null)
            {
                if (item.Assy != null)
                {
                    item.Assy = item.Assy.Replace("\r\n", " ");
                    item.Assy = item.Assy.Replace("no ", "!");
                    p.GetValue().Assy = item.Assy;
                }
                if (item.Note != null)
                {
                    item.Note = item.Note.Replace("\r\n", " ");
                    p.GetValue().Notes = item.Note;
                }

                p = p.GetNext();
            }
        }
        /// <summary>
        /// Creates the output.
        /// </summary>
        /// <param name="usedPins">The used pins.</param>
        /// <param name="outputPath">The output path.</param>
        public static void CreateOutput(List<Pin> usedPins, string outputPath)
        {
            XElement xdoc = new XElement("som");
            for (int i = 0; i < usedPins.Count; i++)
            {
                XElement pin = new XElement("pin",
                    new XAttribute("name", usedPins[i].Name),
                    new XAttribute("description", usedPins[i].Description),
                    new XAttribute("coords", usedPins[i].Coords),
                    new XAttribute("power_group", usedPins[i].Power_group));

                Node<ALT> arr = usedPins[i].Arr;

                while (arr != null)
                {
                    XElement alt;

                    alt = new XElement("connection",
                            new XAttribute("name_part", arr.GetValue().Name_part),
                            new XAttribute("package_function", arr.GetValue().Package_function),
                            new XAttribute("assy", arr.GetValue().Assy),
                            new XAttribute("notes", arr.GetValue().Notes),

                            new XElement("peripheral_signal_ref",
                                new XAttribute("signal", arr.GetValue().Signal),
                                new XAttribute("peripheral", arr.GetValue().Peripheral)),

                            new XElement("peripheral_dts_ref",
                                new XAttribute("group_name_postfix", arr.GetValue().Group_name_postfix1),
                                new XAttribute("padname", arr.GetValue().Padname1),
                                new XAttribute("value", arr.GetValue().Value1)),

                            new XElement("peripheral_dts_ref",
                                new XAttribute("group_name_postfix", "-sleep"/*arr.GetValue().Group_name_postfix2*/),
                                new XAttribute("padname", arr.GetValue().Padname2),
                                new XAttribute("value", arr.GetValue().Value2))
                            );

                    pin.Add(alt);

                    arr = arr.GetNext();
                }

                xdoc.Add(pin);
            }
            xdoc.Save(outputPath);

        }
        /// <summary>
        /// Initializes the pins.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="xmlFullPath">The XML full path.</param>
        /// <returns></returns>
        public static List<Pin> InitPins(IEnumerable<Info> names, string xmlFullPath)//Get all pins from the big xml file
        {
            //list
            List<Pin> allpins = new List<Pin>();

            //document 
            XDocument xdoc = XDocument.Load(xmlFullPath);

            XElement categories = xdoc.Descendants().Where(x => x.Name.LocalName == "pins").FirstOrDefault();


            var pins = from c in categories.Elements("pin")
                       select c;

            int i = 0;
            //get pins
            foreach (var pin in pins)
            {
                string name = string.Empty, description = string.Empty, coords = string.Empty, power_group = string.Empty;
                string name_part = string.Empty, package_function = string.Empty, signal = string.Empty, peripheral = string.Empty;

                name = pin.Attribute("name").Value.ToString();

                if (IsInChip(name, names))
                {
                    description = pin.Attribute("description").Value.ToString();
                    power_group = pin.Attribute("power_group").Value.ToString();


                    Pin p = new Pin(name, description, coords, power_group);


                    //get alts (connection)
                    var connections = from a in pin.Elements("connections")
                                      select a;

                    foreach (var connection in connections)
                    {
                        name_part = connection.Attribute("name_part").Value.ToString();
                        package_function = connection.Attribute("package_function").Value.ToString();
                        //string assy = connection.Attribute("assy").ToString();
                        //string notes = connection.Attribute("notes").ToString();

                        var peripheral_signal_refs = from p1 in connection.Elements("connection").Elements("peripheral_signal_ref")
                                                     select p1;

                        foreach (var peripheral_signal_ref in peripheral_signal_refs)
                        {
                            signal = peripheral_signal_ref.Attribute("signal").Value.ToString();
                            peripheral = peripheral_signal_ref.Attribute("peripheral").Value.ToString();
                        }
                        ALT a1 = new ALT(name_part, package_function, signal, peripheral);
                        p.AddALT(a1);
                    }
                    allpins.Add(p);

                    i++;
                }
            }
            return allpins;
        }
        /// <summary>
        /// Determines whether [is in chip] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public static int IsInChip(Pin x, List<Info> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].Name.Equals(x.Name))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Determines whether [is in chip] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="names">The names.</param>
        /// <returns>
        ///   <c>true</c> if [is in chip] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInChip(string name, List<Info> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (names[i].Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Determines whether [is in chip] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="names">The names.</param>
        /// <returns>
        ///   <c>true</c> if [is in chip] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInChip(string name, IEnumerable<Info> names)
        {
            foreach (var n in names)
            {
                if (n.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
