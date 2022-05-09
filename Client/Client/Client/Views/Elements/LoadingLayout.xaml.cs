using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views.Elements
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingLayout : ContentView
    {
        public LoadingLayout()
        {
            InitializeComponent();
        }
    }
}