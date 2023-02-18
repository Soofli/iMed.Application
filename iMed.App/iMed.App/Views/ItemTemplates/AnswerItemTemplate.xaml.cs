using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.ItemTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnswerItemTemplate : ContentView
    {
        public static readonly BindableProperty SelectCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(AnswerItemTemplate),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand SelectCommand
        {
            get { return (ICommand)GetValue(SelectCommandProperty); }
            set { SetValue(SelectCommandProperty, value); }
        }

        public static readonly BindableProperty IsEnabledProperty =
            BindableProperty.Create(
                propertyName: nameof(IsEnabled),
                returnType: typeof(bool),
                declaringType: typeof(AnswerItemTemplate),
                defaultValue: false,
                defaultBindingMode: BindingMode.OneWay,
                null,
                IsEnabledPropertyChanged
            );

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }
        private static void IsEnabledPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var viewModel = bindable as AnswerItemTemplate;
            if (newvalue is bool flag)
            {
                viewModel.mainFrame.IsEnabled = false;
            }
        }


        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                propertyName: nameof(IsSelected),
                returnType: typeof(bool),
                declaringType: typeof(AnswerItemTemplate),
                defaultValue: false,
                defaultBindingMode: BindingMode.OneWay,
                null,
                IsSelectedPropertyChanged
            );

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        private static void IsSelectedPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var viewModel = bindable as AnswerItemTemplate;
            if (newvalue is bool flag)
            {
                if (flag)
                {
                    viewModel.mainFrame.BorderColor = (Color)Application.Current.Resources["AccentColor"];
                    viewModel.iconFrame.BackgroundColor = (Color)Application.Current.Resources["AccentColor"];
                    viewModel.iconLabel.IsVisible = true;
                }
                else
                {
                    viewModel.mainFrame.BorderColor = Color.Transparent;
                    viewModel.iconFrame.BackgroundColor = (Color)Application.Current.Resources["Gray-100"];
                    viewModel.iconLabel.IsVisible = false;
                }
            }
        }

        public static readonly BindableProperty ShowResultProperty =
            BindableProperty.Create(
                propertyName: nameof(ShowResult),
                returnType: typeof(bool),
                declaringType: typeof(AnswerItemTemplate),
                defaultValue: false,
                defaultBindingMode: BindingMode.OneWay,
                null,
                ShowResultPropertyChanged
            );

        public bool ShowResult
        {
            get { return (bool)GetValue(ShowResultProperty); }
            set { SetValue(ShowResultProperty, value); }
        }
        private static void ShowResultPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var viewModel = bindable as AnswerItemTemplate;
            if (newvalue is bool flag && viewModel.BindingContext is FlashCardAnswerSDto answer)
            {
                if (flag)
                {
                    if (answer.IsTrue)
                    {
                        viewModel.mainFrame.BorderColor = Color.FromHex("#08B874");
                        viewModel.iconFrame.BackgroundColor = Color.FromHex("#08B874");
                        viewModel.iconLabel.IsVisible = true;
                        viewModel.iconLabel.Text = MaterialIconFont.CheckBold;
                    }
                    else
                    {
                        viewModel.mainFrame.BorderColor = Color.FromHex("#ff4a4a");
                        viewModel.iconFrame.BackgroundColor = Color.FromHex("#ff4a4a");
                        viewModel.iconLabel.IsVisible = true;
                        viewModel.iconLabel.Text = MaterialIconFont.Close;
                    }
                }
                else
                {
                    if (viewModel.IsSelected)
                    {

                        viewModel.mainFrame.BorderColor = (Color)Application.Current.Resources["AccentColor"];
                        viewModel.iconFrame.BackgroundColor = (Color)Application.Current.Resources["AccentColor"];
                        viewModel.iconLabel.IsVisible = true;
                    }
                    else
                    {

                        viewModel.mainFrame.BorderColor = Color.Transparent;
                        viewModel.iconFrame.BackgroundColor = (Color)Application.Current.Resources["Gray-100"];
                        viewModel.iconLabel.IsVisible = false;
                    }
                }
            }
        }


        public AnswerItemTemplate()
        {
            InitializeComponent();
        }

        private void AnswerTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (BindingContext is FlashCardAnswerSDto answer && IsEnabled == true)
                SelectCommand?.Execute(answer);
        }
    }
}