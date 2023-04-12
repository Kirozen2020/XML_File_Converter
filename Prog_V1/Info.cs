using LINQtoCSV;
using System;

namespace Prog_V1
{
    internal class Info
    {
        [CsvColumn(Name = "Pin", FieldIndex = 1)]
        public string Id { get; set; }

        [CsvColumn(Name = "Pin name", FieldIndex = 3)]
        public string Name { get; set; }

        [CsvColumn(Name = "Notes", FieldIndex = 4)]
        public string Note { get; set; }

        [CsvColumn(Name = "Assembly", FieldIndex = 2)]
        public string Assy { get; set; }

        /*
        public Info(string id, string name, string note, string assy)
        {
            Id = id;
            Name = name;
            Note = note;
            Assy = assy;
        }*/
    }
}
