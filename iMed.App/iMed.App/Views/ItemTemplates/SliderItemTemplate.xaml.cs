using iMed.Domain.Dtos.SmalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iMed.App.Views.ItemTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SliderItemTemplate : ContentView
    {
        public static readonly BindableProperty TappedCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(TappedCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(SliderItemTemplate),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay
            );

        public ICommand TappedCommand
        {
            get => (ICommand)GetValue(TappedCommandProperty);
            set => SetValue(TappedCommandProperty, value);
        }
        public SliderItemTemplate()
        {
            InitializeComponent();
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == nameof(this.BindingContext))
            {
                if(BindingContext is SpecialOfferSDto special)
                {
                    if (special.SpecialOfferType == SpecialOfferType.Course)
                    {
                        courseCard.IsVisible = true;
                        flashCard.IsVisible = false;
                    }
                    if(special.SpecialOfferType == SpecialOfferType.FlashCard)
                    {
                        courseCard.IsVisible = false;
                        flashCard.IsVisible = true;
                    }
                }
            }
        }
        private void ItemTapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            if(TappedCommand!=null)
                TappedCommand.Execute(this.BindingContext);
        }
    }
}