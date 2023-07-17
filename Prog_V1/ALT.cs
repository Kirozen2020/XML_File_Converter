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
        /// <summary>
        /// Gets or sets the name part.
        /// </summary>
        /// <value>
        /// The name part.
        /// </value>
        public string Name_part { get; set; }
        /// <summary>
        /// Gets or sets the package function.
        /// </summary>
        /// <value>
        /// The package function.
        /// </value>
        public string Package_function { get; set; }
        /// <summary>
        /// Gets or sets the assy.
        /// </summary>
        /// <value>
        /// The assy.
        /// </value>
        public string Assy { get; set; }
        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }
        /// <summary>
        /// Gets or sets the signal.
        /// </summary>
        /// <value>
        /// The signal.
        /// </value>
        public string Signal { get; set; }
        /// <summary>
        /// Gets or sets the peripheral.
        /// </summary>
        /// <value>
        /// The peripheral.
        /// </value>
        public string Peripheral { get; set; }
        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public string Channel { get; set; }
        /// <summary>
        /// Gets or sets the group name postfix1.
        /// </summary>
        /// <value>
        /// The group name postfix1.
        /// </value>
        public string Group_name_postfix1 { get; set; }
        /// <summary>
        /// Gets or sets the padname1.
        /// </summary>
        /// <value>
        /// The padname1.
        /// </value>
        public string Padname1 { get; set; }
        /// <summary>
        /// Gets or sets the value1.
        /// </summary>
        /// <value>
        /// The value1.
        /// </value>
        public string Value1 { get; set; }
        /// <summary>
        /// Gets or sets the group name postfix2.
        /// </summary>
        /// <value>
        /// The group name postfix2.
        /// </value>
        public string Group_name_postfix2 { get; set; }
        /// <summary>
        /// Gets or sets the padname2.
        /// </summary>
        /// <value>
        /// The padname2.
        /// </value>
        public string Padname2 { get; set; }
        /// <summary>
        /// Gets or sets the value2.
        /// </summary>
        /// <value>
        /// The value2.
        /// </value>
        public string Value2 { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ALT"/> class.
        /// </summary>
        /// <param name="name_part">The name part.</param>
        /// <param name="package_function">The package function.</param>
        /// <param name="assy">The assy.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="signal">The signal.</param>
        /// <param name="peripheral">The peripheral.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="group_name_postfix1">The group name postfix1.</param>
        /// <param name="padname1">The padname1.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="group_name_postfix2">The group name postfix2.</param>
        /// <param name="padname2">The padname2.</param>
        /// <param name="value2">The value2.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ALT"/> class.
        /// </summary>
        /// <param name="name_part">The name part.</param>
        /// <param name="package_function">The package function.</param>
        /// <param name="assy">The assy.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="signal">The signal.</param>
        /// <param name="peripheral">The peripheral.</param>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="ALT"/> class.
        /// </summary>
        /// <param name="name_part">The name part.</param>
        /// <param name="package_function">The package function.</param>
        /// <param name="signal">The signal.</param>
        /// <param name="peripheral">The peripheral.</param>
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
