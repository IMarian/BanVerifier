using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Key : IStatement
    {
        public Key(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public bool Fresh { get; set; } 
    }
}
