﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Protocol
    {
        public Protocol()
        {
            Phases = new List<Phase>();
            Agents = new List<Agent>();
        }

        public List<Phase> Phases { get; set; }
        public List<Agent> Agents { get; set; }
    }
}