using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog_V1
{
    internal class Pin
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the coords.
        /// </summary>
        /// <value>
        /// The coords.
        /// </value>
        public string Coords { get; set; }
        /// <summary>
        /// Gets or sets the power group.
        /// </summary>
        /// <value>
        /// The power group.
        /// </value>
        public string Power_group { get; set; }

        /// <summary>
        /// Gets or sets the arr.
        /// </summary>
        /// <value>
        /// The arr.
        /// </value>
        public Node<ALT> Arr { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pin"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="coords">The coords.</param>
        /// <param name="power_group">The power group.</param>
        public Pin(string name, string description, string coords, string power_group)
        {
            this.Name = name;
            this.Description = description;
            this.Coords = coords;
            this.Power_group = power_group;
            this.Arr = null;
        }
        /// <summary>
        /// Determines whether [is in arr] [the specified x].
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>
        ///   <c>true</c> if [is in arr] [the specified x]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInArr(ALT x)
        {
            Node<ALT> p = this.Arr;
            while (p != null)
            {
                if (p.GetValue().Equals(x))
                {
                    return true;
                }
                p = p.GetNext();
            }
            return false;
        }
        /// <summary>
        /// Adds the alt.
        /// </summary>
        /// <param name="x">The x.</param>
        public void AddALT(ALT x)
        {
            if (this.Arr == null)
            {
                this.Arr = new Node<ALT>(x);
            }
            else
            {
                Node<ALT> p = this.Arr;
                while (p.GetNext() != null)
                {
                    p = p.GetNext();
                }

                p.SetNext(new Node<ALT>(x));
            }
        }
        /// <summary>
        /// Adds the alts.
        /// </summary>
        /// <param name="x">The x.</param>
        public void AddAlts(Node<ALT> x)
        {
            Node<ALT> p = x;
            while (p != null)
            {
                AddALT(p.GetValue());
                p = p.GetNext();
            }
        }
        /// <summary>
        /// Counts the alts.
        /// </summary>
        /// <returns></returns>
        public int CountAlts()
        {
            int count = 0;
            Node<ALT> p = this.Arr;
            while(p != null)
            {
                count++;
                p=p.GetNext();
            }
            return count;
        }
        /// <summary>
        /// Delets the alt.
        /// </summary>
        /// <param name="name_part">The name part.</param>
        public void DeletAlt(string name_part)
        {
            if (this.Arr == null)
            {
                //Console.WriteLine("null");
                return;

            }

            if (this.Arr.GetValue().Name_part.Equals(name_part))
            {
                this.Arr = this.Arr.GetNext();
                //Console.WriteLine("first");
            }

            Node<ALT> current = this.Arr, pre = null;
            while (current != null && !current.GetValue().Name_part.Equals(name_part))
            {
                pre = current; ;
                current = current.GetNext();
            }
            if (current == null)
            {
                //Console.WriteLine("not found");
                return;
            }
            pre.SetNext(current.GetNext());
        }
    }
}
