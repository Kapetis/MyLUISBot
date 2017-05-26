using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLUISBot.Models
{
    public interface IRepository<T> where T:class
    {
       IEnumerable<T> GetAll();
    }
}
