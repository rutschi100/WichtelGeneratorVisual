using System;
using System.Collections.Generic;
using RutschiSwiss.Helpers.WPF.Services;
using WichtelGenerator.WPF.Pages;
using WichtelGenerator.WPF.ViewModels;

namespace WichtelGenerator.WPF.Services
{
    /// <summary>
    ///     Vor dem nutzen müssen die ViewModels in ViewLocatorDictionary registriert werden.
    /// </summary>
    public class ViewLocatorService : IViewLocatorService
    {
        /// <summary>
        ///     Registrierung:
        ///     Beispiel: ViewLocatorDictionary.Add(nameof(-ViewMode-), typeof(-Page-));
        /// </summary>
        private static Dictionary<string, Type> ViewLocatorDictionary { get; } = new Dictionary<string, Type>
        {
            {nameof(MainSiteViewModel), typeof(MainSite)},
            {nameof(StorageSettingViewModel), typeof(StorageSettingPage)},
            {nameof(AddUserViewModel), typeof(AddUserPage)},
            {nameof(ManageBlackListViewModel), typeof(ManageBlackListsPage)},
            {nameof(ManageUserViewModel), typeof(ManageUserPage)},
            {nameof(RuffleViewModel), typeof(RufflePage)},
            {nameof(SettingViewModel), typeof(SettingPage)},
            {nameof(WelcomeViewModel), typeof(WelcomePage)}
        };

        //public BaseViewModel GetViewModel<TView>()
        //    where TView : class, IBindableView
        //{
        //    var viewModel = DependencyContainer.Instance.GetInstance<TView>();

        //    return viewModel.DataContext as BaseViewModel;
        //}


        public IBindableView GetViewFor<TViewModel>()
            where TViewModel : BaseViewModel
        {
            var viewModel = DependencyContainer.Instance.GetInstance<TViewModel>();
            var viewType = ViewLocatorDictionary[typeof(TViewModel).Name];
            var viewInstance = DependencyContainer.Instance.GetInstance(viewType);


            IBindableView view;
            try
            {
                view = viewInstance as IBindableView;
                view.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error while binding viewmodel to view. VM='{typeof(TViewModel).Name}' | View='{viewType.Name}'",
                    ex);
            }

            return view;
        }

        public IBindableView GetViewFor<TViewModel>(TViewModel viewModel)
            where TViewModel : BaseViewModel
        {
            var view =
                (IBindableView) DependencyContainer.Instance.GetInstance(
                    ViewLocatorDictionary[$"{viewModel.GetType().Name}"]);
            view.DataContext = viewModel;
            return view;
        }

        /// <summary>
        ///     Gets the view type matching the given view model.
        /// </summary>
        /// <typeparam name="TViewModel">
        ///     The view model type.
        /// </typeparam>
        /// <returns>
        /// </returns>
        public Type GetViewTypeFor<TViewModel>()
            where TViewModel : BaseViewModel
        {
            return ViewLocatorDictionary[typeof(TViewModel).Name];
        }

        /// <summary>
        ///     Gets the view type matching the given view model and transition.
        /// </summary>
        /// <param name="viewModel">
        /// </param>
        /// <typeparam name="TViewModel">
        ///     The view model type.
        /// </typeparam>
        /// <returns>
        /// </returns>
        public Type GetViewTypeFor<TViewModel>(TViewModel viewModel)
            where TViewModel : BaseViewModel
        {
            return ViewLocatorDictionary[$"{viewModel.GetType().Name}"];
        }
    }
}