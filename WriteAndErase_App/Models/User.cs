using System;
using System.Collections.Generic;

namespace WriteAndErase_App.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Usersurname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Userpatronymic { get; set; } = null!;

    public string Userlogin { get; set; } = null!;

    public string Userpassword { get; set; } = null!;

    public int Userrole { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role UserroleNavigation { get; set; } = null!;
}
