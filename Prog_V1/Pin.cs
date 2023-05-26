using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog_V1
{
    internal class Pin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Coords { get; set; }
        public string Power_group { get; set; }

        public Node<ALT> Arr { get; set; }

        public Pin(string name, string description, string coords, string power_group)
        {
            this.Name = name;
            this.Description = description;
            this.Coords = coords;
            this.Power_group = power_group;
            this.Arr = null;
        }

        public bool IsInArr(ALT x)
        {
            Node<ALT> p = this.Arr;
            while (p != null)
            {
                if (p.Equals(x))
                {
                    return true;
                }
                p = p.GetNext();
            }
            return false;
        }

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
        public void AddAlts(Node<ALT> x)
        {
            Node<ALT> p = x;
            while (p != null)
            {
                AddALT(p.GetValue());
                p = p.GetNext();
            }
        }
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
    }
}
