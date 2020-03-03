using Core.Models.TalentsFinderModels;
using Core.TalentsFinder.Helpers;
using Core.TalentsFinder.Resources;
using Core.TalentsFinder.ViewModels;
using Core.TalentsFinder.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.TalentsFinder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        private MainViewModel _viewmodel;

        public MainView(MainViewModel viewmodel)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);

            BindingContext = _viewmodel = viewmodel;
        }

        protected  override void OnAppearing()
        {
            base.OnAppearing();

            // Init
            _ = _viewmodel.Init();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}