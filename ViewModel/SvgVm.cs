using System.IO;
using System.Windows.Media;
using SvgViewer.Core;
using SvgViewer.Utility;

namespace SvgViewer.ViewModel
{
    public class SvgVm : NotifyPropertyChanger, IFileObject
    {
        public string FilePath { get; }
        public string Name { get; }

        private readonly SvgThumbnailSystem _svgThumbnailSystem = new SvgThumbnailSystem(null);

        private ImageSource _image;

        public ImageSource Image => _image ?? _svgThumbnailSystem.GetImageSourceAsync(FilePath, null,
                                        source =>
                                        {
                                            _image = source;
                                            OnPropertyChanged();
                                        });

        public SvgVm(string filePath)
        {
            FilePath = filePath;
            Name = Path.GetFileName(FilePath);
        }

        public override string ToString() => Name;
    }
}
