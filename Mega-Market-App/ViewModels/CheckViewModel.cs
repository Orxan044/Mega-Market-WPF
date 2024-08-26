using Mega_Market_App.Command;
using Mega_Market_Data.Data;
using Mega_Market_Data.Models.Concretes;
using Mega_Market_Data.Repositoies;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Net;
using ToastNotifications.Messages;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Colors;
using System.Net.Http;
using Mega_Market_App.Services.Mail;

namespace Mega_Market_App.ViewModels;

public class CheckViewModel : BaseViewModel, INotifyPropertyChanged
{
    private History? _history;
    public History History
    {
        get => _history!;
        set { _history = value; OnPropertyChanged(); }
    }

    private DateTime? _Time;

    public DateTime? Time
    {
        get => _Time;
        set { _Time = value; OnPropertyChanged(); }
    }

    private double? _total;

    public double? Total
    {
        get => _total;
        set { _total = value; OnPropertyChanged(); }
    }

    private string? _payMethod;
    public string? PayMethod
    {
        get => _payMethod;
        set { _payMethod = value; OnPropertyChanged(); }
    }


    private ObservableCollection<ProductHistory>? products;
    public ObservableCollection<ProductHistory> Products { get => products!; set { products = value; OnPropertyChanged(); } }

    private IRepository<ProductHistory, UserDbContext> _productHistoryRepository;
    private readonly LoginViewModel _loginViewModel;
    private User _user = new();

    public RelayCommand PdfToMailCommand { get; set; }
    public CheckViewModel(IRepository<ProductHistory, UserDbContext> productHistoryRepository, LoginViewModel loginViewModel)
    {
        _productHistoryRepository = productHistoryRepository;
        _loginViewModel = loginViewModel;

        UserCheck();

        History = new();
        Time = History.Date;
        Total = History.TotalAmount;
        PayMethod = History.PayMethod;
        Products = [];

        PdfToMailCommand = new RelayCommand(PdfToMailClick);
    }

    private void PdfToMailClick(object? obj)
    {
        CreatePdf();
    }

    public void AddCheckHistory(History history)
    {
        History = history;
        Time = history.Date;
        Total = history.TotalAmount;
        PayMethod = history.PayMethod;
        foreach (var productHistory in _productHistoryRepository.GetAll())
            if (productHistory.History == history) Products.Add(productHistory);

    }

    public void CreatePdf()
    {

        string tempPath = Path.GetTempPath();
        string filename = Path.Combine(tempPath, "ProductCheck.pdf");

        using (PdfWriter writer = new PdfWriter(filename))
        {
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                iText.Layout.Document document = new iText.Layout.Document(pdf);
                document.SetMargins(20, 20, 20, 20);

                Paragraph title = new Paragraph("Product Check")
                    .SetFontSize(26)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetBold();
                document.Add(title);

                Paragraph dateParagraph = new Paragraph($"Time: {Time:dd/MM/yyyy HH:mm:ss}")
                    .SetFontSize(15)
                    .SetMarginTop(10);
                document.Add(dateParagraph);

                foreach (var product in Products)
                {
                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2 })).UseAllAvailableWidth();

                    Paragraph productInfo = new Paragraph()
                        .Add(new Text($"{product.Name}, {product.Description}\n").SetFontSize(15))
                        .Add(new Text($"Price: {product.Price} ₼\n").SetFontSize(15))
                        .Add(new Text($"Count: {product.Count} pcs").SetFontSize(15));

                    Cell infoCell = new Cell().Add(productInfo)
                        .SetBorder(Border.NO_BORDER)
                        .SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE);
                    table.AddCell(infoCell);

                    document.Add(table);
                }

                Paragraph payMethodParagraph = new Paragraph($"Pay Method: {PayMethod}")
                    .SetFontSize(15)
                    .SetMarginTop(10);
                document.Add(payMethodParagraph);


                Paragraph totalParagraph = new Paragraph($"Total: {Total} ₼")
                    .SetFontSize(26)
                    .SetFontColor(ColorConstants.BLACK)
                    .SetBold()
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetMarginTop(10);
                document.Add(totalParagraph);

            }
        }

        try
        {
            MailServices.SendMail(
                _user.Mail!,
                "Mega Market Products Check",
                "Please find the attached Product Check PDF.",
                filename
            );

            notifier.ShowSuccess("Email Sent Successfully");
        }
        catch (Exception)
        {
            notifier.ShowError("Something went wrong with the mail. Please try again.");
        }
    }

    void UserCheck()
    {
        foreach (var user in _loginViewModel.UserRepository.GetAll())
        {
            if (_loginViewModel.UserLogin.Mail == user.Mail)
            {
                _user = user;
            }
        }
    }
}






