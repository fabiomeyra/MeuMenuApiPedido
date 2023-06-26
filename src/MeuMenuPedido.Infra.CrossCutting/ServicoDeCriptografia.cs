using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace MeuMenuPedido.Infra.CrossCutting;

[ExcludeFromCodeCoverage]
public static class ServicoDeCriptografia
{
    // Chave secreta a ser usada pelo algoritmo simétrico.
    static readonly byte[] Key =
    {
        37, 120, 23, 42, 107, 142, 29, 251, 155, 116, 105, 88, 86, 189, 181, 79, 152, 39, 81, 216, 216, 141, 130, 81,
        31, 177, 130, 64, 185, 253, 248, 87
    };

    // Vetor de inicialização.
    static readonly byte[] Iv = { 145, 231, 35, 51, 239, 138, 190, 74, 182, 91, 176, 189, 240, 105, 43, 253 };


    /// <summary>
    ///     Método responsável por Criptografar um valor passado como parâmetro.
    /// </summary>
    /// <param name="value">Valor a ser criptografado.</param>
    /// <returns>Valor criptografado.</returns>
    public static string Criptografar(string value)
    {
        // Check arguments.
        if (value is not { Length: > 0 })
            throw new ArgumentNullException(nameof(value));

        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = Iv;

            // Create a decrytor to perform the stream transform.
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(value);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    ///     Método responsável por Criptografar um valor passado como parâmetro.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Descriptografar(string value)
    {
        // Check arguments.
        if (value == null || value.Length <= 0)
            throw new ArgumentNullException(nameof(value));

        // Declare the string used to hold the decrypted text.

        // Create an Aes object with the specified key and IV.
        using var aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = Iv;

        // Create a decrytor to perform the stream transform.
        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for decryption.
        using var msDecrypt = new MemoryStream(Convert.FromBase64String(value));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        // Read the decrypted bytes from the decrypting stream and place them in a string.
        var plaintext = srDecrypt.ReadToEnd();

        return plaintext;
    }
}