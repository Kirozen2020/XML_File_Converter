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
        static void Main(string[] args)
        {
            Console.WriteLine("Example for full path: C:\\Users\\user\\Downloads\\Text.txt");
            Console.Write("Enter full csv path: ");
            string csvFullPath = "C:\\Users\\rozen\\OneDrive\\Рабочий стол\\Work\\VAR-SOM-MX8M-PLUS.csv";
            csvFullPath = csvFullPath.Replace("\\", "/");

            Console.Write("Enter full output path: ");
            string outputName = "C:\\Users\\rozen\\Downloads\\norem.xml";
            outputName = outputName.Replace("\\", "/");

            Console.Write("Enter full xml path: ");
            string xmlFullPath =  "C:\\Users\\rozen\\Downloads\\iMX8M-PLUS.xml";
            xmlFullPath = xmlFullPath.Replace("\\", "/");

            List<Pin> allPins = new List<Pin>();

            List<Info> names = new List<Info>();
            InitNames(names, csvFullPath);

            //init for all the pins 
            allPins = InitPins(names, xmlFullPath);

            UpdateInfo(allPins, names);

            AddMisingPins(allPins, names);

            AddPins(allPins, names);

            SortPinsById(allPins);

            //creating the output xml file
            CreateOutPut(allPins, outputName);

            Console.WriteLine("\n\nFile created successfully!");
            Console.WriteLine("Enter any key for closing this window");
            Console.ReadKey();
        }

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
        public static void AddMisingPins(List<Pin> allPins, IEnumerable<Info> infos)
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
        private static void AddPins(List<Pin> allPins, IEnumerable<Info> names)
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
                            //Console.WriteLine("YesAlt -> " + pins[k].Coords);
                            pins[0].AddAlts(pins[k].Arr);//working
                        }
                        else
                        {
                            //Console.WriteLine("NoAlt -> " + pins[k].Coords);
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

                    //ALT assy = new ALT(yesAssy.Arr.GetValue().Name_part, "NoALT", yesAssy.Arr.GetValue().Signal, yesAssy.Arr.GetValue().Peripheral);
                    //assy.Assy = yesAssy.Arr.GetValue().Assy;
                    //assy.Notes = yesAssy.Arr.GetValue().Notes;
                    //noAssy.AddALT(assy);
                    //allPins.Remove(yesAssy);
                }
            }
        }

        private static Info FindInfo(string x, IEnumerable<Info> names)
        {
            foreach (var item in names)
            {
                if(item.Name.Equals(x))
                {
                    return item;
                }
            }
            return null;
        }

        private static List<Pin> GetSameCoordsPins(List<Pin> allPins, string coords)
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
        
        private static int CountCoords(string coords, List<Pin> allPins)
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

        private static void SortPinsById(List<Pin> allPins)
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

        private static void UpdateInfo(List<Pin> allPins, IEnumerable<Info> names)
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

        private static void UpdateAlt(Node<ALT> arr, Info item)
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

        private static void CreateOutPut(List<Pin> usedPins, string outputPath)
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
        /*
        public static void InitNames()//get all names of pins that we need in the output
        {
            var csvFileDescription = new CsvFileDescription
            {
                FirstLineHasColumnNames = true,
                IgnoreUnknownColumns = true,
                SeparatorChar = ',',
                UseFieldIndexForReadingData = false,

            };

            var csvContext = new CsvContext();
            var countries = csvContext.Read<Info>("VAR-SOM-MX8M-PLUS.csv", csvFileDescription);

            foreach (var country in countries)
            {
                Console.WriteLine($"{country.Id} -> {country.Name}");
            }
        }
        */
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
