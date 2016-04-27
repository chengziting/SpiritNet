using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Enums
{
    public enum EncryptEnum
    {
        /// <summary>
        /// AES对称加密
        /// </summary>
        AES = 0,
        /// <summary>
        /// DES对称加密
        /// </summary>
        DES = 1,
        /// <summary>
        /// RSA非对称公钥加密
        /// </summary>
        RSA = 2
    }
}
