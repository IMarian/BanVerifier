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
            Nonces = new List<string>();
            Keys = new List<Key>();
            Messages = new List<Message>();
            Agents = new List<Agent>();
            Encrypted = new Key();
        }

        public List<String> Nonces { get; set; }
        public List<Key> Keys { get; set; }
        public List<Message> Messages { get; set; }
        public List<Agent> Agents { get; set; }
        // Should be replaced with
        public List<IStatement> statements { get; set; }

        public Key Encrypted { get; set; }

        public bool Fresh
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nEncrypted: " + Encrypted.ToString());

            sb.Append("\nNonces: ");
            foreach (string nonce in Nonces)
                sb.Append(nonce);

            sb.Append("\nKeys: ");
            foreach (Key key in Keys)
                sb.Append(key.ToString());

            sb.Append("\nMessages: ");
            foreach (Message m in Messages)
            {
                sb.Append(m.ToString());
            }

            sb.Append("\nAgents: ");
            foreach (Agent a in Agents)
            {
                sb.Append(a.Id);
            }

            return sb.ToString();
        }
    }
}
