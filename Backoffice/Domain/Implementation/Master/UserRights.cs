using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class UserRights : IUserRights
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string MnuName { get; set; }
        public virtual string TableName { get; set; }
        public virtual string Operation { get; set; }
        public virtual string Description { get; set; }
    }
}
