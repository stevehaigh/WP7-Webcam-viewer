// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemViewModel.cs" company="Steve Haigh">
//   
// </copyright>
// <summary>
//   Defines the ItemViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StAntonCams.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Net;

    /// <summary>
    /// View Model for individual webcams, compatible with MVVM architecture.
    /// </summary>
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
        /// File name in local storage.
        /// </summary>
        private string cameraFileName;

        /// <summary>
        /// Last update time stamp.
        /// </summary>
        private DateTime lastUpdated;
        
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name to display
        /// </summary>
        /// <returns></returns>
        public string CameraName
        {
            get
            {
                return this.cameraName;
            }

            set
            {
                if (value != this.cameraName)
                {
                    this.cameraName = value;
                    this.NotifyPropertyChanged("CameraName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of for static camera image.
        /// </summary>
        /// <returns></returns>
        public string CameraUrl
        {
            get
            {
                return this.cameraUrl;
            }

            set
            {
                if (value != this.cameraUrl)
                {
                    this.cameraUrl = value;
                    this.NotifyPropertyChanged("CameraUrl");
                }
            }
        }

        /// <summary>
        /// Gets or sets the file name of for static camera image in local storage.
        /// </summary>
        /// <returns></returns>
        public string CameraFileName
        {
            get
            {
                return this.cameraFileName;
            }

            set
            {
                if (value != this.cameraFileName)
                {
                    this.cameraFileName = value;
                    this.NotifyPropertyChanged("CameraFileName");
                }
            }
        }

        /// <summary>
        /// Gets or setsmthe time of last update of iage in local store.
        /// </summary>
        public DateTime LastUpdated
        {
            get
            {
                var retVal = DateTime.MinValue;
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (myIsolatedStorage.FileExists(this.CameraFileName))
                    {
                        return myIsolatedStorage.GetCreationTime(this.CameraFileName).UtcDateTime.ToLocalTime();
                    }
                }
                
                return retVal;
            }

            set
            {
                if (value != this.lastUpdated)
                {
                    this.lastUpdated = value;
                    this.NotifyPropertyChanged("LastUpdated");
                }
            }
        }

        /// <summary>
        /// Updates images in local storage.
        /// </summary>
        /// <param name="force">if set to <c>true</c> the images will always be updared, otherwide images are only updae if they are more than 1 hour old.</param>
        public void Update(bool force)
        {
            // Only update if last update was more than an hour ago
            if (force || this.LastUpdated < DateTime.Now.AddHours(-1))
            {
                var request = (HttpWebRequest)WebRequest.Create(this.CameraUrl);
                request.BeginGetResponse(this.ResponseCallback, request);
            }
        }

        /// <summary>
        /// Response callback called when image data is returned.
        /// </summary>
        /// <param name="result">The async result object.</param>
        private void ResponseCallback(IAsyncResult result)
        {
            var request = (HttpWebRequest)result.AsyncState;
            var response = request.EndGetResponse(result);

            using (var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var tempFileName = this.CameraFileName + ".tmp";

                using (var fileStream = new IsolatedStorageFileStream(tempFileName, FileMode.Create, myIsolatedStorage))
                {
                    using (var writer = new BinaryWriter(fileStream))
                    {
                        var resourceStream = response.GetResponseStream();
                        var length = resourceStream.Length;
                        var buffer = new byte[4096];
                        var readCount = 0;

                        using (var reader = new BinaryReader(resourceStream))
                        {
                            while (readCount < length)
                            {
                                var actual = reader.Read(buffer, 0, buffer.Length);
                                readCount += actual;
                                writer.Write(buffer, 0, actual);
                            }
                        }
                    }
                }

                if (myIsolatedStorage.FileExists(this.CameraFileName))
                {
                    Debug.WriteLine("Removing {0}", this.CameraFileName);
                    myIsolatedStorage.DeleteFile(this.CameraFileName);
                }

                Debug.WriteLine("Creating {0}", this.CameraFileName);
                myIsolatedStorage.MoveFile(tempFileName, this.CameraFileName);

                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.LastUpdated = DateTime.Now;
                });
            }
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}