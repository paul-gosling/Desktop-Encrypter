using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Encrypter.Ciphers;
using Microsoft.Win32;

namespace Encrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button currentActiveTab;
        private bool isEncrypting;
        private bool isFileLoaded;

        private string fileName;
        private string textFromFile;

        private Caesar caesar;

        public MainWindow()
        {
            InitializeComponent();
            currentActiveTab = caesarTab;
            encryptButton.IsChecked = true;
            alphabetInput.Text = "abcdefghijklmnopqrstuvwxyz";
            caesar = new Caesar(alphabetInput.Text);
        }

        public void TabButton_Click(object sender, RoutedEventArgs e)
        {
            ResetUI();

            var converter = new BrushConverter();
            currentActiveTab.Background = (Brush)converter.ConvertFrom("#29292B");
            currentActiveTab.Foreground = Brushes.Gray;

            currentActiveTab.BorderThickness = GetTabButtonThickness(currentActiveTab.Name, false);

            if (sender is Button button)
            {
                button.Background = (Brush)converter.ConvertFrom("#434347");
                button.Foreground = Brushes.White;
                currentActiveTab = button;

                button.BorderThickness = GetTabButtonThickness(button.Name, true);
                alphabetLabel.Visibility = GetAlphabetVisibility(button.Name);
                alphabetInput.Visibility = GetAlphabetVisibility(button.Name);

                switch (button.Name)
                {
                    case "caesarTab":
                        cipderTitle.Text = "Caesar";
                        break;
                    case "vigenereTab":
                        cipderTitle.Text = "Vigenère";
                        break;
                    case "xorTab":
                        cipderTitle.Text = "XOR";
                        break;
                }
            }
        }

        private void ResetUI()
        {
            encryptButton.IsChecked = true;
            alphabetInput.Text = "abcdefghijklmnopqrstuvwxyz";
            keyInput.Text = "";
            textOutput.Text = "";
        }

        private Thickness GetTabButtonThickness(string buttonName, bool isGettingCurrent)
        {
            switch (buttonName)
            {
                case "caesarTab":
                    return isGettingCurrent ? new Thickness(0) : new Thickness(0, 0, 0, 1);
                case "vigenereTab":
                case "xorTab":
                    return isGettingCurrent ? new Thickness(1, 0, 0, 0) : new Thickness(1, 0, 0, 1);
            }
            return new Thickness(0);
        }

        private Visibility GetAlphabetVisibility(string buttonName)
        {
            switch (buttonName)
            {
                case "caesarTab":
                case "vigenereTab":
                    return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public void EncryptButton_Checked(object sender, RoutedEventArgs e)
        {
            decryptLED.Source = new BitmapImage(new Uri("pack://application:,,,/Encrypter;component/Pictures/black-led.png"));
            encryptLED.Source = new BitmapImage(new Uri("pack://application:,,,/Encrypter;component/Pictures/green-led.png"));
            mainButton.Content = "Encrypt";
            isEncrypting = true;

            //MessageBox.Show($"isEncrypting = {isEncrypting}");
        }

        public void DecryptButton_Checked(object sender, RoutedEventArgs e)
        {
            encryptLED.Source = new BitmapImage(new Uri("pack://application:,,,/Encrypter;component/Pictures/black-led.png"));
            decryptLED.Source = new BitmapImage(new Uri("pack://application:,,,/Encrypter;component/Pictures/green-led.png"));
            mainButton.Content = "Decrypt";
            isEncrypting = false;

            //MessageBox.Show($"isEncrypting = {isEncrypting}");
        }

        public void KeyInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentActiveTab.Name == "caesarTab" && sender is TextBox keyInput)
            {
                if (keyInput.Text.Length > 1)
                {
                    keyInput.Text = keyInput.Text[0].ToString();
                }
            }
        }

        public void MainButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(keyInput.Text))
                return;

            var textToWorkWith = isFileLoaded ? textFromFile : textInput.Text;

            switch (currentActiveTab.Name)
            {
                case "caesarTab":
                    if (caesar.Alphabet != alphabetInput.Text)
                        caesar = new Caesar(alphabetInput.Text);

                    textOutput.Text = isEncrypting
                        ? caesar.Encrypt(textToWorkWith, keyInput.Text[0])
                        : caesar.Decrypt(textToWorkWith, keyInput.Text[0]);
                    break;
            }
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (isFileLoaded)
            {
                isFileLoaded = false;
                textFromFile = ""; 
                textInput.IsReadOnly = false;
            }

            textInput.Text = "";
        }

        public void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                isFileLoaded = true;
                fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                textInput.Text = fileName;
                textInput.IsReadOnly = true;

                fileName = fileName.Substring(0, fileName.Length - 4);

                textFromFile = File.ReadAllText(openFileDialog.FileName);
            }
        }

        public void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";

            var saveFileName = isEncrypting ? fileName + "(encrypted)" : fileName + "(decrypted)";
            saveFileDialog.FileName = saveFileName;

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, textOutput.Text);
            }
        }

        public void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textOutput.Text);
        }
    }
}