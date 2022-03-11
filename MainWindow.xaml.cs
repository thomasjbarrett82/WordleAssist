using System;
using System.Collections.Generic;
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
#pragma warning disable CS8601 // Possible null reference assignment.
            ListOfWords = JsonSerializer.Deserialize<List<string>>(jsonString);
#pragma warning restore CS8601 // Possible null reference assignment.
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

            char[] grayLetters = Gray.Text.ToLower().ToCharArray();
            var greenLetters = new List<string> {
                Green1.Text.ToLower(),
                Green2.Text.ToLower(),
                Green3.Text.ToLower(),
                Green4.Text.ToLower(),
                Green5.Text.ToLower()
            };
            List<char[]> yellowFields = new List<char[]> {
                Yellow1.Text.ToLower().ToCharArray(),
                Yellow2.Text.ToLower().ToCharArray(),
                Yellow3.Text.ToLower().ToCharArray(),
                Yellow4.Text.ToLower().ToCharArray(),
                Yellow5.Text.ToLower().ToCharArray(),
            };

            var validWords = WordFilter.GetValidWords(ListOfWords, grayLetters, yellowFields, greenLetters);

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
