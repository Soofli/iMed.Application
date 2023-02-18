namespace iMed.App.Views.ItemTemplates;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CourseItemTemplate : ContentView
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

    public CourseItemTemplate()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
    {
        if (TappedCommand != null)
            TappedCommand.Execute(this.BindingContext);
    }
}