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
        }

        public List<String> Nonces { get; set; }
        public List<Key> Keys { get; set; }
        public List<Message> Messages { get; set; }
        public String Encrypted { get; set; }
        public List<Agent> Agents { get; set; }

        public bool Fresh { get; set; }
    }
}
