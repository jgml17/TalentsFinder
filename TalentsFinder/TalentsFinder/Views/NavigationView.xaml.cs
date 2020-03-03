using Core.TalentsFinder.Resources;
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
    public partial class NavigationView : NavigationPage
    {
        public NavigationView() : base()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, AppResources.BACK);
        }

        public NavigationView(Page root) : base(root)
        {
            InitializeComponent();
        }
    }
}