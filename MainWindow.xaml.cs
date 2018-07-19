using System.IO;
using System.Windows;
using System.Windows.Input;

namespace SvgViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnDragStart(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement && frameworkElement.DataContext != null && frameworkElement.DataContext is SvgVm svgVm)
            {
                var dataObject = new DataObject();
                dataObject.SetData(DataFormats.FileDrop, new[] { Path.GetFullPath(svgVm.FilePath) });
                DragDrop.DoDragDrop(frameworkElement, dataObject, DragDropEffects.Copy );
            }
        }
    }
}
