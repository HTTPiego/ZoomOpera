
namespace ZoomOpera.Shared.Entities
{
    public readonly struct Address
    {

        public string State { get; init; }

        public string City { get; init; }

        public string ZipCode { get; init; }

        public string StreetAddress { get; init; }

        public int StreetAddressNumber { get; init; }

        public Address(string state, 
                        string city, 
                        string zipcode, 
                        string streetAddress, 
                        int streetAddressNumber)
        {
            this.State = state;
            this.City = city;
            this.ZipCode = zipcode;
            this.StreetAddress = streetAddress;
            this.StreetAddressNumber = streetAddressNumber;
        }

    }
}
