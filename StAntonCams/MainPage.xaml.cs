﻿using System;
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

            // Set the data context of the listbox control to the data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

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
        }
    }
}