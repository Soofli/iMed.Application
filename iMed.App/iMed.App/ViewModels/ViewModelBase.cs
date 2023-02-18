namespace iMed.App.ViewModels;
public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
{
    protected INavigationService NavigationService { get; private set; }
    public IRestWrapper RestWrapper { get; private set; }
    public ICommand BackNavigationCommand { get; set; }

    private string _title;
    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    public ViewModelBase(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
    public ViewModelBase(INavigationService navigationService, IRestWrapper restWrapper)
    {
        NavigationService = navigationService;
        RestWrapper = restWrapper;
    }

    public virtual void InitializeCommand()
    {
        BackNavigationCommand = new DelegateCommand(() => { NavigationService.GoBackAsync(); });
    }
    public virtual void Initialize(INavigationParameters parameters)
    {
        InitializeCommand();
    }

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {

    }

    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {

    }

    public virtual void Destroy()
    {

    }
}

public class ViewModelBase<TPageDto> : BindableBase, IInitialize, INavigationAware, IDestructible
{
    protected INavigationService NavigationService { get; private set; }
    public IRestWrapper RestWrapper { get; private set; }
    public ICommand BackNavigationCommand { get; set; }
    public TPageDto PageDto { get; set; } = Activator.CreateInstance<TPageDto>();
    private string _title;
    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    public ViewModelBase()
    {

    }
    public ViewModelBase(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
    public ViewModelBase(INavigationService navigationService, IRestWrapper restWrapper)
    {
        NavigationService = navigationService;
        RestWrapper = restWrapper;
    }

    public virtual void InitializeCommand()
    {
        BackNavigationCommand = new DelegateCommand(() => { NavigationService.GoBackAsync(); });
    }
    public virtual void Initialize(INavigationParameters parameters)
    {
        InitializeCommand();
    }

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {

    }

    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {

    }

    public virtual void Destroy()
    {

    }
}

