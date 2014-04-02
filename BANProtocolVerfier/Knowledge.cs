using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public enum KnowledgeType { believes, sees, said, controls }
	
    public class Knowledge : IStatement
    {
        public Agent agent { get; set; }

        public KnowledgeType knowledgeType { get; set; }

        public IStatement statement { get; set; }

        public bool Fresh { get; set; }

        public Knowledge(Agent newAgent, KnowledgeType newKnowledgeType, IStatement newStatement)
        {
            agent = newAgent;
            knowledgeType = newKnowledgeType;
            statement = newStatement;
        }

        public override string ToString()
        {
            return ToString(1);
        }

        public string ToString(int numberOfTabs)
        {
            return agent.Id + " " + knowledgeType.ToString() + " " + statement.ToString(numberOfTabs + 1);
        }
    }
}
