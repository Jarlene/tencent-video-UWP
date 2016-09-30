﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace TVWP.Class
{
    class FileManage
    {
        #region file 
        readonly static byte[] cypher = System.Text.Encoding.UTF8.GetBytes("huqiang@1990outlook.com");
        public static byte[] AES_Encrypt(byte[] input, byte[] pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            //string encrypted = "";
            //try
            //{
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(pass));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);//key1
                Array.Copy(temp, 0, hash, 15, 16);//key2

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                //IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(input));
                //encrypted = CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(AES, Buffer, null));
                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(input);
                byte[] Encrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Encrypt(AES, Buffer, null), out Encrypted);
                return Encrypted;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
        public static byte[] AES_Decrypt(byte[] input, byte[] pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            //string decrypted = "";
            //try
            //{
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(pass));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);//key1
                Array.Copy(temp, 0, hash, 15, 16);//key2

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));
                //IBuffer Buffer = CryptographicBuffer.DecodeFromBase64String(input);
                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(input);
                byte[] Decrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(AES, Buffer, null), out Decrypted);
                //decrypted = System.Text.Encoding.UTF8.GetString(Decrypted, 0, Decrypted.Length);

                return Decrypted;
            //}
            //catch (Exception ex)
            //{                
            //    return null;
            //}
        }

        public static void SaveFile(string text,string name)
        {
            byte[] buff=  Encoding.Unicode.GetBytes(text);
            IsolatedStorageFile temp = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs;
            if (temp.FileExists(name))
                fs = temp.OpenFile(name, FileMode.OpenOrCreate);
            else fs = temp.CreateFile(name);
            fs.Write(buff, 0, buff.Length);
            fs.Dispose();
            temp.Dispose();
        }
        public static void LoadDisposeFile()
        {
            IsolatedStorageFile temp = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs;
            if (temp.FileExists("vs"))
            {
                fs = temp.OpenFile("vs", FileMode.Open);
                byte[] b = new byte[fs.Length];
                fs.Read(b,0,b.Length);
                //ReadGood(AES_Decrypt(b,cypher));
            }                
            else
            {
                fs = temp.CreateFile("vs");
                //fs.Write(b,0,b.Length);
            } 
            fs.Dispose();
            temp.Dispose();
        }
        public static void SaveDisposeFile()
        {
            IsolatedStorageFile temp = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fs;
            if (temp.FileExists("vs"))
                fs = temp.OpenFile("vs", FileMode.OpenOrCreate);
            else
                fs = temp.CreateFile("vs");
            byte[] b = new byte[2400];
            //GoodsToByte(ref b);
            b = AES_Encrypt(b, cypher);
            fs.Write(b,0,b.Length);
            fs.Dispose();
            temp.Dispose();
        }
        #endregion
    }
}
