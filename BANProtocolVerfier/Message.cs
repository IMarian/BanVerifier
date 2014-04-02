using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Message : IStatement
    {
        public Message()
        {
            Nonces = new List<Nonce>();
            Keys = new List<Key>();
            Messages = new List<Message>();
            Agents = new List<Agent>();
            Encrypted = new Key();

            statements = new List<IStatement>();
        }

        public List<Nonce> Nonces { get; set; }
        public List<Key> Keys { get; set; }
        public List<Message> Messages { get; set; }
        public List<Agent> Agents { get; set; }
		
        public List<IStatement> statements { get; set; }

        public Key Encrypted { get; set; }

        public bool Fresh { get; set; }

        public void refreshStatementsList()
        {
            statements = new List<IStatement>();
            statements.AddRange(Nonces);
            statements.AddRange(Keys);
            statements.AddRange(Messages);
        }

        public bool contains(IStatement statementToBeChecked)
        {
            foreach (IStatement statement in statements)
                if (statement.Equals(statementToBeChecked))
                    return true;
            return false;
        }

        private string getTabs(int number)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < number; i++ )
                sb.Append("\t");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(1);
        }

        public string ToString(int numberOfTabs)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Message: \r\n" + getTabs(numberOfTabs) + "Encrypted: " + Encrypted.ToString());

			if(Nonces.Count != 0)
			{
                sb.Append("\r\n" + getTabs(numberOfTabs) + "Nonce(s): ");
				foreach (Nonce nonce in Nonces)
					sb.Append(nonce.id);
			}

			if(Keys.Count() != 0)
			{
                sb.Append("\r\n" + getTabs(numberOfTabs) + "Key(s): ");
				foreach (Key key in Keys)
					sb.Append(key.ToString());
			}
			
			if(Messages.Count() != 0)
			{
                sb.Append("\r\n" + getTabs(numberOfTabs));
				foreach (Message m in Messages)
                    sb.Append(m.ToString(numberOfTabs + 1));
            }

			if(Agents.Count() != 0)
			{
                sb.Append("\r\n" + getTabs(numberOfTabs) + "Agent(s): ");
				foreach (Agent a in Agents)
					sb.Append(a.Id);
            }

            return sb.ToString();
        }
    }
}
