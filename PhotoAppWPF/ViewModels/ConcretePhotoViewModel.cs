using PhotoAppWPF.Infrastructure.Commands;
using PhotoAppWPF.Models;
using PhotoAppWPF.ViewModels.Base;
using System.Windows.Input;

namespace PhotoAppWPF.ViewModels
{
    internal class ConcretePhotoViewModel : ViewModel
    {
        #region Properties
        public string Title => $"View photo: \"{PhotoData?.Title}\" ";

        #region PhotoData
        private PhotoModel _photoData;
        public PhotoModel PhotoData
        {
            get => _photoData;
            set => Set(ref _photoData, value);
        }
        #endregion
        #endregion

        #region Commands
        #region GoBackCommand
        public ICommand GoBackCommand { get; }
        private void OnGoBackCommandExecuted(object o)
        {
            if (o is System.Windows.Window)
            {
                (o as System.Windows.Window).Close();
            }
        }
        private bool CanGoBackCommandExecute(object o) => true;
        #endregion
        #endregion

        public ConcretePhotoViewModel()
        {
            GoBackCommand = new LambdaCommand(OnGoBackCommandExecuted, CanGoBackCommandExecute);
        }
    }
}
