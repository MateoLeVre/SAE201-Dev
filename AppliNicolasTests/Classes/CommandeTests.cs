using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliNicolas.Classes.Tests
{
    [TestClass()]
    public class CommandeTests
    {
        [TestMethod()]
        public void RecupereCommandeDansBDDTestTRY()
        {
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            Commande commande = new Commande();

            List<Commande> lesCommandes = commande.RecupereCommandeDansBDD();
            Assert.IsInstanceOfType(lesCommandes, typeof(List<Commande>), "Le try block devrait retourner une List<Commande>");
        }
    }
}