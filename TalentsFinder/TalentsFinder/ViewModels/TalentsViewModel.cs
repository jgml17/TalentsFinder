using Core.Models.TalentsFinderModels;
using Core.TalentsFinder.Helpers;
using Core.TalentsFinder.Resources;
using Core.TalentsFinder.Services.IFS;
using Core.TalentsFinder.Services.Navigation;
using Core.TalentsFinder.Services.SQLiteProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MLToolkit.Forms.SwipeCardView.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core.TalentsFinder.ViewModels
{
    public class TalentsViewModel : BaseViewModel<TalentsViewModel>
    {
        #region Variables

        private uint _threshold;
        private int ItensCount;
        private List<TalentModel> dbTalents;
        private SQLiteDbContext _dbSqlite;
        private CriteriaModel criteria;
        private readonly IIFSService _service;

        #endregion

        #region Properties

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

        private bool isCardVisible;
        public bool IsCardVisible
        {
            get
            {
                return isCardVisible;
            }
            set
            {
                SetProperty(ref isCardVisible, value);
            }
        }

        private bool isEmptyMessageVisible;
        public bool IsEmptyMessageVisible
        {
            get
            {
                return isEmptyMessageVisible;
            }
            set
            {
                SetProperty(ref isEmptyMessageVisible, value);
            }
        }

        private string emptyMessage;
        public string EmptyMessage
        {
            get
            {
                return emptyMessage;
            }
            set
            {
                SetProperty(ref emptyMessage, value);
            }
        }

        private ObservableCollection<TalentModel> _talents;
        public ObservableCollection<TalentModel> Talents
        {
            get => _talents;
            set
            {
                _talents = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public TalentsViewModel(SQLiteDbContext dbSqlite, IIFSService service, ILogger<TalentsViewModel> logger, INavigationService<TalentsViewModel> navigationService) : base(logger, navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
            _dbSqlite = dbSqlite;
            _service = service;

            Title = AppResources.TALENTS_VIEW_TITLE;
            IsEmptyMessageVisible = true;
            IsCardVisible = false;
            Talents = new ObservableCollection<TalentModel>();

            Threshold = (uint)(App.ScreenWidth / 3);

            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);

            ClearItemsCommand = new Command(OnClearItemsCommand);
            AddItemsCommand = new Command(OnAddItemsCommand);
        }

        public uint Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                RaisePropertyChanged();
            }
        }

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
            TalentModel talent;

            switch (eventArgs.Direction)
            {
                case SwipeCardDirection.None:
                    break;

                case SwipeCardDirection.Right:
                    // Accept and set Properties
                    talent = eventArgs.Item as TalentModel;

                    //  Update Talents Status
                    _dbSqlite.Status.Add(new StatusModel { Seen = true, Accepted = true, TalentId = talent.Id });
                    _dbSqlite.SaveChanges();


                    // Vibration is not supported by UWP
                    if (Device.RuntimePlatform != Device.UWP)
                    {
                        var duration = TimeSpan.FromSeconds(1);
                        Vibration.Vibrate(duration);
                    }

                    ItensCount--;
                    if (ItensCount == 0)
                    {
                        // CardView is done !!!
                        EmptyMessage = AppResources.TALENTS_DONE_MESSAGE;
                        IsEmptyMessageVisible = true;
                        IsCardVisible = false;
                    }
                    Title = $"{AppResources.TALENTS_VIEW_TITLE} - {string.Format(AppResources.TALENTS_VIEW_TITLE_COUNT, ItensCount)}";

                    break;

                case SwipeCardDirection.Left:
                    // Reject and set Properties
                    talent = eventArgs.Item as TalentModel;

                    //  Update Talents Status
                    _dbSqlite.Status.Add(new StatusModel { Seen = true, Accepted = false, TalentId = talent.Id });
                    _dbSqlite.SaveChanges();

                    ItensCount--;
                    if (ItensCount == 0)
                    {
                        // CardView is done !!!
                        EmptyMessage = AppResources.TALENTS_DONE_MESSAGE;
                        IsEmptyMessageVisible = true;
                        IsCardVisible = false;
                    }
                    Title = $"{AppResources.TALENTS_VIEW_TITLE} - {string.Format(AppResources.TALENTS_VIEW_TITLE_COUNT, ItensCount)}";

                    break;


                case SwipeCardDirection.Up:
                    break;
                case SwipeCardDirection.Down:
                    break;
                default:
                    break;
            }
        }

        private void OnClearItemsCommand()
        {
            Talents.Clear();
        }

        private void OnAddItemsCommand()
        {
        }

        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;
                case DraggingCardPosition.UnderThreshold:
                    break;
                case DraggingCardPosition.OverThreshold:
                    break;
                case DraggingCardPosition.FinishedUnderThreshold:
                    return;
                case DraggingCardPosition.FinishedOverThreshold:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Init(object navigationData)
        {
            // Get Talents Saved filtered by Criteria
            criteria = navigationData as CriteriaModel;
            Task.Run(() => LoadTalents());
        }

        // Init - Load Talents
        public void LoadTalents()
        {
            try
            {
                var status = _dbSqlite.Status.ToList();
                dbTalents = new List<TalentModel>();

                foreach (var item in criteria.TecNamesModel)
                {
                    var hasTechChosen = _dbSqlite.Talents.Include(z => z.Name).Include(w => w.Technologies).Where(a => a.Technologies.Any(b => b.Name == item.TecName && b.ExperianceYears >= criteria.YearsOfExperience)).ToList();
                    dbTalents.AddRange(hasTechChosen.Where(a => status.All(b => b.TalentId != a.Id)).Distinct().ToList());
                }

                if (dbTalents != null)
                {
                    ItensCount = dbTalents.Count;

                    if (ItensCount > 0)
                    {
                        IsEmptyMessageVisible = false;
                        IsCardVisible = true;

                        Talents.Clear(); // This is for list refresh works
                        Talents = new ObservableCollection<TalentModel>(dbTalents.OrderBy(x => x.Name.First));

                        _logger.LogInformation($" ################ COUNT: {Talents.Count} TALENTS ##################");

                    }
                    else
                    {
                        // talents is empty
                        EmptyMessage = AppResources.TALENTS_EMPTY_MESSAGE;
                        IsEmptyMessageVisible = true;
                        IsCardVisible = false;
                    }

                }
                else
                {
                    // talents is empty
                    EmptyMessage = AppResources.TALENTS_EMPTY_MESSAGE;
                    IsEmptyMessageVisible = true;
                    IsCardVisible = false;
                }
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "TalentsViewModel", "LoadTalents");
            }
        }

        #region Commands 

        public Command RefreshTalentsCommand => new Command(async () =>
        {
            IsBusy = true;
            ShowLoading(AppResources.UPDATING);

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

                LoadTalents();
            }
            catch (Exception ex)
            {
                ErrorHandle(ex, "TalentsViewModel", "RefreshTalentsCommand");
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        });


        #endregion

    }
}
