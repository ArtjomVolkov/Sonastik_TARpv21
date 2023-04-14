using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sonastik_TARpv21
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void Start_Dc(object sender, EventArgs e)
        {
            nm.Text = "1";
        }

        private void Insert_Dc(object sender, EventArgs e)
        {
            nm.Text = "2";
        }

        private void Delete_Dc(object sender, EventArgs e)
        {
            nm.Text = "3";
        }

        private void Update_Dc(object sender, EventArgs e)
        {
            nm.Text = "4";
        }
    }
}
