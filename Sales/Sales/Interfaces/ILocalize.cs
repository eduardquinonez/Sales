namespace Sales.Interfaces
{
    using System.Globalization;

    public interface ILocalize
    {
        //Interface para determinar el idioma del celular
        CultureInfo GetCurrentCultureInfo();

        void SetLocale(CultureInfo ci);

    }
}
