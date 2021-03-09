using System.Collections.Generic;

namespace Valuator
{
    public interface IStorage
    {
        void Store(string key, string value);
        string StoreKey(string key);
        bool TextSignes(string prefix, string text);
        string Load(string key);
    }
}