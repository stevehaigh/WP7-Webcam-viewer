// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Steve Haigh">
//   
// </copyright>
// <summary>
//   Main View Model for the app, compatible with MVVM architecture.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StAntonCams.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// Main View Model for the app, compatible with MVVM architecture.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>Initializes a new instance of the <see cref="MainViewModel"/> class.</summary>
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>Property changed event handler.</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Gets the  collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is data loaded.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is data loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            this.Items.Add(new ItemViewModel() { CameraName = "Galzig", CameraUrl = "http://livecam.abbag.com/galzig.jpg", CameraFileName = "galzig.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Valluga", CameraUrl = "http://livecam.abbag.com/valluga.jpg", CameraFileName = "valluga.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Rendl", CameraUrl = "http://livecam.abbag.com/rendl.jpg", CameraFileName = "rendl.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Town", CameraUrl = "http://livecam.abbag.com/skicenter.jpg", CameraFileName = "skicenter.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Nasserein", CameraUrl = "http://livecam.abbag.com/nasserein.jpg", CameraFileName = "nasserein.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Gampen", CameraUrl = "http://livecam.abbag.com/gampen.jpg", CameraFileName = "gampen.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "St Christoph", CameraUrl = "http://livecam.abbag.com/christoph.jpg", CameraFileName = "christoph1.jpg" });
            ////this.Items.Add(new ItemViewModel() { CameraName = "Kapall", CameraUrl = "http://livecam.abbag.com/kapallbig.jpg", CameraFileName = "kapallbig.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Stuben", CameraUrl = "http://livecam.abbag.com/stuben.jpg", CameraFileName = "stuben.jpg" });

            this.IsDataLoaded = true;
        }

        /// <summary>
        /// Refreshes the cameras.
        /// </summary>
        /// <param name="force">if set to <c>true</c> updates will always ahppen, otherwide updates will only happen if images are more than 1 hour old.</param>
        public void RefreshCameras(bool force)
        {
            foreach (var item in this.Items)
            {
                item.Update(force);
            }
            
            this.NotifyPropertyChanged("Items");
        }

        /// <summary>
        /// Notifies that some property has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}