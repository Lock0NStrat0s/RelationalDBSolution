using Microsoft.Extensions.Configuration;
using DataAccessLibrary;
using DataAccessLibrary.Models;

public class Program
{
    private static void Main(string[] args)
    {
        SqlCRUD sql = new SqlCRUD(GetConnectionString());

        //ReadAllContacts(sql);

        //ReadContact(sql, 1);

        //CreateNewContact(sql);

        //UpdateContact(sql);

        //RemovePhoneNumberFromContact(sql, 3, 5);

        Console.WriteLine("Done Processing");

        Console.ReadLine();
    }

    private static void RemovePhoneNumberFromContact(SqlCRUD sql, int contactId, int phoneNumberId)
    {
        sql.RemovePhoneNumberFromContact(contactId, phoneNumberId);
    }

    private static void UpdateContact(SqlCRUD sql)
    {
        BasicContactModel contact = new BasicContactModel
        {
            Id = 1,
            FirstName = "Tom",
            LastName = "Corey"
        };
        sql.UpdateContactName(contact);
    }

    private static void CreateNewContact(SqlCRUD sql)
    {
        FullContactModel user = new FullContactModel
        {
            BasicInfo = new BasicContactModel
            {
                FirstName = "Hakuna",
                LastName = "Matata"
            }
        };

        user.EmailAddresses.Add(new EmailAddressModel { EmailAddress = "nope@aol.com" });
        user.EmailAddresses.Add(new EmailAddressModel { Id = 1, EmailAddress = "mj@me.com" });

        user.PhoneNumbers.Add(new PhoneNumberModel { Id = 1, PhoneNumber = "123-456" });
        user.PhoneNumbers.Add(new PhoneNumberModel { PhoneNumber = "852-963" });

        sql.CreateContact(user);
    }

    private static void ReadAllContacts(SqlCRUD sql)
    {
        var rows = sql.GetAllContacts();

        foreach (var row in rows)
        {
            Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
        }
    }

    private static void ReadContact(SqlCRUD sql, int contactId)
    {
        var contact = sql.GetFullContactById(contactId);

        Console.WriteLine($"{contact.BasicInfo.Id}: {contact.BasicInfo.FirstName} {contact.BasicInfo.LastName}");

    }

    private static string GetConnectionString(string connectionStringName = "Default")
    {
        string output = "";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = builder.Build();

        output = config.GetConnectionString(connectionStringName);

        return output;
    }


}