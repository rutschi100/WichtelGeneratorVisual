using System;
using RutschiSwiss.Helpers.WPF.Services;
using WichtelGenerator.WPF.ViewModels;

namespace WichtelGenerator.WPF.Services
{
    public interface IViewLocatorService
    {
        /// <summary>
        ///     Builds the view matching the given view model type.
        ///     Builds the view model and bind it to the created view.
        ///     Loads the view model.
        /// </summary>
        /// <typeparam name="TViewModel">
        ///     The view model type.
        /// </typeparam>
        /// <returns>
        /// </returns>
        IBindableView GetViewFor<TViewModel>()
            where TViewModel : BaseViewModel;

        /// <summary>
        ///     Builds the view matching the given [view model type + transition].
        ///     Binds the view model instance to the created view.
        /// </summary>
        /// <example>
        ///     FullAutoPatientAssayScreenVm + NavigationTransition.ToResult =&gt; creates a PatientAssayResultView
        /// </example>
        /// <typeparam name="TViewModel">
        ///     The view model type.
        /// </typeparam>
        /// <param name="viewModel">
        ///     The view model to be bound to the created view.
        /// </param>
        /// <remarks>
        ///     The service regards the view model as already loaded.
        /// </remarks>
        /// <returns>
        /// </returns>
        IBindableView GetViewFor<TViewModel>(TViewModel viewModel)
            where TViewModel : BaseViewModel;

        /// <summary>
        ///     Gets the view type matching the given view model.
        /// </summary>
        /// <typeparam name="TViewModel">
        ///     The view model type.
        /// </typeparam>
        /// <returns>
        /// </returns>
        Type GetViewTypeFor<TViewModel>()
            where TViewModel : BaseViewModel;

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
        Type GetViewTypeFor<TViewModel>(TViewModel viewModel)
            where TViewModel : BaseViewModel;
    }
}