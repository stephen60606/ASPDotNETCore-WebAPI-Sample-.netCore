namespace NetCore.Logging
{
    public abstract class BaseLogProvider : ILogProvider
    {
        protected IConfiguration configuration;

        public BaseLogProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public virtual void Debug(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual void Info(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual void Warn(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual void Error(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public virtual void Fatal(string message, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}

