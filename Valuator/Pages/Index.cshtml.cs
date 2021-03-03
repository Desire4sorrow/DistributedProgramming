using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string text)
        {
            _logger.LogDebug(text);

            string id = Guid.NewGuid().ToString();

            string rankKey = "RANK-" + id;
            string rank = GetRank(text).ToString();

            _storage.Store(rankKey, rank);

            string similarityKey = "SIMILARITY-" + id; //реорганизация принципа подсчета 
            double similarity = GetSimilarity(text);

            _storage.Store(similarityKey, similarity.ToString()); //преобразуем для корректного отображения

            if (similarity == 0) //переработка метода сохранения текста
            {
                string textKey = "TEXT-" + id;
                _storage.Store(textKey, text);
                _storage.StoreKey(textKey);
            }

            return Redirect($"summary?id={id}");
        }
        double GetRank(string text) 
        {
            int lettersCount = text.Count(char.IsLetter);

            return Math.Round(((text.Length - lettersCount) / (double)text.Length), 3);
        }
        double GetSimilarity(string text)
        {
            var keys = _storage.TextSignes();

            foreach (var key in keys)
            {
                if (_storage.Load(key) == text)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
