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

            double GetRank(string text)
            {
                int lettersCount = text.Count(char.IsLetter);

                return Math.Round(((text.Length - lettersCount) / (double)text.Length), 3);
            }

            string rankKey = "RANK-" + id;
            string rank = GetRank(text).ToString();

            _storage.Store(rankKey, rank);

            string similarityKey = "SIMILARITY-" + id;
 
            if (_storage.Indeed("TEXT-", text))
            {
                _storage.Store(similarityKey, "1");
            }
            else
            {
                _storage.Store(similarityKey, "0");
            }

            return Redirect($"summary?id={id}");
        }
    }
}
