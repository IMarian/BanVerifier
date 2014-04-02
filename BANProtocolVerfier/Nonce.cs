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
		
        public override string ToString()
        {
            return "Nonce " + id;
        }

        public string ToString(int numberOfTabs)
        {
            return ToString();
        }
    }
}
