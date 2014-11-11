namespace Phytel.Services.Journal
{
    public class Error
    {
        public string ErrorMessage { get; set; }

        public Error InnerError { get; set; }

        public string StackTrace { get; set; }
    }
}