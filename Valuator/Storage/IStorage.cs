using System.Collections.Generic;

namespace Valuator
{
    public interface IStorage
    {
        void Store(string key, string value);

        bool Indeed(string prefix, string value);

        string Load(string key);
    }
}