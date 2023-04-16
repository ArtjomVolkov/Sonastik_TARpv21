using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Sonastik_TARpv21
{
    public partial class MainPage : ContentPage
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        public MainPage()
        {
            InitializeComponent();

            // загрузка слов из Preferences
            if (Preferences.ContainsKey("Dictionary"))
            {
                string dictStr = Preferences.Get("Dictionary", "");
                dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictStr);
            }
        }

        // обработчик события для кнопки добавления слова
        private async void Insert_Dc(object sender, EventArgs e)
        {
            string englishWord = await DisplayPromptAsync("Добавить слово", "Введите слово на английском языке");
            if (englishWord == " ")
            {
                await DisplayAlert("Ошибка", "Нельзя так! ", "ОК");
            }
            else if(englishWord == "")
            {
                await DisplayAlert("Ошибка", "Нельзя так! ", "ОК");
            }
            else if (englishWord != null)
            {
                string russianWord = await DisplayPromptAsync("Добавить слово", "Введите слово на русском языке");
                if (russianWord != null)
                {
                    if (!dict.ContainsKey(englishWord))
                    {
                        dict.Add(englishWord, russianWord);
                        await DisplayAlert("Успех", $"Слово {englishWord} добавлено в словарь", "ОК");
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", $"Слово {englishWord} уже есть в словаре", "ОК");
                    }
                }
            }
        }

        // обработчик события для кнопки просмотра словаря
        private async void Start_Dc(object sender, EventArgs e)
        {
            if (dict.Count > 0)
            {
                CarouselPage carouselPage = new CarouselPage();
                foreach (string key in dict.Keys)
                {
                    ContentPage contentPage = new ContentPage();
                    Label label = new Label();
                    label.Text = key;
                    label.FontSize = 40;
                    label.HorizontalOptions = LayoutOptions.Center;
                    label.VerticalOptions = LayoutOptions.Center;
                    label.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            string currentText = label.Text;
                            if (dict.ContainsKey(currentText))
                            {
                                label.Text = dict[currentText];
                            }
                            else
                            {
                                var reversedDict = dict.ToDictionary(x => x.Value, x => x.Key);
                                if (reversedDict.ContainsKey(currentText))
                                {
                                    label.Text = reversedDict[currentText];
                                }
                            }
                        })
                    });
                    contentPage.Content = label;
                    carouselPage.Children.Add(contentPage);
                }
                carouselPage.Title = "Dictionary";
                await Navigation.PushAsync(carouselPage);
            }
            else
            {
                await DisplayAlert("Словарь пуст", "Добавьте слова в словарь", "ОК");
            }
        }

        // обработчик события для кнопки удаления слова
        private async void Delete_Dc(object sender, EventArgs e)
        {
            string englishWord = await DisplayPromptAsync("Удалить слово", "Введите слово на английском языке");
            if (englishWord != null)
            {
                if (dict.ContainsKey(englishWord))
                {
                    dict.Remove(englishWord);
                    await DisplayAlert("Успех", $"Слово {englishWord} удалено из словаря", "ОК");
                }
                else
                {
                    await DisplayAlert("Ошибка", $"Слово {englishWord} не найдено в словаре", "ОК");
                }
            }
        }
    }
}
