using PhotoAppWPF.Infrastructure;
using PhotoAppWPF.Infrastructure.Commands;
using PhotoAppWPF.Models;
using PhotoAppWPF.ViewModels.Base;
using PhotoAppWPF.Views;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PhotoAppWPF.ViewModels
{
    internal class PhotoGalleryViewModel : ViewModel
    {
        #region Properties
        public string Title => $"Photo application";

        #region SelectedItem
        private PhotoModel _selectedItem;
        public PhotoModel SelectedItem
        {
            get => _selectedItem; set
            {
                Set(ref _selectedItem, value);
                if (SelectedItem != null)
                {
                    GoToConcreteImage();
                }
            }
        }
        #endregion

        #region PhotoData
        private List<PhotoModel> _photoData;
        public List<PhotoModel> PhotoData
        {
            get => _photoData.OrderByDescending(o => o.CreationDateTime).ToList();
            set => Set(ref _photoData, value);
        }
        #endregion

        #endregion

        #region Commands
        #region ToTakePhotoCommand
        public ICommand ToTakePhotoCommand { get; }
        private void OnToTakePhotoCommandExecuted(object o)
        {

            if (o is System.Windows.Window)
            {
                TakePhotoView takephotoView = new TakePhotoView();
                takephotoView.Show();
                (o as System.Windows.Window).Close();
            }

        }
        private bool CanToTakePhotoCommandExecute(object o) => true;
        #endregion
        #endregion

        #region UserMethods
        private void GoToConcreteImage()
        {
            ConcretePhotoView photoViw = new ConcretePhotoView(SelectedItem);
            SelectedItem = null;
            photoViw.Show();
        }
        public static List<string> GetImagesPaths(string path) => Directory.GetFiles(path, "*jpg", SearchOption.TopDirectoryOnly).ToList();
        public static List<PhotoModel> ParsePhotoData(List<string> paths)
        {
            var data = new List<PhotoModel>();
            foreach (var p in paths)
            {
                if (p != null)
                {
                    data.Add(new PhotoModel(p));
                }
            }
            return data;
        }
        #endregion

        public PhotoGalleryViewModel()
        {
            ToTakePhotoCommand = new LambdaCommand(OnToTakePhotoCommandExecuted, CanToTakePhotoCommandExecute);
            _photoData = ParsePhotoData(GetImagesPaths(ConfigureParams.ImagesDirectory));
        } 
    }
}
