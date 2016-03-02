namespace AppDomain.Engage.Population.DTO.Demographics
{
    public class Phone
    {
        public string Id { get; set; }
        public long Number { get; set; }
        public string TypeId { get; set; }
        public bool IsText { get; set; }
        public bool PhonePreferred { get; set; }
        public bool TextPreferred { get; set; }
        public bool OptOut { get; set; }
        public string DataSource { get; set; }
    }
}
