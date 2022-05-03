namespace WichtelGenerator.WPF.Services
{
    /// <summary>
    ///     Bindable View.
    /// </summary>
    public interface IBindableView
    {
        /// <summary>
        ///     Gets or sets the binding context.
        /// </summary>
        object DataContext { get; set; }
    }
}