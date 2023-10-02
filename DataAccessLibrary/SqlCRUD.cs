using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary;

public class SqlCRUD
{
    private readonly string _connectionString;
    private SqlDataAccess db = new();

    public SqlCRUD(string connectionString)
    {
        _connectionString = connectionString;
    }

    //READ
    public List<BasicContactModel> GetAllContacts()
    {
        string sql = "select Id, FirstName, LastName from dbo.Contacts";

        return db.LoadData<BasicContactModel, dynamic>(sql, new { }, _connectionString);
    }

    //WRITE
    public FullContactModel GetFullContactById(int id)
    {
        string sql = "select Id, FirstName, LastName from dbo.Contacts where Id = @Id";
        FullContactModel output = new();

        output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

        if (output.BasicInfo == null)
        {
            //do something to tell the user that the record was not found like
            //throw new Exception("User not found");
            //or return null
            return null;
        }

        sql = @"select e.*
                from dbo.EmailAddresses e
                inner join dbo.ContactEmail ce on ce.EmailAddressId = e.Id
                where ce.ContactId = @Id";

        output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

        sql = @"select p.*
                from dbo.PhoneNumbers p
                inner join dbo.ContactPhoneNumbers cp on cp.PhoneNumberId = p.Id
                where cp.ContactId = @Id";

        output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

        return output;
    }

    //CREATE
    public void CreateContact(FullContactModel contact)
    {
        // Save the basic contact
        string sql = "insert into dbo.Contacts (FirstName, LastName) value (@FirstName, @LastName);";
        db.SaveData(sql,
                    new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                    _connectionString);

        // Get ID number of the contact
        sql = "select Id from dbo.Contacts where FirstName = @FirstName and LastName = @LastName;";
        int contactId = db.LoadData<IdLookupModel, dynamic>(sql,
                                                          new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                                                          _connectionString).First().Id;

        
        foreach (var phoneNumber in contact.PhoneNumbers)
        {
            if (phoneNumber.Id == 0)
            {
                sql = "insert into dbo.PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
                db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

                phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                                                                     new { phoneNumber.PhoneNumber },
                                                                     _connectionString).First().Id;
            }

            sql = "insert into dbo.ContactPhoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";

            db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
        }

        
        foreach (var email in contact.EmailAddresses)
        {
            if (email.Id == 0)
            {
                sql = "insert into dbo.EmailAddresses (EmailAddress) values (@EmailAddress);";
                db.SaveData(sql, new { email.EmailAddress }, _connectionString);

                email.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { email.EmailAddress }, _connectionString).First().Id;
            }

            sql = "insert into dbo.ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";

            db.SaveData(sql, new { ContactId = contactId, EmailAddressId = email.Id }, _connectionString);
        }


        // Identify if the phone number exists
            // Insert into the link table for that number
            // Insert the new phone number if not, and get the id
            // Then do the link table insert
        // Do the same for email

    }
}
