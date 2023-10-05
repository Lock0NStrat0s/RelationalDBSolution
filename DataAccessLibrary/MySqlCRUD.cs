using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary;

public class MySqlCRUD
{
    private readonly string _connectionString;
    private MySqlDataAccess db = new MySqlDataAccess();

    public MySqlCRUD(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<BasicContactModel> GetAllContacts()
    {
        string sql = "select Id, FirstName, LastName from contacts";

        return db.LoadData<BasicContactModel, dynamic>(sql, new { }, _connectionString);
    }

    public FullContactModel GetFullContactById(int id)
    {
        string sql = "select Id, FirstName, LastName from contacts where Id = @Id";
        FullContactModel output = new FullContactModel();

        output.BasicInfo = db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();

        if (output.BasicInfo == null)
        {
            // do something to tell the user that the record was not found
            return null;
        }

        sql = @"select e.*
                    from emailaddresses e
                    inner
                    join contactemail ce on ce.EmailAddressId = e.Id
                    where ce.ContactId = @Id";

        output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);

        sql = @"select p.*
                    from phoneNumbers p
                    inner join contactphonenumbers cp on cp.PhoneNumberId = p.Id
                    where cp.ContactId = @Id";

        output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);

        return output;
    }

    public void CreateContact(FullContactModel contact)
    {
        // Save the basic contact
        string sql = "insert into contacts (FirstName, LastName) values (@FirstName, @LastName);";
        db.SaveData(sql,
                    new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                    _connectionString);

        // Get the ID number of the contact
        sql = "select Id from contacts where FirstName = @FirstName and LastName = @LastName;";
        int contactId = db.LoadData<IdLookupModel, dynamic>(
            sql,
            new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
            _connectionString).First().Id;

        foreach (var phoneNumber in contact.PhoneNumbers)
        {
            if (phoneNumber.Id == 0)
            {
                sql = "insert into phonenumbers (PhoneNumber) values (@PhoneNumber);";
                db.SaveData(sql, new { phoneNumber.PhoneNumber }, _connectionString);

                sql = "select Id from phonenumbers where PhoneNumber = @PhoneNumber;";
                phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(sql,
                    new { phoneNumber.PhoneNumber },
                    _connectionString).First().Id;
            }

            sql = "insert into contactphoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";
            db.SaveData(sql, new { ContactId = contactId, PhoneNumberId = phoneNumber.Id }, _connectionString);
        }

        foreach (var email in contact.EmailAddresses)
        {
            if (email.Id == 0)
            {
                sql = "insert into emailaddresses (EmailAddress) values (@EmailAddress);";
                db.SaveData(sql, new { email.EmailAddress }, _connectionString);

                sql = "select Id from emailaddresses where EmailAddress = @EmailAddress;";
                email.Id = db.LoadData<IdLookupModel, dynamic>(sql, new { email.EmailAddress }, _connectionString).First().Id;
            }

            sql = "insert into contactemail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
            db.SaveData(sql, new { ContactId = contactId, EmailAddressId = email.Id }, _connectionString);
        }
    }

    public void UpdateContactName(BasicContactModel contact)
    {
        string sql = "update contacts set FirstName = @FirstName, LastName = @LastName where Id = @Id";
        db.SaveData(sql, contact, _connectionString);
    }

    public void RemovePhoneNumberFromContact(int contactId, int phoneNumberId)
    {
        string sql = "select Id, ContactId, PhoneNumberId from contactphonenumbers where PhoneNumberId = @PhoneNumberId;";
        var links = db.LoadData<ContactPhoneNumberModel, dynamic>(sql,
            new { PhoneNumberId = phoneNumberId },
            _connectionString);

        sql = "delete from contactphoneNumbers where PhoneNumberId = @PhoneNumberId and ContactId = @ContactId";
        db.SaveData(sql, new { PhoneNumberId = phoneNumberId, ContactId = contactId }, _connectionString);

        if (links.Count == 1)
        {
            sql = "delete from phonenumbers where Id = @PhoneNumberId;";
            db.SaveData(sql, new { PhoneNumberId = phoneNumberId }, _connectionString);
        }
    }
}
