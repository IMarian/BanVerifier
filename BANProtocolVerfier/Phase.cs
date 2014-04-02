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

        }

        public Agent Sender { get; set; }
        public Agent Receiver { get; set; }
        public Message message { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\nAgent: " + Sender.Id);
            sb.Append("\r\nReceiver: " + Receiver.Id);
            sb.Append("\r\n" + message.ToString());

            return sb.ToString();
        }
    }
}
