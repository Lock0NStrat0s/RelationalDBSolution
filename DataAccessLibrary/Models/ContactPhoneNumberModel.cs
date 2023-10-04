using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models;

public class ContactPhoneNumberModel
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public int PhoneNumberId { get; set; }
}
