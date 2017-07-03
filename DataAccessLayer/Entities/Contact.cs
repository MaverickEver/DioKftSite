namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class Contact : EntityBase
    {
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber{ get; set; }
        public string ImageUrl { get; set; }
    }
}
