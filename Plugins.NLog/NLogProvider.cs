using NetCore.Logging;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Plugin.NLog
{
    /// <summary>
    /// customized NLog
    /// </summary>
    public class NLogProvider : BaseLogProvider, ILogProvider
    {
        #region Initialization

        private readonly ILogger logger;

        public NLogProvider(IConfiguration configuration) : base(configuration)
        {
            var config = new ConfigurationBuilder().AddConfiguration(configuration).Build().GetSection("NLog");
            LogManager.Configuration = new NLogLoggingConfiguration(config);
            this.logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();
        }

        #endregion Initialization

        public override void Debug(string message, params object[] args)
        {
            this.logger?.Debug(message, args);
        }

        public override void Info(string message, params object[] args)
        {
            this.logger?.Info(message, args);
        }

        public override void Warn(string message, params object[] args)
        {
            this.logger?.Warn(message, args);
        }

        public override void Error(string message, params object[] args)
        {
            this.logger?.Error(message, args);
        }

        public override void Fatal(string message, params object[] args)
        {
            this.logger?.Fatal(message, args);
        }
    }
}

