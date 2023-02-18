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
    public partial class FlashCardCategoryItemTemplate : ContentView
    {
        public static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(TappedCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(CourseItemTemplate),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }

        public FlashCardCategoryItemTemplate()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if (TappedCommand != null)
                TappedCommand.Execute(this.BindingContext);
        }
    }
}