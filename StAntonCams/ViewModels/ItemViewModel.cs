using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace StAntonCams
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Name to display
        /// </summary>
        private string cameraName;

        /// <summary>
        /// URL of for static camera image.
        /// </summary>
        private string cameraUrl;

        /// <summary>
        /// Name to display
        /// </summary>
        /// <returns></returns>
        public string CameraName
        {
            get
            {
                return cameraName;
            }
            set
            {
                if (value != cameraName)
                {
                    cameraName = value;
                    NotifyPropertyChanged("CameraName");
                }
            }
        }

        /// <summary>
        /// URL of for static camera image.
        /// </summary>
        /// <returns></returns>
        public string CameraUrl
        {
            get
            {
                return cameraUrl;
            }
            set
            {
                if (value != cameraUrl)
                {
                    cameraUrl = value;
                    NotifyPropertyChanged("CameraUrl");
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
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