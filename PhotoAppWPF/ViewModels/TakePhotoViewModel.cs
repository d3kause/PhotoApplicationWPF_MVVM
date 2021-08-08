using AForge.Video;
using AForge.Video.DirectShow;
using PhotoAppWPF.Infrastructure;
using PhotoAppWPF.Infrastructure.Commands;
using PhotoAppWPF.ViewModels.Base;
using PhotoAppWPF.Views;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoAppWPF.ViewModels
{
    internal class TakePhotoViewModel : ViewModel
    {
        #region Variables
        private readonly VideoCaptureDevice _localWebCam;
        private readonly FilterInfoCollection LocalWebCamsCollection;
        #endregion

        #region Properties
        #region Title
        private string _title = "Take new photo";
        public string Title 
        { 
            get => _title; 
            set => Set(ref _title, value); 
        }
        #endregion

        #region CameraSource
        private ImageSource _cameraSource;
        public ImageSource CameraSource
        {
            get => _cameraSource;
            set => Set(ref _cameraSource, value);
        }
        #endregion

        #region ImageTitle
        private string _imageTitle;
        public string ImageTitle
        {
            get => _imageTitle;
            set
            {
                Set(ref _imageTitle, value);
                CanTakePhoto = CanSavePhoto = ImageTitle?.Length > 0;
            }
        }

        #region ImgDescription
        private string _imageDescription;
        public string ImageDescription
        {
            get => _imageDescription;
            set => Set(ref _imageDescription, value);
        }
        #endregion

        #region SnapshotDateTime
        private DateTime _snapshotDateTime;
        public DateTime SnapshotDateTime
        {
            get => _snapshotDateTime;
            set => Set(ref _snapshotDateTime, value);
        }
        #endregion
        #endregion

        #region CanTakePhoto
        private bool _canTakePhoto;
        public bool CanTakePhoto
        {
            get => _canTakePhoto;
            set => Set(ref _canTakePhoto, value);
        }
        #endregion

        #region CanSavePhoto
        private bool _canSavePhoto;
        public bool CanSavePhoto
        {
            get => _canSavePhoto;
            set => Set(ref _canSavePhoto, value);
        }
        #endregion

        #region IsTakePhotoVisible
        private Visibility _isTakePhotoVisible = Visibility.Visible;
        public Visibility IsTakePhotoVisible { get => _isTakePhotoVisible; set => Set(ref _isTakePhotoVisible, value); }
        #endregion

        #region ClosingApp
        private bool _closingApp;

        public bool ClosingApp
        {
            get => _closingApp;
            set
            {
                if (value)
                {
                    StopWebCam();
                }
                Set(ref _closingApp, value);
            }
        }
        #endregion
        #endregion

        #region Commands
        #region TakePhotoCommand
        public ICommand TakePhotoCommand { get; }
        private void OnTakePhotoCommandExecuted(object o)
        {
            StopWebCam();
            SnapshotDateTime = DateTime.Now;
            CanTakePhoto = false;
            CanSavePhoto = true;
            IsTakePhotoVisible = Visibility.Hidden;
        }
        private bool CanTakePhotoCommandExecute(object o) => true;
        #endregion

        #region SavePhotoCommand
        public ICommand SavePhotoCommand { get; }
        private void OnSavePhotoCommandExecuted(object o)
        {
            try
            {
                TakePhotoCommand.Execute(null);

                if (Directory.Exists(ConfigureParams.ImagesDirectory) == false)
                {
                    Directory.CreateDirectory(ConfigureParams.ImagesDirectory);
                }

                string filePath = GeneratuUniqueFilePath();
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)CameraSource));

                using (Stream stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    encoder.Save(stream);
                }

                SetCurrectMetadata(filePath);

                if (o is System.Windows.Window)
                {
                    PhotoGalleryView photoGalleryView = new PhotoGalleryView();
                    photoGalleryView.Show();
                    (o as System.Windows.Window).Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save photo exeption. {ex.Message}");
            }
        }
        private bool CanSavePhotoCommandExecute(object o) => CameraSource != null;
        #endregion

        #region GoBackCommand
        public ICommand GoBackCommand { get; }
        private void OnGoBackCommandExecuted(object o)
        {
            StopWebCam();
            if (o is System.Windows.Window)
            {
                PhotoGalleryView photoGalleryView = new PhotoGalleryView();
                photoGalleryView.Show();
                (o as System.Windows.Window).Close();
            }
        }
        private bool CanGoBackCommandExecute(object o) => true;
        #endregion
        #endregion

        #region UserMethods
        private void SetCurrectMetadata(string filePath)
        {
            var jpeg = new JpegMetadataAdapter(filePath);
            jpeg.Metadata.Title = ImageTitle;
            jpeg.Metadata.Comment = ImageDescription ?? "";
            jpeg.Metadata.DateTaken = SnapshotDateTime.ToString();
            jpeg.Save();              // Saves the jpeg in-place
        }

        private string GeneratuUniqueFilePath()
        {
            string fullPath = $"{ConfigureParams.ImagesDirectory}{ImageTitle}.jpg";
            if (File.Exists(fullPath))
            {
                for (int i = 2; File.Exists(fullPath); i++)
                {
                    fullPath = $"{ConfigureParams.ImagesDirectory}{ImageTitle}_{i}.jpg";
                }
            }
            return fullPath;
        }

        public void StopWebCam()
        {
            _localWebCam.SignalToStop();
            _localWebCam.WaitForStop();
            _localWebCam.Stop();
        }

        private void GetNewFrameFromWebcam(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = new BitmapImage();
                    bi.BeginInit();
                    MemoryStream ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Bmp);
                    bi.StreamSource = ms;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                }
                bi.Freeze();
                new Thread(() => CameraSource = bi).Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update webcam exeption. {ex.Message}");
            }

        }
        #endregion

        public TakePhotoViewModel()
        {
            TakePhotoCommand = new LambdaCommand(OnTakePhotoCommandExecuted, CanTakePhotoCommandExecute);
            SavePhotoCommand = new LambdaCommand(OnSavePhotoCommandExecuted, CanSavePhotoCommandExecute);
            GoBackCommand = new LambdaCommand(OnGoBackCommandExecuted, CanGoBackCommandExecute);
            LocalWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _localWebCam = new VideoCaptureDevice(LocalWebCamsCollection[0].MonikerString);
            _localWebCam.NewFrame += new NewFrameEventHandler(GetNewFrameFromWebcam);
            _localWebCam.Start();
        }
    }
}