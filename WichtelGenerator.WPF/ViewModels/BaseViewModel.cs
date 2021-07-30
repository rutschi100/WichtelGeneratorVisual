using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using RutschiSwiss.Helpers.WPF.Services;

namespace WichtelGenerator.WPF.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool isBusy;

        /// <summary>
        ///     Is view currently busy or not
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => SetAndRaise(ref isBusy, value);
        }

        /// <summary>
        ///     The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        internal abstract void InitCommands();

        /// <summary>
        ///     The raise property changed.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// </exception>
        public void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Getting property name form expression is not supported for this type.");
            }

            if (!(expression is LambdaExpression lamda))
            {
                throw new NotSupportedException(
                    "Getting property name form expression is not supported for this type.");
            }

            if (lamda.Body is MemberExpression memberExpression)
            {
                RaisePropertyChanged(memberExpression.Member.Name);
                return;
            }

            var unary = lamda.Body as UnaryExpression;
            if (unary?.Operand is MemberExpression member)
            {
                RaisePropertyChanged(member.Member.Name);
                return;
            }

            throw new NotSupportedException("Getting property name form expression is not supported for this type.");
        }

        public Page GetPageValue(IBindableView page)
        {
            return (Page) page;
        }

        /// <summary>
        ///     The raise property changed.
        /// </summary>
        /// <param name="propertyName">
        ///     The property name.
        /// </param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     The set and raise.
        /// </summary>
        /// <param name="property">
        ///     The property.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="propertyName">
        ///     The property name.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected bool SetAndRaise<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, value))
            {
                return false;
            }

            property = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}