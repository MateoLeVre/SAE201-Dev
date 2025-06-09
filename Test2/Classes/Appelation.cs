using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes
{
    public class Appelation
    {
        public Appelation(int numAppelation, string nomAppelation)
        {
            NumAppelation = numAppelation;
            NomAppelation = nomAppelation;
        }

        private int numAppelation;
        public int NumAppelation
        {
            get { return numAppelation; }
            set { numAppelation = value; }
        }

        private string nomAppelation;
        public string NomAppelation
        {
            get { return nomAppelation; }
            set { nomAppelation = value; }
        }

        public override bool Equals(object? obj)
        {
            return obj is Appelation appelation &&
                   NumAppelation == appelation.NumAppelation;
        }
    }
}
