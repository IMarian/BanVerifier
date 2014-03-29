using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Key
    {
        public Key()
        {
            agents = new List<Agent>();
        }
        public List<Agent> agents { get; set; }

        public Agent getOpposed(Agent a)
        {
            foreach (Agent agent in agents)
                if (agent != a)
                    return agent;
            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("K: ");

            foreach (Agent agent in agents)
            {
                sb.Append(agent.Id + " ");
            }

            return sb.ToString();
        }
    }
}
