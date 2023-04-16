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

        // добавления слова
        private async void Insert_Dc(object sender, EventArgs e)
        {
            string eng = await DisplayPromptAsync("Добавить слово", "Введите слово на английском языке");
            if (eng == " ")
            {
                await DisplayAlert("Ошибка", "Нельзя так! ", "ОК");
            }
            else if(eng == "")
            {
                await DisplayAlert("Ошибка", "Нельзя так! ", "ОК");
            }
            else if (eng != null)
            {
                string rus = await DisplayPromptAsync("Добавить слово", "Введите слово на русском языке");
                if (rus != null)
                {
                    if (!dict.ContainsKey(eng))
                    {
                        dict.Add(eng, rus);
                        await DisplayAlert("Успех", $"Слово {eng} добавлено в словарь", "ОК");
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", $"Слово {eng} уже есть в словаре", "ОК");
                    }
                }
            }
        }

        // просмотр словаря
        private async void Start_Dc(object sender, EventArgs e)
        {
            if (dict.Count > 0)
            {
                CarouselPage carousel = new CarouselPage();
                foreach (string key in dict.Keys)
                {
                    ContentPage content = new ContentPage();
                    Label lbl = new Label();
                    lbl.Text = key;
                    lbl.FontSize = 40;
                    lbl.HorizontalOptions = LayoutOptions.Center;
                    lbl.VerticalOptions = LayoutOptions.Center;
                    lbl.GestureRecognizers.Add(new TapGestureRecognizer
                    {   
                        Command = new Command(() =>
                        {
                            string txt = lbl.Text;
                            if (dict.ContainsKey(txt))
                            {
                                lbl.Text = dict[txt];
                            }
                            else
                            {
                                var reverse = dict.ToDictionary(x => x.Value, x => x.Key);
                                if (reverse.ContainsKey(txt))
                                {
                                    lbl.Text = reverse[txt];
                                }
                            }
                        })
                    });
                    content.Content = lbl;
                    carousel.Children.Add(content);
                }
                carousel.Title = "Dictionary";
                await Navigation.PushAsync(carousel);
            }
            else
            {
                await DisplayAlert("Словарь пуст", "Добавьте слова в словарь", "ОК");
            }
        }

        // удаления слова
        private async void Delete_Dc(object sender, EventArgs e)
        {
            string eng = await DisplayPromptAsync("Удалить слово", "Введите слово на английском языке");
            if (eng != null)
            {
                if (dict.ContainsKey(eng))
                {
                    dict.Remove(eng);
                    await DisplayAlert("Успех", $"Слово {eng} удалено из словаря", "ОК");
                }
                else
                {
                    await DisplayAlert("Ошибка", $"Слово {eng} не найдено в словаре", "ОК");
                }   
            }
        }
    }
}
