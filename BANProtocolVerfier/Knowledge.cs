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

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Knowledge))
                return false;

            Knowledge objKnowledge = (Knowledge)obj;

            if (agent.Id.Equals(objKnowledge.agent.Id) && knowledgeType.Equals(objKnowledge.knowledgeType)
                    && statement.Equals(objKnowledge.statement))
                return true;
            else
                return false;

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
