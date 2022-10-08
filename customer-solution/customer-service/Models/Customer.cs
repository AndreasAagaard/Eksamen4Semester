namespace customer_service.Models;

public class Customer
{
    public int CustomerId;

    public Customer(string name, string mobile, string email)
    {
        Name = name;
        Mobile = mobile;
        Email = email;
    }

    public string Name { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate = DateTime.Now;
}
