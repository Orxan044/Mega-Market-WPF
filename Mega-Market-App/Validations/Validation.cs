using System.Text.RegularExpressions;

namespace Mega_Market_App.Validations;

public static class Validation
{
    public static bool IsText(string? value)
    {
        string pattern = @"^[A-ZƏĞİÖŞÜÇa-zəğıöşüç]+([ '-][A-ZƏĞİÖŞÜÇa-zəğıöşüç]+)*$";
        Regex regex = new Regex(pattern);

        if (value is not null && regex.IsMatch(value)) return true;
        else return false;
    }


    public static bool IsPassword(string? input)
    {
        string pattern = @"^(?=.*[A-Za-z])(?=.*\d).{8,}$";
        Regex regex = new Regex(pattern);

        if (input is not null && regex.IsMatch(input)) return true;
        else return false;
    }

    public static bool IsMail(string? input)
    {
        string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        Regex regex = new Regex(pattern);

        if (input is not null && regex.IsMatch(input)) return true;
        else return false;
    }

    public static bool IsCardNumber(string? input)
    {
        string pattern = @"^\d{16}$";
        Regex regex = new Regex(pattern);

        if (input is not null && regex.IsMatch(input)) return true;
        else return false;
    }

    public static bool IsCartYear(int? input)
    {
        var dateTime = DateTime.Now.Year;
        if(input > dateTime) return true;
        else return false;  
    }

    public static bool IsCardCVV(string? input)
    {
        string pattern = @"^\d{3}$";
        Regex regex = new Regex(pattern);

        if (input is not null && regex.IsMatch(input)) return true;
        else return false;
    }
}
