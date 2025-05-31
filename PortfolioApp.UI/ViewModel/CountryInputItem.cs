namespace PortfolioApp.UI.ViewModel;

public partial class CountryInputItem : ViewModelBase
{
    public string Country { get; set; }


    [ObservableProperty]
    private double _priceChange;
}
