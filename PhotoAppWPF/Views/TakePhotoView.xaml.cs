using PhotoAppWPF.Infrastructure;
using PhotoAppWPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace PhotoAppWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TakePhotoView : Window
    {
        public TakePhotoView()
        {
            InitializeComponent();
        }

        private void ClosingWindow(object sender, CancelEventArgs e)
        {
                (DataContext as TakePhotoViewModel).ClosingApp = true;
        }

    }
}

