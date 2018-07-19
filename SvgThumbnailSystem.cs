using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SvgViewer
{
    public class SvgUtilSetupInfo
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public Brush Fill { get; set; } = Brushes.Gray;
        public double StrokeThickness { get; set; } = 1;

        public PenLineJoin StrokeLineJoin { get; set; } = PenLineJoin.Round;

        public int Width { get; set; } = 128;
        public int Height { get; set; } = 128;
    }

    public class SvgThumbnailSystem
    {
        private readonly SvgUtilSetupInfo _setupInfo;
        public SvgThumbnailSystem(SvgUtilSetupInfo setupInfo)
        {
            _setupInfo = setupInfo ?? new SvgUtilSetupInfo();
        }

        private void CreateThumbnail(string filePath)
        {
            var thumnbnailServis = new ThumbnailService(filePath);
            var view = new Grid()
            {
                Width = _setupInfo.Width,
                Height = _setupInfo.Height,
            };
            view.Children.Add(new System.Windows.Shapes.Path()
            {
                Data = SvgThumbnailUtil.GetGeometry(filePath),
                Fill = _setupInfo.Fill,
                Stroke = _setupInfo.Stroke,
                StrokeThickness = _setupInfo.StrokeThickness,
                Stretch = Stretch.Uniform,
                Opacity = 1.0,
                StrokeLineJoin = _setupInfo.StrokeLineJoin,
            });

            view.Measure(new Size(_setupInfo.Width, _setupInfo.Height));
            view.Arrange(new Rect(new Size(_setupInfo.Width, _setupInfo.Height)));

            //! rendering
            var bmp = new RenderTargetBitmap(_setupInfo.Width, _setupInfo.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(view);

            //! save and encode to file
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            thumnbnailServis.CreateThumbnailDirectory();
            using (var stream = File.Create(thumnbnailServis.ThumbnailPath))
                encoder.Save(stream);
        }

        public ImageSource GetImageSource(string filePath)
        {
            var thumnbnailServis = new ThumbnailService(filePath);

            if (thumnbnailServis.ExistsThumnail is false)
                CreateThumbnail(filePath);

            var image = new BitmapImage();

            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = thumnbnailServis.GetThumnailMemorySteream();
            image.EndInit();

            return image.DoFreeze();
        }

        public ImageSource GetImageSourceAsync(string filePath, ImageSource _default, Action<ImageSource> onLoaded)
        {
            TaskEx.RunOnSta(() =>
            {
                var imageSource = GetImageSource(filePath);
                Application.Current.Dispatcher.Invoke(() => onLoaded(imageSource));

                //! kill sta thread dispatcher. 
                Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.SystemIdle);
                Dispatcher.Run();
            });
            return _default;
        }
    }
}
