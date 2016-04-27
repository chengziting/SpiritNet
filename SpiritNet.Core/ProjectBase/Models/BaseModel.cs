using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Models
{
    public abstract class BaseModel
    {
        #region ToString()


        /// <summary>
        /// 序列化当前实例为Json字符串
        /// </summary>
        /// <returns>
        ///返回一个Json字符串
        /// </returns>
        public virtual string SerializeObject()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
