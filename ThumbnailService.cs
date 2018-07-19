using System.IO;

namespace SvgViewer
{
    public class ThumbnailService
    {
        public string FilePath { get; }
        public string ThumbnailPath { get; }

        public string ThumnbnailDirectory { get; }

        public ThumbnailService(string filePath)
        {
            FilePath = filePath;
            ThumnbnailDirectory = Path.Combine(Directory.GetParent(FilePath).FullName, ".Thumbnail");
            ThumbnailPath = Path.Combine(ThumnbnailDirectory, $"{Path.GetFileName(FilePath)}.png");
        }

        public void CreateThumbnailDirectory()
        {
            Directory.CreateDirectory(ThumnbnailDirectory);
        }

        public bool ExistsThumnail => File.Exists(ThumbnailPath);

        public MemoryStream GetThumnailMemorySteream()
        {
            if (ExistsThumnail)
                return  new MemoryStream(File.ReadAllBytes(Path.GetFullPath(ThumbnailPath)));

            return null;
        }
    }
}
