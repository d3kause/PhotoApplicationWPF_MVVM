using PhotoAppWPF.Models;
using PhotoAppWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhotoAppWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для ConcretePhotoView.xaml
    /// </summary>
    public partial class ConcretePhotoView : Window
    {
        public ConcretePhotoView(PhotoModel photoData)
        {
            InitializeComponent();

            if (DataContext is ConcretePhotoViewModel)
            {
                (DataContext as ConcretePhotoViewModel).PhotoData = photoData;
            }
        }
    }
}
