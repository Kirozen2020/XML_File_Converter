using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Prog_V1
{
    internal class ALT
    {
        //string name_part, package_function, assy, notes;

        //string signal, peripheral, channel;
        //string group_name_postfix1, padname1, value1;
        //string group_name_postfix2, padname2, value2;

        public string Name_part { get; set; }
        public string Package_function { get; set; }
        public string Assy { get; set; }
        public string Notes { get; set; }

        public string Signal { get; set; }
        public string Peripheral { get; set; }
        public string Channel { get; set; }

        public string Group_name_postfix1 { get; set; }
        public string Padname1 { get; set; }
        public string Value1 { get; set; }

        public string Group_name_postfix2 { get; set; }
        public string Padname2 { get; set; }
        public string Value2 { get; set; }

        public ALT(string name_part, string package_function, string assy, string notes, string signal, string peripheral, string channel, string group_name_postfix1, string padname1, string value1, string group_name_postfix2, string padname2, string value2)
        {
            this.Name_part = name_part;
            this.Package_function = package_function;
            this.Assy = assy;
            this.Notes = notes;
            this.Signal = signal;
            this.Peripheral = peripheral;
            this.Channel = channel;
            this.Group_name_postfix1 = group_name_postfix1;
            this.Padname1 = padname1;
            this.Value1 = value1;
            this.Group_name_postfix2 = group_name_postfix2;
            this.Padname2 = padname2;
            this.Value2 = value2;
        }
        public ALT(string name_part, string package_function, string assy, string notes, string signal, string peripheral)
        {
            this.Name_part = name_part;
            this.Package_function = package_function;
            this.Assy = assy;
            this.Notes = notes;
            this.Signal = signal;
            this.Peripheral = peripheral;

            this.Channel = string.Empty;
            this.Group_name_postfix1 = string.Empty;
            this.Padname1 = string.Empty;
            this.Value1 = string.Empty;
            this.Group_name_postfix2 = string.Empty;
            this.Padname2 = string.Empty;
            this.Value2 = string.Empty;
        }
        public ALT(string name_part, string package_function, string signal, string peripheral)
        {
            Name_part = name_part;
            Package_function = package_function;
            Signal = signal;
            Peripheral = peripheral;

            this.Assy = string.Empty;
            this.Notes = string.Empty;
            this.Channel = string.Empty;
            this.Group_name_postfix1 = string.Empty;
            this.Padname1 = string.Empty;
            this.Value1 = string.Empty;
            this.Group_name_postfix2 = "-sleep";
            this.Padname2 = string.Empty;
            this.Value2 = string.Empty;
        }
    }
}
