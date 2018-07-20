using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using SvgViewer.Core;
using SvgViewer.Utility;

namespace SvgViewer.ViewModel
{
    public class MainWindowVm : NotifyPropertyChanger
    {
        public ObservableCollection<SvgVm> Items { get; } = new ObservableCollection<SvgVm>();
        
        private double _scale = 128;
        public double Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }

        public ICollectionView ItemsView { get; }

        private string _filterWord;
        public string FilterWord
        {
            get => _filterWord;
            set
            {
                SetProperty(ref _filterWord, value);
                if (string.IsNullOrWhiteSpace(_filterWord))
                    ItemsView.Filter = null;
                else
                    ItemsView.Filter = x => x.ToString().Contains(_filterWord);
            }
        }

        public MainWindowVm()
        {
            var svgs = Directory.EnumerateFiles("Svgs", "*.svg", SearchOption.AllDirectories);
            var thumbnailSystem = new SvgThumbnailSystem(null,new StaWorkerManager(4));
            foreach (var svg in svgs)
                Items.Add(new SvgVm(svg, thumbnailSystem));
            ItemsView = new ListCollectionView(Items);
        }
    }
}
