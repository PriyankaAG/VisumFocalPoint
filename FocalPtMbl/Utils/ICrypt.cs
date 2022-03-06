using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.Utils
{
    public interface ICrypt
    {
        string Encrypt(string passKey, string sValue);
        string Decrypt(string passKey, string sValue);
    }
}
