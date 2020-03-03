using Core.Models.TalentsFinderModels;
using Core.TalentsFinder.Exceptions;
using Core.TalentsFinder.Helpers;
using Core.TalentsFinder.Resources;
using Core.TalentsFinder.Services.IFS;
using Core.TalentsFinder.Services.Navigation;
using Core.TalentsFinder.Services.RequestProvider;
using Core.TalentsFinder.Services.SQLiteProvider;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

// Always remember for DI
using Xamarin.Forms.Xaml;

namespace Core.TalentsFinder.ViewModels
{
    public class MainViewModel : BaseViewModel<MainViewModel>
    {
        #region Variables

        public int IndexSelected;
        private readonly IIFSService _service;
        private SQLiteDbContext _dbSqlite;
        private bool isFirstLoading;
        
        #endregion
       
        #region Properties

        private bool _canActive;
        public bool CanActive
        {
            get { return _canActive; }
            set
            {
                SetProperty(ref _canActive, value);
                OnPropertyChanged("ShowButtonCommand");
            }
        }

        private string _pickerText;
        public string PickerText
        {
            get
            {
                return _pickerText;
            }
            set
            {
                SetProperty(ref _pickerText, value);
            }
        }

        private List<string> _pickerList;
        public List<string> PickerList
        {
            get
            {
                return _pickerList;
            }
            set
            {
                SetProperty(ref _pickerList, value);
            }
        }

        private string _buttonShow;
        public string ButtonShow
        {
            get
            {
                return _buttonShow;
            }
            set
            {
                SetProperty(ref _buttonShow, value);
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        private ObservableCollection<TecModel> _technologies;
        public ObservableCollection<TecModel> Technologies
        {
            get
            {
                return _technologies;
            }
            set
            {
                SetProperty(ref _technologies, value);
            }
        }

        private int _pickerIndex;
        public int PickerIndex
        {
            get
            {
                return _pickerIndex;
            }
            set
            {
                SetProperty(ref _pickerIndex, value);
            }
        }

        private bool _pickerEnabled;
        public bool PickerEnabled
        {
            get
            {
                return _pickerEnabled;
            }
            set
            {
                SetProperty(ref _pickerEnabled, value);
            }
        }

        private CriteriaModel _criteria;
        public CriteriaModel Criteria
        {
            get
            {
                return _criteria;
            }
            set
            {
                SetProperty(ref _criteria, value);
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public MainViewModel(SQLiteDbContext dbSqlite, IIFSService service, ILogger<MainViewModel> logger, INavigationService<MainViewModel> navigationService) : base(logger, navigationService)
        {
            _service = service;
            _logger = logger;
            _navigationService = navigationService;
            _dbSqlite = dbSqlite;

            Title = AppResources.MAIN_VIEW_TITLE;
            Technologies = new ObservableCollection<TecModel>();
            ButtonShow = string.Empty;
            CanActive = false;
            IndexSelected = 0; // Default value -> More then 1 year
            isFirstLoading = true; // Refresh when app starts only
            
            var pic = new List<string> { AppResources.MORE_1_YEAR, AppResources.MORE_5_YEAR, AppResources.MORE_10_YEAR };
            PickerList = pic;
            PickerIndex = 0;
            PickerText = AppResources.PICKER_TEXT;
        }

        public async Task Init()
        {
            // Update Data Technologies
            if (isFirstLoading)
            {
                await GetTalents();

                await GetTechnologies();
                isFirstLoading = false;
            }

            // Fill Picker
            await FillExperiencePicker();
        }

        private async Task FillExperiencePicker()
        {
            IsBusy = true;
            ShowLoading(AppResources.SETTING);

            try
            {
                // Verify if some technology was chosen previously and get the last one saved (stores all criterias done)
                Criteria = await _dbSqlite.Criteria.Include(x => x.TecNamesModel).LastOrDefaultAsync();

                if (Criteria != null)
                {
                    // Active Picker
                    PickerEnabled = true;
                    PickerIndex = Criteria.YearsOfExperienceIndex;

                    // Set Techs chosen
                    var techs = Criteria.TecNamesModel ?? new List<TecNamesModel>();
                    foreach (var item in techs)
                    {
                        Technologies.Where(x => x.Name == item.TecName).FirstOrDefault().ItemSelected = true;
                    }

                    // Active button
                    CanActive = true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "MainViewModel", "FillExperiencePicker");
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        /// <summary>
        /// Get Technologies Data from API
        /// </summary>
        /// <returns></returns>
        public async Task GetTechnologies()
        {
            IsBusy = true;
            ShowLoading(AppResources.LOADING_TECH);

            try
            {
                // Call api
                var task = await _service.GetTechnologies();
                // Technologies
                Technologies = new ObservableCollection<TecModel>(task.OrderBy(x => x.Name));
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "MainViewModel", "GetTechnologies");
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        /// <summary>
        /// Get Talents Data from API
        /// </summary>
        /// <returns></returns>
        public async Task GetTalents()
        {
            IsBusy = true;
            ShowLoading(AppResources.LOADING_TALENTS);

            try
            {
                // Call api
                List<TalentModel> apiTalents = await _service.GetTalents();

                // Saves in database
                var dbTalents = _dbSqlite.Talents.ToList();

                if (dbTalents.Count > 0)
                {
                    // Delete all
                    foreach (var item in dbTalents)
                    {
                        _dbSqlite.Talents.Remove(item);
                    }
                    await _dbSqlite.SaveChangesAsync();

                    // Saves new ones
                    foreach (var item in apiTalents)
                    {
                        _dbSqlite.Talents.Add(item);
                    }
                    await _dbSqlite.SaveChangesAsync();
                }
                else
                {
                    // Save for first time
                    foreach (var item in apiTalents)
                    {
                        _dbSqlite.Talents.Add(item);
                    }
                    await _dbSqlite.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "MainViewModel", "GetTalents");
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        /// <summary>
        /// Get years by picker index
        /// </summary>
        /// <param name="pickerIndex"></param>
        /// <returns></returns>
        private int YearsByIndex(int pickerIndex)
        {
            int ret = 1;

            switch (pickerIndex)
            {
                case 1:
                    ret = 5;
                    break;

                case 2:
                    ret = 10;
                    break;
            }

            return ret;
        }

        #region Commands

        /// <summary>
        /// Show Talents
        /// </summary>
        public Command ShowButtonCommand => new Command(async() =>
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                ShowLoading(AppResources.SETTING);

                // Saves Criteria Search
                Criteria = new CriteriaModel();
                Criteria.TecNamesModel = new List<TecNamesModel>();

                Criteria.YearsOfExperienceIndex = PickerIndex;
                Criteria.YearsOfExperience = YearsByIndex(PickerIndex);

                var tec = Technologies.Where(x => x.ItemSelected == true).Select(c => c.Name).ToList();
                foreach (var item in tec)
                {
                    Criteria.TecNamesModel.Add(new TecNamesModel { TecName = item });
                }

                _dbSqlite.Criteria.Add(Criteria);
                await _dbSqlite.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "MainViewModel", "ShowButtonCommand");
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }

            // Call Talents View
            await _navigationService.NavigateToAsync<TalentsViewModel>(Criteria);

        }, () => CanActive);

        /// <summary>
        /// Technology tapped
        /// </summary>
        public Command ListItemTappedCommand => new Command<TecModel>(technology =>
        {
            try
            {
                if (Technologies.Where(x=> x.Name == technology.Name).FirstOrDefault().ItemSelected)
                {
                    // Uncheck
                    Technologies.Where(x => x.Name == technology.Name).FirstOrDefault().ItemSelected = false;

                }
                else
                {
                    // Check
                    Technologies.Where(x => x.Name == technology.Name).FirstOrDefault().ItemSelected = true;
                }

                // Active button and Picker
                PickerEnabled = true;
                CanActive = true;
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "MainViewModel", "ListItemTappedCommand");
            }
        });

        #endregion
    }
}
