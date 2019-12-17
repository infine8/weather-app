using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WeatherApp.WpfApplication.CityService;
using WeatherApp.WpfApplication.WeatherService;

namespace WeatherApp.WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsLoading
        {
            set => Loading.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            get => Loading.Visibility == Visibility.Visible;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            await InitComponents();
        }


        private async Task InitComponents()
        {
            try
            {
                IsLoading = true;

                DpWeaterDate.SelectedDate = DateTime.Today;

                using (var service = new CityServiceClient())
                {
                    CbCity.ItemsSource = (await service.GetCityListAsync()).OrderBy(x => x.Name);
                }

                IsLoading = false;

            }
            catch (Exception e)
            {
                ShowError(e);
            }
        }

        private async void CbCity_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateWeatherData();
        }

        private async void DpWeaterDate_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateWeatherData();
        }

        private async Task UpdateWeatherData()
        {
            try
            {
                if (IsLoading) return;

                var city = CbCity.SelectedItem as CityViewModel;
                var date = DpWeaterDate.SelectedDate;

                if (city == null) throw new Exception("Не выбран город");
                if (date == null) throw new Exception("Не выбрана дата");

                using (var service = new WeatherServiceClient())
                {
                    IsLoading = true;
                    
                    var data = await service.GetWeatherDataAsync(city.Id, date.Value);

                    IsLoading = false;

                    if (data == null)
                    {
                        ShowError("Для указанных параметров данных нет.");
                        return;
                    }

                    LblMaxTemp.Content = $"{data.MaxTemp}℃";
                    LblMinTemp.Content = $"{data.MinTemp}℃";
                }
            }
            catch (Exception e)
            {
                ShowError(e);
            }
        }


        private void ShowError(string error) =>
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

        private void ShowError(Exception ex) =>
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
