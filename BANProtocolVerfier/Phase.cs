using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Phase
    {
        public Phase()
        {
            Messages = new List<Message>();
        }

        public Agent Sender { get; set; }
        public Agent Receiver { get; set; }
        public List<Message> Messages { get; set; }
    }
}
