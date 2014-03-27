using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Key : IStatement
    {
        List<Agent> agents { get; set; }

        public Agent getOpposed(Agent a)
        {
            foreach (Agent agent in agents)
                if (agent != a)
                    return agent;
            return null;
        }
    }
}
