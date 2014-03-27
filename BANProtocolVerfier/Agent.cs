using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Agent
    {
        public Agent()
        {
            Nonces = new List<string>();
            Keys = new List<string>();
            Actions = new List<Action>();
        }

        public string Id { get; set; }
        public List<String> Nonces { get; set; }
        public List<String> Keys { get; set; }
        // We rock.

        public List<Action> Actions { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("------------ Agent " + this.Id + (" ------------"));
            sb.Append("\nNonces: ");
            foreach (string nonce in this.Nonces)
            {
                sb.Append(nonce + " ");
            }

            sb.Append("\nKeys: ");
            foreach (string key in this.Keys)
            {
                sb.Append(key + " ");
            }

            return sb.ToString();
        }
    }
}
