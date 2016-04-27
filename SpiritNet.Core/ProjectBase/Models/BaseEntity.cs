using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Models
{
    public abstract class BaseEntity :
        BaseModel
    {

        public BaseEntity()
        {
            this.CreateUser = string.Empty;
            this.LastUpdateUser = string.Empty;
            this.CreateDateTime = DateTime.Now;
            this.LastUpdateDateTime = DateTime.Now;
        }
        private System.String _createUser;
        ///<summary>
        ///创建人
        ///</summary> 
        public virtual System.String CreateUser
        {
            get { return _createUser; }
            set
            {
                _createUser = value;
            }
        }

        private System.DateTime? _createDateTime;
        ///<summary>
        ///创建时间
        ///</summary> 
        public virtual System.DateTime? CreateDateTime
        {
            get { return _createDateTime; }
            set
            {
                _createDateTime = value;
            }
        }

        private System.String _lastUpdateUser;
        ///<summary>
        ///上次更新人
        ///</summary> 
        public virtual System.String LastUpdateUser
        {
            get { return _lastUpdateUser; }
            set
            {
                _lastUpdateUser = value;
            }
        }

        private System.DateTime? _lastUpdateDateTime;
        ///<summary>
        ///上次更新时间
        ///</summary> 
        public virtual System.DateTime? LastUpdateDateTime
        {
            get { return _lastUpdateDateTime; }
            set
            {
                _lastUpdateDateTime = value;
            }
        }

        #region 方法
        /// <summary>
        /// 更新时候，设置操作人和操作时间
        /// </summary>
        /// <param name="accessUser">The access user.</param>
        public virtual void SetUpdate(string accessUser)
        {
            LastUpdateUser = accessUser;
            LastUpdateDateTime = DateTime.Now;
        }
        /// <summary>
        /// 创建时，设置操作人和操作时间
        /// </summary>
        /// <param name="accessUser">The access user.</param>
        public virtual void SetCreate(string accessUser)
        {
            CreateUser = accessUser;
            CreateDateTime = DateTime.Now;
            LastUpdateUser = accessUser;
            LastUpdateDateTime = DateTime.Now;
        }


        #endregion
    }
}
