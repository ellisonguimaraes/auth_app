namespace AuthApp.Domain.Email;

public class To
{
    public string Name { get; set; }

    public string Address { get; set; }

    public To(string name, string address)
    {
        Name = name;
        Address = address;
    }
}
