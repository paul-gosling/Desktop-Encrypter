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

namespace Encrypter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button currentActiveTab;
        private bool encrypt;

        public MainWindow()
        {
            InitializeComponent();
            currentActiveTab = caesarTab;
            encryptButton.IsChecked = true;

            System.Diagnostics.Debug.WriteLine("Hello");
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
            encrypt = true;

            //MessageBox.Show($"encrypt = {encrypt}");
        }

        public void DecryptButton_Checked(object sender, RoutedEventArgs e)
        {
            encryptLED.Source = new BitmapImage(new Uri("pack://application:,,,/Encrypter;component/Pictures/black-led.png"));
            decryptLED.Source = new BitmapImage(new Uri("pack://application:,,,/Encrypter;component/Pictures/green-led.png"));
            mainButton.Content = "Decrypt";
            encrypt = false;

            //MessageBox.Show($"encrypt = {encrypt}");
        }
    }
}