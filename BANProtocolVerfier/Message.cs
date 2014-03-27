using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Message : IStatement
    {
        public List<String> Nonces { get; set; }
        public List<Key> Keys { get; set; }
        public List<Message> Messages { get; set; }
        public List<Agent> Agents { get; set; }
        // Should be replaced with
        public List<IStatement> statements { get; set; }

        public Key Encrypted { get; set; }
    }
}
