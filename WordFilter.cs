using System.Collections.Generic;
using System.Linq;

namespace WordleAssist {
    public static class WordFilter {
        public static List<string> GetValidWords(List<string> ListOfWords, char[] grayLetters, List<char[]> yellowFields, List<string> greenLetters) {
            // exclude gray letters
            var validWords = ListOfWords.Where(w => !grayLetters.Any(w.Contains)).ToList();

            // filter for only green letters
            for (var i = 0; i < greenLetters.Count; i++) {
                if (string.IsNullOrWhiteSpace(greenLetters[i].ToString()))
                    continue;
                validWords = validWords.Where(w => w[i] == char.Parse(greenLetters[i])).ToList();
            }

            // filter down to yellow letters, but not in the same location
            for (var i = 0; i < yellowFields.Count; i++) {
                foreach (var letter in yellowFields[i]) {
                    if (string.IsNullOrWhiteSpace(letter.ToString()))
                        continue;
                    validWords = validWords.Where(w => w.Contains(letter) && w[i] != letter).ToList();
                }
            }

            return validWords;
        }
    }
}
