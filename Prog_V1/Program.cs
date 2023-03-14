using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Prog_V1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Pin> allPins = new List<Pin>();
            List<Pin> usedPins = new List<Pin>();

            List<Info> pinNames = new List<Info>();

            //init the names of used pins in order
            InitNames(pinNames);

            //init for all the pins 
            allPins = InitPins();

            //CreateOutPut(allPins);
            //geting the right pins
            foreach (Pin pin in allPins)
            {
                int index = IsInChip(pin, pinNames);
                if (index != -1)
                {
                    pin.Coords = pinNames[index].Id;

                    Node<ALT> p = pin.Arr;

                    while (p != null)
                    {
                        p.GetValue().Notes = pinNames[index].Note;
                        p.GetValue().Notes = pinNames[index].Assy;

                        p = p.GetNext();
                    }

                    usedPins.Add(pin);

                }
            }


            //creating the output xml file
            CreateOutPut(usedPins);

        }

        private static void CreateOutPut(List<Pin> usedPins)
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
                    XElement alt = new XElement("connection",
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
            xdoc.Save("C:/Users/rozen/OneDrive/Рабочий стол/Work/xmlTest1.xml");

        }

        public static void InitNames(List<Info> names)//get all names of pins that we need in the output
        {
            string filePath = "";
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while((line = reader.ReadLine()) != null) 
                    {
                        string[] info = line.Split(',');
                        Info i = new Info(info[0], info[2], info[3], info[1]);
                        names.Append(i);
                    }
                }
            }
            catch (Exception ex)
            {
                //Eroror, cannot read the file
            }
        }

        public static List<Pin> InitPins()//Get all pins from the big xml file
        {
            //list
            List<Pin> allpins = new List<Pin>();

            //direction
            string file1 = @"C:\Users\rozen\OneDrive\Рабочий стол\Work\iMX8M-PLUS.xml";

            //document 
            XDocument xdoc = XDocument.Load(file1);

            var pins = from c in xdoc.Elements("pinsmodelsignal_configuration").Elements("pins").Elements("pin")
                       select c;
            int i = 0;
            //get pins
            foreach (var pin in pins)
            {
                string name = string.Empty, description = string.Empty, coords = string.Empty, power_group = string.Empty;
                string name_part = string.Empty, package_function = string.Empty, signal = string.Empty, peripheral = string.Empty;
                try
                {
                    name = pin.Attribute("name").Value.ToString();
                    description = pin.Attribute("description").Value.ToString();
                    coords = pin.Attribute("coords").Value.ToString();//
                    power_group = pin.Attribute("power_group").Value.ToString();
                }
                catch (Exception ex)
                {
                    //error
                }

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

    }
}
