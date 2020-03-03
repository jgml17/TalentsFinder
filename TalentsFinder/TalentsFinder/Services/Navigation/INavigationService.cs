using Core.TalentsFinder.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.TalentsFinder.Services.Navigation
{
    public interface INavigationService<TViewModel>

    {
        BaseViewModel<TViewModel> PreviousPageViewModel { get; }

        Task InitializeAsync();

        Task NavigateToAsync<T>() where T : BaseViewModel<T>;

        Task NavigateToAsync<T>(object parameter) where T : BaseViewModel<T>;

        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();

    }
}
