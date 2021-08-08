using PhotoAppWPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhotoAppWPF.Models
{
    public class PhotoModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Path { get; set; }
        public ImageSource ImgSource { get => new BitmapImage(new Uri(Path));  }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoModel"/> class.
        /// </summary>
        /// <param name="path">The path to .jpg file</param>
        public PhotoModel(string path)
        {
            var metaAdapter = new JpegMetadataAdapter(path);
            Title = metaAdapter.Metadata.Title;
            Description = metaAdapter.Metadata.Comment;
            string dateTaken = metaAdapter.Metadata.DateTaken;
            if (dateTaken != null)
                CreationDateTime = DateTime.Parse(dateTaken);
            Path = path;
        }
    }
}
