using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace MySqlUI;

public class Program
{
    static void Main(string[] args)
    {
        MySqlCRUD sql = new MySqlCRUD(GetConnectionString());

        //ReadAllContacts(sql);

        //ReadContact(sql, 1);

        //CreateNewContact(sql);

        //UpdateContact(sql);
        //ReadAllContacts(sql);

        //RemovePhoneNumberFromContact(sql, 1, 1);

        Console.WriteLine("\nDone Processing MySQL");

        Console.ReadLine();
    }

    private static void RemovePhoneNumberFromContact(MySqlCRUD sql, int contactId, int phoneNumberId)
    {
        sql.RemovePhoneNumberFromContact(contactId, phoneNumberId);
    }

    private static void UpdateContact(MySqlCRUD sql)
    {
        BasicContactModel contact = new BasicContactModel
        {
            Id = 1,
            FirstName = "Blob",
            LastName = "Borey"
        };
        sql.UpdateContactName(contact);
    }

    private static void CreateNewContact(MySqlCRUD sql)
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

    private static void ReadAllContacts(MySqlCRUD sql)
    {
        var rows = sql.GetAllContacts();

        foreach (var row in rows)
        {
            Console.WriteLine($"{row.Id}: {row.FirstName} {row.LastName}");
        }
    }

    private static void ReadContact(MySqlCRUD sql, int contactId)
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