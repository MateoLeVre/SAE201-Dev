using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppliNicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace AppliNicolas.Classes.Tests
{
    [TestClass()]
    public class DemandeTests
    {
        [TestMethod()]
        public void RecupereDemandeDansBDDTest()
        {
            ConnexionBD.Instance.ConfigurerConnexion("home", "home");

            Demande demande = new Demande();

            List<Demande> lesDemandes = demande.RecupereDemandeDansBDD();
            Assert.IsInstanceOfType(lesDemandes, typeof(List<Demande>), "Le try block devrait retourner une List<Demande>");
        }
    }
}