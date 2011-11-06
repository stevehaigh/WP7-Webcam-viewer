using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using Microsoft.Phone.Shell;

namespace StAntonCams
{
    public partial class MainPage : PhoneApplicationPage
    {

        ShellTileSchedule tileSchedule = new ShellTileSchedule();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            // Set up initial tile
            StandardTileData NewTileData = new StandardTileData
            {
                BackTitle = "Last update: " + DateTime.Now.ToShortDateString(),
                BackBackgroundImage = new Uri("http://livecam.abbag.com/valluga.jpg"),
                BackContent = "View from Valluga"
            };


            // Set up the tile here too
            tileSchedule.Interval = UpdateInterval.EveryHour;
            tileSchedule.MaxUpdateCount = 50;
            tileSchedule.Recurrence = UpdateRecurrence.Interval;
            tileSchedule.RemoteImageUri = new Uri(@"http://livecam.abbag.com/galzig.jpg");
            
            tileSchedule.Start();
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            App.ViewModel.RefreshCameras();
            
        }
    }
}