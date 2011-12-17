// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Steve Haigh">
//   
// </copyright>
// <summary>
//   Defines the MainPage type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StAntonCams
{
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

    /// <summary>
    /// MainPage codebehind. Code here is allowed fo rsetup only. Everything else is a sad refelction on the programming model or the author's ability to abstract
    /// in to an acceptable model based architecure.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// The tile gets some image updates, so we need a schedule.
        /// </summary>
        private readonly ShellTileSchedule tileSchedule = new ShellTileSchedule();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            this.Loaded += this.MainPage_Loaded;

            // Set up the tile here too
            this.tileSchedule.Interval = UpdateInterval.EveryHour;
            this.tileSchedule.MaxUpdateCount = 50;
            this.tileSchedule.Recurrence = UpdateRecurrence.Interval;
            this.tileSchedule.RemoteImageUri = new Uri(@"http://livecam.abbag.com/galzig.jpg");
            this.tileSchedule.Start();
        }

        /// <summary>
        /// Handles the Loaded event of the MainPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.RefreshCameras(false);
        }

        /// <summary>
        /// Handles the Click event of the ApplicationBarIconButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.RefreshCameras(true);
        }
    }
}