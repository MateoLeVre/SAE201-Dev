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
        private int numAppelation;
        private string nomAppelation;

        public Appelation(int numAppelation, string nomAppelation)
        {
            NumAppelation = numAppelation;
            NomAppelation = nomAppelation;
        }

        public int NumAppelation
        {
            get { return numAppelation; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le numéro d'appellation doit être positif");
                }
                numAppelation = value;
            }
        }

        public string NomAppelation
        {
            get { return nomAppelation; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom de l'appellation ne peut pas être vide");
                }
                if (value.Length > 100)
                {
                    throw new ArgumentException("Le nom de l'appellation ne peut pas dépasser 100 caractères");
                }
                nomAppelation = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Appelation appelation &&
                   NumAppelation == appelation.NumAppelation;
        }

        public override int GetHashCode()
        {
            return NumAppelation.GetHashCode();
        }

        public override string ToString()
        {
            return $"Appellation {NumAppelation}: {NomAppelation}";
        }
    }
}