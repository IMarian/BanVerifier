using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Action : IStatement
    {
        public Agent Agent { get; set; }

        public String Type { get; set; }

        public IStatement Statement { get; set; }
    }
}
