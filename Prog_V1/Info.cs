using LINQtoCSV;
using System;

namespace Prog_V1
{
    internal class Info
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public string Assy { get; set; }

        
        public Info(string id, string name, string note, string assy)
        {
            Id = id;
            Name = name;
            Note = note;
            Assy = assy;
        }
    }
}
