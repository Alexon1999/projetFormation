using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour FrmPresence.xaml
    /// </summary>
    public partial class FrmPresence : Window
    {
        formationEntities gst;
        public FrmPresence(formationEntities uneFormationEntities)
        {
            InitializeComponent();
            gst = uneFormationEntities;
            lstFormations.ItemsSource = gst.formation.ToList();
            
        }

        private void btnPresent_Click(object sender, RoutedEventArgs e)
        {
            //foreach (agent a in lstAgentInscrits.SelectedItems)
            //{

            //    //gst.inscription.First(inscription => inscription.numAgent == a.idAgent).presence = 
            //}
            //foreach(ListItem li in  lstAgentInscrits.Items)
            //{
            //}
            gst.SaveChanges();
        }

        private void lstFormations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstFormations.SelectedItem != null)
            {
                List<agent> allAgents = gst.agent.ToList();
                List<inscription> allInscriptionsParFormation = gst.inscription.ToList().FindAll(i => i.numFormation == (lstFormations.SelectedItem as formation).idFormation);

                var query_agentInscrits = from ag in allAgents
                                              where allInscriptionsParFormation.Any(ins => ins.numAgent == ag.idAgent)
                                              select ag;
                lstAgentInscrits.ItemsSource = query_agentInscrits;
            }
        }
    }
}
