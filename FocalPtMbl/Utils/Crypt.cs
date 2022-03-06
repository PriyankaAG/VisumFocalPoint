﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Xamarin.Forms;
using FocalPoint.Utils;

[assembly: Dependency(typeof(Crypt))]
namespace FocalPoint.Utils
{
        public class Crypt : ICrypt
        {
            public string Encrypt(string passKey, string sValue)
            {
                try
                {
                    MD5CryptoServiceProvider md5Obj = new MD5CryptoServiceProvider();
                    byte[] keyBytes = Encoding.ASCII.GetBytes(passKey);
                    keyBytes = md5Obj.ComputeHash(keyBytes);

                    // Create a new instance of the Rijndael
                    // class.  This generates a new key and initialization 
                    // vector (IV).
                    using (Rijndael myRijndael = Rijndael.Create())
                    {
                        // Encrypt the string to an array of bytes.
                        byte[] encrypted = EncryptStringToBytes(sValue, keyBytes, myRijndael.IV);

                        // Decrypt the bytes to a string.
                        byte[] mByte = new byte[myRijndael.IV.Length + encrypted.Length];
                        myRijndael.IV.CopyTo(mByte, 0);
                        encrypted.CopyTo(mByte, 16);
                        return Convert.ToBase64String(mByte);
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }

            public string Decrypt(string passKey, string sValue)
            {
                try
                {
                    System.Security.Cryptography.MD5CryptoServiceProvider md5Obj = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] keyBytes = Encoding.ASCII.GetBytes(passKey);
                    keyBytes = md5Obj.ComputeHash(keyBytes);

                    // Create a new instance of the Rijndael
                    // class.  This generates a new key and initialization 
                    // vector (IV).

                    byte[] mByte = Convert.FromBase64String(sValue);
                    byte[] ivByte = new byte[16];
                    byte[] dByte = new byte[mByte.Length - ivByte.Length];
                    Buffer.BlockCopy(mByte, 0, ivByte, 0, ivByte.Length);
                    Buffer.BlockCopy(mByte, 16, dByte, 0, dByte.Length);

                    using (Rijndael myRijndael = Rijndael.Create())
                    {
                        return DecryptStringFromBytes(dByte, keyBytes, ivByte);
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }

            private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
            {
                // Check arguments.
                if (plainText == null || plainText.Length <= 0)
                {
                    throw new ArgumentNullException("plainText");
                }
                if (Key == null || Key.Length <= 0)
                {
                    throw new ArgumentNullException("Key");
                }
                if (IV == null || IV.Length <= 0)
                {
                    throw new ArgumentNullException("Key");
                }
                byte[] encrypted = null;
                // Create an Rijndael object
                // with the specified key and IV.
                using (Rijndael rijAlg = Rijndael.Create())
                {

                    rijAlg.Key = Key;
                    rijAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {

                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                // Return the encrypted bytes from the memory stream.
                return encrypted;

            }
            //EncryptStringToBytes

            private static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
            {
                // Check arguments.
                if (cipherText == null || cipherText.Length <= 0)
                {
                    throw new ArgumentNullException("cipherText");
                }
                if (Key == null || Key.Length <= 0)
                {
                    throw new ArgumentNullException("Key");
                }
                if (IV == null || IV.Length <= 0)
                {
                    throw new ArgumentNullException("Key");
                }
                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;

                // Create an Rijndael object
                // with the specified key and IV.
                using (Rijndael rijAlg = Rijndael.Create())
                {
                    rijAlg.Key = Key;
                    rijAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

                return plaintext;

            }
            //DecryptStringFromBytes 
        }
    }
