using Core.TalentsFinder.Exceptions;
using Core.TalentsFinder.Services.Navigation;
using Core.TalentsFinder.Services.RequestProvider;
using Core.TalentsFinder.Services.SQLiteProvider;
using Core.TalentsFinder.Views.Popups;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Core.TalentsFinder.ViewModels
{
    public class BaseViewModel<T> : INotifyPropertyChanged
    {
        protected ILogger<T> _logger;
        protected INavigationService<T> _navigationService;
        protected LoadingView _loadingView;

        #region Properties

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        bool isTalentsBusy = false;
        public bool IsTalentsBusy
        {
            get { return isTalentsBusy; }
            set { SetProperty(ref isTalentsBusy, value); }
        }

        bool isTecBusy = false;
        public bool IsTecBusy
        {
            get { return isTecBusy; }
            set { SetProperty(ref isTecBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #endregion

        public BaseViewModel(ILogger<T> logger, INavigationService<T> navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }

        public virtual void Init(object navigationData)
        {

        }

        public virtual Task InitAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        #region Error

        protected void ErrorHandle(Exception ex, string className, string methodName)
        {
            var Properties = new Dictionary<string, string> { { className, methodName } };

            // Access
            if (ex is ServiceAuthenticationException)
            {
                // Call Login view if it exists !!! ===========
                // TODO
                // ============================================

                // May or not log this ===================
                Properties.Add("Content", (ex as ServiceAuthenticationException).Content);
                Analytics.TrackEvent("ServiceAuthentication", Properties);
                _logger.LogInformation($"##### INFO ===> {(ex as ServiceAuthenticationException).Content} ######");
                // =================================

                return;
            }

            // Requests
            if (ex is HttpRequestExceptionEx)
            {
                Properties.Add("HttpCode", (ex as HttpRequestExceptionEx).HttpCode.ToString());
                Properties.Add("Message", ex.Message);
                Crashes.TrackError(ex, Properties);
                _logger.LogError($"##### ERROR {(ex as HttpRequestExceptionEx).HttpCode.ToString()} => {(ex as HttpRequestExceptionEx).Message} ######");
                return;
            }

            // Deafult
            Properties.Add("Message", ex.Message);
            Crashes.TrackError(ex, Properties);
            _logger.LogError($"##### ERROR => {ex.Message} ######");
        }



        #endregion
                
        #region Loading

        protected void ShowLoading(string LoadMessage)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                _loadingView = new LoadingView(LoadMessage);
                await Application.Current.MainPage.Navigation.PushPopupAsync(_loadingView);
            });
        }

        protected void HideLoading()
        {
            Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.Navigation.RemovePopupPageAsync(_loadingView));
        }

        #endregion

        #region INotifyPropertyChanged

        protected bool SetProperty<Tvm>(ref Tvm backingStore, Tvm value, [CallerMemberName]string propertyName = "",  Action onChanged = null)
        {
            if (EqualityComparer<Tvm>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion
    }
}
