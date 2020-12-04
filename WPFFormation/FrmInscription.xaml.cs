using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFFormation
{
    /// <summary>
    /// Logique d'interaction pour FrmInscription.xaml
    /// </summary>
    public partial class FrmInscription : Window
    {
        formationEntities gst;
        public FrmInscription(formationEntities uneformationEntities)
        {
            InitializeComponent();
            gst = uneformationEntities;
            lstFormations.ItemsSource = gst.formation.ToList();
        }

        private void lstFormations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFormations.SelectedItem != null)
            {

                List<agent> allAgents = gst.agent.ToList();
                List<inscription> allInscriptionsParFormation = gst.inscription.ToList().FindAll(i => i.numFormation == (lstFormations.SelectedItem as formation).idFormation);

                var query_agent_nonInscrits = from ag in allAgents
                                              where !allInscriptionsParFormation.Any(ins => ins.numAgent == ag.idAgent)
                                              select ag;

                lstAgentsNonInscrits.ItemsSource = query_agent_nonInscrits.ToList();
            }
        }

        private async void lstAgentsNonInscrits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstAgentsNonInscrits.SelectedItem != null)
            {
                string ville1 = (lstFormations.SelectedItem as formation).lieuFormation;
                string ville2 = (lstAgentsNonInscrits.SelectedItem as agent).villeAgent;
                // un objet de type http client
                HttpClient ws = new HttpClient(); // ws : webservice
                string response = await ws.GetStringAsync("https://fr.distance24.org/route.json?stops=" + ville1 + "|" + ville2);
                var json = JsonConvert.DeserializeObject<dynamic>(response);
                txtKM.Text = json.distance;

               //montant = (durée de la formation* prix) +(Km * 1, 36)
                formation selected_formation = (lstFormations.SelectedItem as formation);
                // var km = Convert.ToInt16(json.distance);
                int km = Convert.ToInt16(json.distance);
                txtMontantTotal.Text = ((selected_formation.dureeFormation * selected_formation.prixFormation) + (km * 1.36)).ToString();
            }
          
        }

        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {
            inscription newInscriptions = new inscription()
            {
                nbKm = Convert.ToInt16(txtKM.Text),
                presence = 0,
                numAgent = (lstAgentsNonInscrits.SelectedItem as agent).idAgent,
                numFormation = (lstFormations.SelectedItem as formation).idFormation
            };

            gst.inscription.Add(newInscriptions);
            gst.SaveChanges();

            // refresh la list view
            List<agent> allAgents = gst.agent.ToList();
            List<inscription> allInscriptionsParFormation = gst.inscription.ToList().FindAll(i => i.numFormation == (lstFormations.SelectedItem as formation).idFormation);

            var query_agent_nonInscrits = from ag in allAgents
                                          where !allInscriptionsParFormation.Any(ins => ins.numAgent == ag.idAgent)
                                          select ag;

            lstAgentsNonInscrits.ItemsSource = query_agent_nonInscrits.ToList();
        }
    }
}
