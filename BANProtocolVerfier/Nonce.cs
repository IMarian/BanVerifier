using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANProtocolVerfier
{
    public class Nonce : IStatement
    {
        public bool Fresh { get; set; }
		
		public string id { get; set; }
		
        public Nonce(string newId)
        {
            id = newId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Nonce))
                return false;

            return id.Equals(((Nonce)obj).id);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Nonce " + id);

            if(Fresh)
                sb.Append(" is fresh");

            return sb.ToString();
        }

        public string ToString(int numberOfTabs)
        {
            return ToString();
        }
    }
}
