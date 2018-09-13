using System.Collections.Concurrent;
using System.IO;
using System.Xml;
using Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Game2048Api.Common
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;
        private readonly FileWatchHelper _watchHelper;
        private XmlElement _configXmlElement;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();
        public Log4NetProvider(string log4NetConfigFile)
        {
            _log4NetConfigFile = log4NetConfigFile;
            _watchHelper=new FileWatchHelper(Configure);
            _watchHelper.FileWatch(Path.Combine(Directory.GetCurrentDirectory(),_log4NetConfigFile));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, Parselog4NetConfigFile(_log4NetConfigFile));
        }

        private void Configure(IFileProvider fileProvider)
        {
            if (fileProvider != null)
            {
                Parselog4NetConfigFile(_log4NetConfigFile, true);
                foreach (var logger in _loggers)
                {
                    logger.Value.ReConfigure(_configXmlElement);

                }

            }

        }
        private  XmlElement Parselog4NetConfigFile(string filename,bool refresh=false)
        {
            if (refresh|| _configXmlElement == null)
            {
                XmlDocument log4netConfig = new XmlDocument();
                using (FileStream stream = File.OpenRead(filename))
                {
                    log4netConfig.Load(stream);
                }
              _configXmlElement= log4netConfig["log4net"];
            }
            return _configXmlElement;
        }
    }
}
