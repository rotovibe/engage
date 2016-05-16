namespace Phytel.Engage.Integrations.DomainEvents
{
    public class LogStatus : IDomainEvent
    {
        public IIntegrationEntity IntegrationEntity { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }

        public static LogStatus Create(string message, bool debug)
        {
            var type = debug == true ? LogType.Debug : LogType.Error;
            var ls = new LogStatus { Message = message, Type = type};
            return ls;
        }
    }
}
