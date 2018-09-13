using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Infrastructure
{
    public class FileWatchHelper
    {
        public delegate void LoadFileAsyncDeletegate(IFileProvider fileProvider);


        public LoadFileAsyncDeletegate LoadFileAsync;

        public FileWatchHelper(LoadFileAsyncDeletegate loadfiledelagate)
        {
            LoadFileAsync = loadfiledelagate;
        }

        public IFileProvider FileProvider { get; set; }

        public string Path { get; set; }

        public void FileWatch(string path)
        {
            if (!System.IO.Path.IsPathRooted(path))
                return;
            string directoryName = System.IO.Path.GetDirectoryName(path);
            string path2 = System.IO.Path.GetFileName(path);
            for (;
                !string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName);
                directoryName = System.IO.Path.GetDirectoryName(directoryName))
                path2 = System.IO.Path.Combine(System.IO.Path.GetFileName(directoryName), path2);
            if (!Directory.Exists(directoryName))
                return;
            this.FileProvider = (IFileProvider) new PhysicalFileProvider(directoryName);
            this.Path = path2;
            WatchFile(this.FileProvider);
        }


        public void FileWatch(IConfigurationBuilder configuration,string path)
        {
            if (!System.IO.Path.IsPathRooted(path))
                return;
            string directoryName = System.IO.Path.GetDirectoryName(path);
            string path2 = System.IO.Path.GetFileName(path);
            for (;
                !string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName);
                directoryName = System.IO.Path.GetDirectoryName(directoryName))
                path2 = System.IO.Path.Combine(System.IO.Path.GetFileName(directoryName), path2);
            if (!Directory.Exists(directoryName))
                return;
            this.FileProvider = configuration.GetFileProvider();
            this.Path = path2;
            WatchFile(this.FileProvider);
        }


        private void WatchFile(IFileProvider fileProvider)
        {
            if (LoadFileAsync != null)
            {
                ChangeToken.OnChange(() => fileProvider.Watch(System.IO.Path.GetFileName(Path)),
                    () => LoadFileAsync(fileProvider));
            }
        }
    }
}