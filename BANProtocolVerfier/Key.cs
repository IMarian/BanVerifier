using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Key : IStatement
    {
        public bool Fresh { get; set; }

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

        public override bool Equals(object obj)
        {
            if (obj is Key && obj != null)
            {
                Key objKey = (Key)obj;
                foreach (Agent agent in agents)
                    if (!objKey.agents.Contains(agent))
                        return false;

                foreach (Agent agent in objKey.agents)
                    if (!agents.Contains(agent))
                        return false;

                if (objKey.Fresh != Fresh)
                    return false;

                return true;
            }
            else return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Key: ");

            foreach (Agent agent in agents)
                sb.Append(agent.Id + " ");

            return sb.ToString();
        }

        public string ToString(int numberOfTabs)
        {
            return ToString();
        }
    }
}
