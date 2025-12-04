using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Security
{
    public interface IRijndaelEncryption                                                                                                                                                                                                                                                                                                                
    {
        string Encryption(string plainText);
        string Decryption(string cipherText);
    }
}
