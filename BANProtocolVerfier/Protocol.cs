using System;
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
            Keys = new List<Key>();
        }

        public List<Phase> Phases { get; set; }
        public List<Agent> Agents { get; set; }
        public List<Key> Keys { get; set; }

        public string runProtocol()
        {
			StringBuilder sb = new StringBuilder();
			
			sb.Append("\r\n======= Initial knowledge =======\r\n\r\n");
			
            foreach (var agent in Agents)
                sb.Append(agent.ToString());

            sb.Append("\r\n======= First knowledge expansion =======\r\n\r\n");
			
            foreach (var agent in Agents)
			{
				agent.expandKnowledge();
                sb.Append(agent.ToString());
			}
		
			foreach (var phase in Phases)
			{
                sb.Append("\r\n======= Phase " + (Phases.IndexOf(phase) + 1) + " =======\r\n" + phase.ToString());
				Agent currentAgent = phase.Receiver;
				currentAgent.addKnowledge(new Knowledge(currentAgent, KnowledgeType.sees, phase.message));
				currentAgent.expandKnowledge();
				sb.Append("\r\n"  + currentAgent.ToString());
			}
				
			return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var agent in Agents)
                sb.Append(agent.ToString());

            foreach (var phase in Phases)
                sb.Append("\r\n======= Phase " + (Phases.IndexOf(phase) + 1) + " =======\r\n" + phase.ToString());

            return sb.ToString();
        }
    }
}
