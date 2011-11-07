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
using System.Windows.Resources;
using System.IO.IsolatedStorage;
using System.IO;

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
        /// File name in local storage.
        /// </summary>
        private string cameraFileName;

        /// <summary>
        /// Last update time stamp.
        /// </summary>
        private DateTime lastUpdated;

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
        /// File name of for static camera image in local storage.
        /// </summary>
        /// <returns></returns>
        public string CameraFileName
        {
            get
            {
                return cameraFileName;
            }
            set
            {
                if (value != cameraFileName)
                {
                    cameraFileName = value;
                    NotifyPropertyChanged("CameraFileName");
                }
            }
        }

        /// <summary>
        /// File name of for static camera image in local storage.
        /// </summary>
        /// <returns></returns>
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
                if (value != lastUpdated)
                {
                    lastUpdated = value;
                    NotifyPropertyChanged("LastUpdated");
                }
            }
        }

        public void Update(bool force)
        {
            // Only update if last update was more than an hour ago
            if (force || this.LastUpdated < DateTime.Now.AddHours(-1))
            {
                var request = (HttpWebRequest)WebRequest.Create(this.CameraUrl);
                var result = (IAsyncResult)request.BeginGetResponse(ResponseCallback, request);
            }
        }

        private void ResponseCallback(IAsyncResult result)
        {

            var request = (HttpWebRequest)result.AsyncState;
            var response = request.EndGetResponse(result);

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var tempFileName = this.CameraFileName + ".tmp";

                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(tempFileName, FileMode.Create, myIsolatedStorage))
                {
                    using (BinaryWriter writer = new BinaryWriter(fileStream))
                    {
                        Stream resourceStream = response.GetResponseStream();
                        long length = resourceStream.Length;
                        byte[] buffer = new byte[4096];
                        int readCount = 0;

                        using (BinaryReader reader = new BinaryReader(resourceStream))
                        {
                            while (readCount < length)
                            {
                                int actual = reader.Read(buffer, 0, buffer.Length);
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

            //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
            //    this.NotifyPropertyChanged("CameraFileName");
            //});
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