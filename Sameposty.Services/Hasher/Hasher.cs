using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Sameposty.Services.Hasher;
public static class Hasher
{
    /// <summary>
    /// Hashes the password.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <param name="saltString">The salt string.</param>
    /// <returns>A string.</returns>
    public static string HashPassword(string password, string saltString)
    {
        byte[] salt = System.Text.Encoding.ASCII.GetBytes(saltString);

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        return hashed;
    }

    /// <summary>
    /// Gets the salt.
    /// </summary>
    /// <returns>A string.</returns>
    public static string GetSalt()
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string saltString = System.Text.Encoding.UTF8.GetString(salt);
        return saltString;
    }
}
