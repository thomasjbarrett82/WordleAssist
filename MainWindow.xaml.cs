using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WordleAssist {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private List<string> ListOfWords = new();

        public MainWindow() {
            InitializeComponent();

            var jsonString = WordList.Words;
            ListOfWords = JsonSerializer.Deserialize<List<string>>(jsonString);
            if (ListOfWords == null)
                Environment.Exit(0);

            foreach (var word in ListOfWords) {
                ListBoxForWords.Items.Add(word);
            }
        }

        private void TextBoxChangedEventHandler(object sender, TextChangedEventArgs args) {
            // validate textbox contents
            if (IsTextInvalid()) {
                MessageBox.Show("Text is invalid");
                return;
            }

            // clear listbox
            ListBoxForWords.Items.Clear();

            // filter ListOfWords
            // exclude gray letters
            char[] grayLetters = Gray.Text.ToCharArray();
            var validWords = ListOfWords.Where(w => !grayLetters.Any(w.Contains)).ToList();

            // filter only green letters
            var greenLetters = new List<string> { 
                Green1.Text,
                Green2.Text,
                Green3.Text,
                Green4.Text,
                Green5.Text
            };
            foreach (var letter in greenLetters) {
                if (string.IsNullOrWhiteSpace(letter.ToString()))
                    continue;
                var index = greenLetters.IndexOf(letter);
                validWords = validWords.Where(w => w[index] == char.Parse(letter)).ToList();
            }

            // filter down to yellow letters, but not in the location
            List<char[]> yellowFields = new List<char[]> {
                Yellow1.Text.ToCharArray(),
                Yellow2.Text.ToCharArray(),
                Yellow3.Text.ToCharArray(),
                Yellow4.Text.ToCharArray(),
                Yellow5.Text.ToCharArray(),
            };
            foreach (var field in yellowFields) {
                var index = yellowFields.IndexOf(field);
                foreach (var letter in field) {
                    if (string.IsNullOrWhiteSpace(letter.ToString()))
                        continue;
                    validWords = validWords.Where(w => w.Contains(letter) && w[index] != letter).ToList();
                }
            }

            // add results to listBox
            foreach (var word in validWords) {
                ListBoxForWords.Items.Add(word);
            }
        }

        private bool IsTextInvalid() {
            // verify only 1 letter in green boxes
            if (IsGreenTextInvalid(Green1.Text))
                return true;
            if (IsGreenTextInvalid(Green2.Text))
                return true;
            if (IsGreenTextInvalid(Green3.Text))
                return true;
            if (IsGreenTextInvalid(Green4.Text))
                return true;
            if (IsGreenTextInvalid(Green5.Text))
                return true;

            // verify only letters in yellow and gray boxes
            if (IsYellowOrGrayTextInvalid(Yellow1.Text))
                return true;
            if (IsYellowOrGrayTextInvalid(Yellow2.Text))
                return true;
            if (IsYellowOrGrayTextInvalid(Yellow3.Text))
                return true;
            if (IsYellowOrGrayTextInvalid(Yellow4.Text))
                return true;
            if (IsYellowOrGrayTextInvalid(Yellow5.Text))
                return true;

            if (IsYellowOrGrayTextInvalid(Gray.Text))
                return true;

            return false;
        }

        private bool IsGreenTextInvalid(string text) {
            return !string.IsNullOrWhiteSpace(text) && 
                (text.Length > 1 || !Regex.IsMatch(text, @"^[a-zA-Z]+$"));
        }

        private bool IsYellowOrGrayTextInvalid(string text) {
            return !string.IsNullOrWhiteSpace(text) && !Regex.IsMatch(text, @"^[a-zA-Z]+$");
        }
    }
}
