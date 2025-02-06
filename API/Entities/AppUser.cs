using System;
using System.Runtime.InteropServices;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
}
