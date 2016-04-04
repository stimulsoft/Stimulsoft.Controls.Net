using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Stimulsoft.Base
{
    public class StiEncryption
    {

        #region Fields

        //private static UInt32 randomSeed = 0;
        private const int rand_m = 714025;
        private const int rand_a = 4096;
        private const int rand_c = 150889;

        #endregion

        #region Methods

        #region Encrypt

        public static byte[] Encrypt(byte[] src, byte[] key)
        {
            return EncryptAdv(src, key);
        }

        public static byte[] Encrypt(byte[] src, string password)
        {
            byte[] byteKey = GetKeyFromPassword(password);
            return EncryptAdv(src, byteKey);
        }

        public static string Encrypt(string src, string password)
        {
            if (src == null) return null;
            
            byte[] byteSrc = Encoding.UTF8.GetBytes(src);
            byte[] byteKey = GetKeyFromPassword(password);
            byte[] dst = EncryptAdv(byteSrc, byteKey);
            return Convert.ToBase64String(dst);
        }

        #endregion

        #region Decrypt

        public static byte[] Decrypt(byte[] src, byte[] key)
        {
            return DecryptAdv(src, key);
        }

        public static byte[] Decrypt(byte[] src, string password)
        {
            byte[] byteKey = GetKeyFromPassword(password);
            return DecryptAdv(src, byteKey);
        }

        public static string Decrypt(string src, string password)
        {
            byte[] byteSrc = Convert.FromBase64String(src);
            byte[] byteKey = GetKeyFromPassword(password);
            byte[] dst = DecryptAdv(byteSrc, byteKey);
            return Encoding.UTF8.GetString(dst);
        }

        #endregion

        public static byte[] GenerateRandomKey()
        {
            byte[] key = new byte[32];

            RandomNumberGenerator.Create();
            Random rand = new Random();
            for (int index = 0; index < 32; index++)
            {
                key[index] = (byte)rand.Next(0, 255);
            }

            return key;
        }

        #endregion

        #region Methods: Private

        private static byte[] EncryptAdv(byte[] src, byte[] key)
        {
            if (src == null) return null;

            byte[] dst = new byte[src.Length];

            dst = CryptRandom(src, key, true);
            dst = CryptXor(dst, key);
            dst = CryptShift(dst, key, true);

            return dst;
        }

        private static byte[] DecryptAdv(byte[] src, byte[] key)
        {
            if (src == null) return null;

            byte[] dst = new byte[src.Length];

            dst = CryptShift(src, key, false);
            dst = CryptXor(dst, key);
            dst = CryptRandom(dst, key, false);

            return dst;
        }

        private static byte[] CryptXor(byte[] src, byte[] key)
        {
            byte[] dst = new byte[src.Length];

            int pos = 0;
            int keyPos = 0;

            while (pos < src.Length)
            {
                if (keyPos >= key.Length) keyPos = 0;
                dst[pos] = (byte)(src[pos] ^ key[keyPos]);

                pos++;
                keyPos++;
            }

            return dst;
        }

        private static byte[] CryptShift(byte[] src, byte[] key, bool encrypt)
        {
            byte[] dst = new byte[src.Length];

            int pos = 0;
            int keyPos = 0;

            while (pos < src.Length)
            {
                if (keyPos >= key.Length) keyPos = 0;

                if (encrypt) dst[pos] = ShiftLeft(src[pos], key[keyPos]);
                else dst[pos] = ShiftRight(src[pos], key[keyPos]);

                pos++;
                keyPos++;
            }

            return dst;
        }

        private static byte ShiftLeft(byte value, byte count)
        {
            int res = value << (count & 0x07);
            res = (res & 0x00FF) | ((res & 0xFF00) >> 8);
            return (byte)res;
        }

        private static byte ShiftRight(byte value, byte count)
        {
            int res = value << (8 - (count & 0x07));
            res = (res & 0x00FF) | ((res & 0xFF00) >> 8);
            return (byte)res;
        }

        private static byte[] CryptRandom(byte[] src, byte[] key, bool encrypt)
        {
            byte[] dst = new byte[src.Length];
            int pos = 0;

            UInt32 randomSeed = SetRandomSeed(key);

            int[] randomMix = GetMixArray(src.Length, randomSeed);
            while (pos < src.Length)
            {
                if (encrypt) dst[pos] = src[randomMix[pos]];
                else dst[randomMix[pos]] = src[pos];

                pos++;
            }

            return dst;
        }

        private static int[] GetMixArray(int count, UInt32 randomSeed)
        {
            int[] check = new int[count];
            int[] mix = new int[count];
            for (int index = 0; index < count; index++)
            {
                check[index] = index;
            }
            for (int indexMix = 0; indexMix < count; indexMix++)
            {
                int rnd = GetRandom(0, count - indexMix - 1, ref randomSeed);
                mix[indexMix] = check[rnd];
                check[rnd] = check[count - indexMix - 1];
            }
            return mix;
        }

        private static UInt32 SetRandomSeed(byte[] key)
        {
            UInt32 randomSeed = (uint)(key[0] | (key[1] << 8) | (key[key.Length - 2] << 16) | (key[key.Length - 1] << 24));
            randomSeed = randomSeed % rand_m;
            return randomSeed;
        }

        private static int GetRandom(int min, int max, ref UInt32 randomSeed)
        {
            randomSeed = (randomSeed * rand_a + rand_c) % rand_m;
            int jran = (int)(min + ((max - min + 1) * randomSeed) / rand_m);
            return jran;
        }

        private static byte[] GetKeyFromPassword(string password)
        {
            MemoryStream ms = new MemoryStream();

            //MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
            //byte[] hash = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] hash = StiMD5Helper.ComputeHash(Encoding.UTF8.GetBytes(password));

            ms.Write(hash, 0, hash.Length);
            int pos = hash.Length;
            while (pos < password.Length)
            {
                //hash = hashMD5.ComputeHash(Encoding.UTF8.GetBytes(password.Substring(0, pos)));
                hash = StiMD5Helper.ComputeHash(Encoding.UTF8.GetBytes(password.Substring(0, pos)));

                ms.Write(hash, 0, (pos + hash.Length < password.Length ? hash.Length : password.Length - pos));
                pos += hash.Length;
            }
            //hashMD5.Clear();

            byte[] result = ms.ToArray();
            ms.Close();
            ms.Dispose();
            ms = null;

            return result;
        }

        #endregion

    }
}
