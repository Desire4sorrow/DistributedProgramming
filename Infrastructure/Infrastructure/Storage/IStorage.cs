using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
     public interface IStorage
     {
         void Store(string sKey, string key, string value);
         bool TextSignes(string prefix, string text);
         string Load(string sKey, string key);
         bool CheckingKey(string key);
     }
}

