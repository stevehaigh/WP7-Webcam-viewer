///
///
///

namespace StAntonCams
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Collections.ObjectModel;
    using System.Windows.Resources;
    using System.Linq;

    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        private int selectedTileImageIndex;

        public string SelectedTileImageIndex
        {
            get
            {
                return selectedTileImageIndex.ToString();
            }
            set
            {
                if (value != selectedTileImageIndex.ToString())
                {
                    selectedTileImageIndex = int.Parse(value);
                    NotifyPropertyChanged("SelectedTileImageIndex");
                }
            }
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

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
            //this.Items.Add(new ItemViewModel() { CameraName = "Rendl", CameraUrl = "http://livecam.abbag.com/rendl.jpg", CameraFileName = "rendl.jpg" });
            //this.Items.Add(new ItemViewModel() { CameraName = "Town", CameraUrl = "http://livecam.abbag.com/skicenter.jpg", CameraFileName = "skicenter.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Nasserein", CameraUrl = "http://livecam.abbag.com/nasserein.jpg", CameraFileName = "nasserein.jpg" });
            //this.Items.Add(new ItemViewModel() { CameraName = "Gampen", CameraUrl = "http://livecam.abbag.com/gampen.jpg", CameraFileName = "gampen.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "St Christoph", CameraUrl = "http://livecam.abbag.com/christoph1.jpg", CameraFileName = "christoph1.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Kapall", CameraUrl = "http://livecam.abbag.com/kapallbig.jpg", CameraFileName = "kapallbig.jpg" });
            this.Items.Add(new ItemViewModel() { CameraName = "Stuben", CameraUrl = "http://livecam.abbag.com/stuben.jpg", CameraFileName = "stuben.jpg" });

            this.IsDataLoaded = true;
        }

        public void RefreshCameras()
        {
            foreach (var item in Items)
            {
                item.Update();
            }

            this.NotifyPropertyChanged("Items");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}