using LandmarkAI.Classes;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LandmarkAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new ();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = "F:\\Training\\WPF_Udemy\\Resources-REST-and-AI-Training-the-AI\\WPF-1040-TrainAI-resources";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                selectedImage.Source = new BitmapImage(new Uri(fileName));

                MakePredictionAsync(fileName);
            }
        }

        private async void MakePredictionAsync(string fileName)
        {
            string url = "";
            string predictionKey = "";
            string contentType = "application/octet-stream";
            var file = File.ReadAllBytes(fileName);

            using (HttpClient client = new ())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);

                using (ByteArrayContent content = new (file))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var response = await client.PostAsync(url, content);                    
                    var responseString = await response.Content.ReadAsStringAsync();
                    List<Prediction> predictions = (JsonConvert.DeserializeObject<CustomVision>(responseString)).Predictions;
                    predictionsListView.ItemsSource = predictions;
                }
            }
        }
    }
}