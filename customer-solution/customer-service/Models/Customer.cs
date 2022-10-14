namespace customer_service.Models;

public class Customer
{

    public Customer(string name, string mobile, string email)
    {
        Name = name;
        Mobile = mobile;
        Email = email;
    }

    public int? CustomerId;
    public string Name { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate = DateTime.Now;
}
