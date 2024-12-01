using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Mastermind_PE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] generatedCode; // De willekeurig gegenereerde code
        private int attempts = 0;
        private DispatcherTimer timer;
        DateTime clicked;
        TimeSpan elapsedTime;

        public MainWindow()
        {
            InitializeComponent();
            GenerateRandomCode();
            OpvullenComboBoxes();
            stopcountdown();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += startcountdown;
        }
        private void startcountdown(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - clicked;
            timerTextBox.Text = $"{elapsedTime.Seconds.ToString()} ";
        }







        private void GenerateRandomCode()
        {

            Random random = new Random();
            string[] Colors = { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };
            generatedCode = Enumerable.Range(0, 4).Select(_ => Colors[random.Next(Colors.Length)]).ToArray();
            this.Title = $"MasterMind ({string.Join(",", generatedCode)}), Poging: "; // Toon de code in de titel voor debugging
        }

        private void OpvullenComboBoxes()
        {
            string[] Colors = { "Rood", "Geel", "Oranje", "Wit", "Groen", "Blauw" };

            ComboBox1.ItemsSource = Colors;
            ComboBox2.ItemsSource = Colors;
            ComboBox3.ItemsSource = Colors;
            ComboBox4.ItemsSource = Colors;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Label1.Background = GetBrushFromColorName(ComboBox1.SelectedItem as string);
            Label2.Background = GetBrushFromColorName(ComboBox2.SelectedItem as string);
            Label3.Background = GetBrushFromColorName(ComboBox3.SelectedItem as string);
            Label4.Background = GetBrushFromColorName(ComboBox4.SelectedItem as string);
        }


        private SolidColorBrush GetBrushFromColorName(string colorName)
        {
            switch (colorName)
            {
                case "Rood": return Brushes.Red;
                case "Geel": return Brushes.Yellow;
                case "Oranje": return Brushes.Orange;
                case "Wit": return Brushes.White;
                case "Groen": return Brushes.Green;
                case "Blauw": return Brushes.Blue;
                default: return Brushes.Transparent;
            }
        }

        private void CheckCode_Click(object sender, RoutedEventArgs e)
        {

            attempts++;
            this.Title = $"MasterMind ({string.Join(",", generatedCode)}), Poging: " + attempts;

            timer.Start();
            clicked = DateTime.Now;

            if (attempts >= 10)
            {

                timer.Stop();
                Close();
            }









            // Haal de geselecteerde kleuren op
            string[] userCode = {
            ComboBox1.SelectedItem as string,
            ComboBox2.SelectedItem as string,
            ComboBox3.SelectedItem as string,
            ComboBox4.SelectedItem as string
        };

            // Controleer elke invoer en stel de randkleur in
            CheckColor(Label1, userCode[0], 0);
            CheckColor(Label2, userCode[1], 1);
            CheckColor(Label3, userCode[2], 2);
            CheckColor(Label4, userCode[3], 3);
        }

        private void CheckColor(System.Windows.Controls.Label label, string selectedColor, int position)
        {
            if (selectedColor == generatedCode[position])
            {
                label.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                label.BorderThickness = new Thickness(3);
            }
            else if (generatedCode.Contains(selectedColor))
            {
                label.BorderBrush = new SolidColorBrush(Colors.Wheat);
                label.BorderThickness = new Thickness(3);
            }
            else
            {
                label.BorderBrush = Brushes.Transparent;
                label.BorderThickness = new Thickness(0);
            }
        }
        private void stopcountdown()
        {

            double time = elapsedTime.Seconds;
            if (time >= 10)
            {
                timer.Stop();
                attempts++;
            }

        }

    }
}
