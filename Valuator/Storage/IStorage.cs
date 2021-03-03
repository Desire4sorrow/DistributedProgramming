using System.Collections.Generic;

namespace Valuator
{
    public interface IStorage
    {
        void Store(string key, string value);
        void StoreKey(string key);
        bool Indeed(string prefix, string value);
        List<string> TextSignes();
        string Load(string key);
    }
}