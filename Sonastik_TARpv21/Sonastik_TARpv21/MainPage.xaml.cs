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
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        public MainPage()
        {
            InitializeComponent();

            // Загрузка слов из Preferences, если они там есть
            if (Preferences.ContainsKey("Dictionary"))
            {
                string dictStr = Preferences.Get("Dictionary", "");
                dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(dictStr);
            }
        }

        // добавления слова
        private async void Insert_Dc(object sender, EventArgs e)
        {
            string eng = await DisplayPromptAsync("Lisa sõna", "Sisestage sõna inglise keeles");
            if (eng == " ")
            {
                await DisplayAlert("Viga", "Sa ei saa seda teha!", "OK");
            }
            else if(eng == "")
            {
                await DisplayAlert("Viga", "Sa ei saa seda teha!", "OK");
            }
            else if (eng != null)
            {
                string est = await DisplayPromptAsync("Lisa sõna", "Sisesta sõna eesti keeles");
                if (est != null)
                {
                    // Проверка, что англ слово еще не добавлено в словарь
                    if (!dictionary.ContainsKey(eng))
                    {
                        dictionary.Add(eng, est);
                        await DisplayAlert("Hästi", $"Sõna {eng} on sõnastikku lisatud", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Viga", $"Sõna {eng} on juba sõnastikus", "OK");
                    }
                }
            }
        }

        // просмотр словаря
        private async void Start_Dc(object sender, EventArgs e)
        {
            if (dictionary.Count > 0)
            {
                CarouselPage carousel = new CarouselPage();
                foreach (string key in dictionary.Keys)
                {
                    ContentPage content = new ContentPage();
                    Label lbl = new Label();
                    lbl.Text = key;
                    lbl.FontSize = 50;
                    lbl.HorizontalOptions = LayoutOptions.Center;
                    lbl.VerticalOptions = LayoutOptions.Center;
                    lbl.GestureRecognizers.Add(new TapGestureRecognizer
                    {   
                        Command = new Command(() =>
                        {
                            string txt = lbl.Text;
                            if (dictionary.ContainsKey(txt))
                            {
                                lbl.Text = dictionary[txt];
                            }
                            else
                            {
                                var back = dictionary.ToDictionary(x => x.Value, x => x.Key);
                                if (back.ContainsKey(txt))
                                {
                                    lbl.Text = back[txt];
                                }
                            }
                        })
                    });
                    content.Content = lbl;
                    carousel.Children.Add(content);
                }
                await Navigation.PushAsync(carousel);
            }
            else
            {
                await DisplayAlert("Sõnastik on tühi", "Lisa sõnu sõnastikku", "OK");
            }
        }

        // удаления слова
        private async void Delete_Dc(object sender, EventArgs e)
        {
            string eng = await DisplayPromptAsync("Kustuta sõna", "Sisestage sõna inglise keeles");
            if (eng != null)
            {
                if (dictionary.ContainsKey(eng))
                {
                    dictionary.Remove(eng);
                    await DisplayAlert("Hästi", $"Sõna {eng} eemaldati sõnastikust", "OK");
                }
                else
                {
                    await DisplayAlert("Viga", $"Sõna {eng} ei leitud sõnastikust", "ОК");
                }   
            }
        }
    }
}
