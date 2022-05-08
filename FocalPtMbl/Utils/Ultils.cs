﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace FocalPoint.Utils
{
    public class Ultils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConvertPhone(long? num)
        {
            if (!num.HasValue)
                return null;

            return ConvertPhone(num.Value.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ConvertPhone(string num)
        {
            if (string.IsNullOrEmpty(num))
                return null;

            var phone = string.Format("{0:(###) ###-####}", num);

            //Appears to be a bug, that the "Format" doesn't convert it correctly, and leaves it as is
            if (phone.Length == phone.Length && phone.Length != 14 && phone.Length == 10)
                phone = string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6, 4));

            return phone;
        }

        public static string ConvertToBase64(Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(bytes);
        }

        public static string CompressImage(string base64String)
        {
            using (var outStream = new MemoryStream())
            {
                using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
                using (var mStream = new MemoryStream(Encoding.UTF8.GetBytes(base64String)))
                    mStream.CopyTo(tinyStream);

                var compressed = outStream.ToArray();
                return Convert.ToBase64String(compressed);
            }
        }

        public static string UnCompressImage(string base64String)
        {
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (var inStream = new MemoryStream(Convert.FromBase64String(base64String)))
                    using (var bigStream = new GZipStream(inStream, CompressionMode.Decompress))
                        bigStream.CopyTo(mStream);

                    return Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
            catch(Exception ex)
            {

            }
            return "";
        }
    }
}
