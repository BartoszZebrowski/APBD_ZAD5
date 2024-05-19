namespace APBD_ZAD5.Dto
{
    public class GetTripDto
    {
        public class Client
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int MaxPeople { get; set; }
        public IEnumerable<string> Countries { get; set; }
        public IEnumerable<Client> Clients { get; set; }
    }
}
