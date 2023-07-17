using System;

namespace Prog_V1
{
    internal class Info
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Note { get; set; }
        /// <summary>
        /// Gets or sets the assy.
        /// </summary>
        /// <value>
        /// The assy.
        /// </value>
        public string Assy { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="note">The note.</param>
        /// <param name="assy">The assy.</param>
        public Info(string id, string name, string note, string assy)
        {
            Id = id;
            Name = name;
            Note = note;
            Assy = assy;
        }
    }
}
