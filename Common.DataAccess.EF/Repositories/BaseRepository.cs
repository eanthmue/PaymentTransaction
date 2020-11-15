using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Common.DataAccess.EF.Repositories
{
    public abstract class BaseRepository<TType> where TType : BaseEntity, new()
    {

    }
}
