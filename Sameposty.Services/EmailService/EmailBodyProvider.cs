
namespace Sameposty.Services.EmailService;
internal static class EmailBodyProvider
{
    public static string NewPostsCreatedBodyEmail()
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Dziekuję za opłacenie subskrypcji! <br><br>
<p>Kliknij poniższy przycisk i zobacz, jakie wspaniałe, nowe posty właśnie przygotowalismy! Pamiętaj, aby je zatwierdzić! Inaczej, nie zostaną opublikowane!<br><br>
<a href=https://sameposty.pl style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Sprawdź nowe posty</a>

<p>-- Marek z sameposty.pl<br>";
    }

    public static string RegisterConfirmationEmailBody(string url, string token)
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Witaj w serwisie sameposty.pl! <br>
Aby potwierdzić swoje konto i wygenerować pierwsze, wspaniałe posty na social media, kliknij w poniższy przycisk <br><br>
<a href={url}/confirm?token={token} style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Potwierdź e-mail</a>
<br><br>
Przycisk nie działa?<br>
Skopiuj i wklej poniższy link do przeglądarki: <br>
{url}/confirm?token={token} <br><br>
<p>-- Marek z sameposty.pl<br>";
    }

    public static string ResetPasswordEmailBody(string url, string to, string token, int userId)
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Podobno chcesz zmienić hasło w serwisie sameposty.pl! <br>
Aby to zrobić, kliknij w poniższy przycisk <br><br>
<a href={url}/update-password?token={token}&email={to}&id={userId} style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Resetuj hasło</a>

<p>-- Marek z sameposty.pl<br>";
    }

    public static string PaymentFailedEmailBody()
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Niestety nie udało się pobrać kolejnej płatnosci z karty. Być może karta płatnicza straciła datę ważności? Mniejsza o to... <br><br>
Jeśli chcesz kontunuować subskrypcję, zaloguj się po prostu na swoje konto sameposty.pl i przejdź do zarządzania subskrypcją i zaktualizuj dane karty. <br><br>
<a href=https://sameposty.pl style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Przejdź do sameposty.pl</a>

<p>-- Marek z sameposty.pl<br>";
    }

    public static string SubscriptionDeletedBodyEmail()
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Tylko nie to! <br><br>
Twoja subskrypcja sameposty.pl właśnie dobiegła końca! <br><br> Zapewne masz swoje powody. Rozumiem to i nie mam Ci za złe :) <br><br>
Jeśli jednak chcesz wrócić, zapraszam do ponownej subskrypcji! <br><br>
Będę również zobowiązany, jeśli w odpowiedzi na ten e-mail napiszesz mi, dlaczego zrezygnowałeś z subskrypcji. <br><br>
Dzieki temu będę wiedział, jakie aspekty sameposty.pl powinienem poprawić! <br><br>
Dzięki Tobie, sameposty.pl mogą stać się jeszcze lepsze! <br><br>

To, że zrezygnowałeś z subskrypcji, nie oznacza, że całe konto zostało anulowane! <br><br> Ciągle możesz logować się na sameposty.pl, Twoje zaplanowane posty możesz ciągle opublikować ręcznie, niewykorzystane tokeny do tworzenia tekstów i zdjęć również są ważne do końca bieżącego okresu! p<br><br>

<p>-- Marek z sameposty.pl<br>";
    }

    public static string SubscriptionCreatedBodyEmail()
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Dziekuję za zakup subskrypcji sameposty.pl! <br><br>

Nawet nie wiesz jak się cieszę! <br><br>

Wierzę, że pozostaniesz subskrybentem na długo! Przed nami setki ekscytujących postów do stworzenia i opublikownia na social mediach Twojej firmy!<br><br>
Poczekaj jeszcze chwilę na kolejny e-mail. Powinien przyjść za około 2-4 minuty z informacją, że kolejna porcja postów jest już gotowa! <br><br>

Oczekuj również na fakturę, którą także wyślemy mejlowo. Pamiętaj, że wystawcą faktury jest Middlers Sp. z o.o., właściciel serwisu sameposty.pl. <br><br>

<p>-- Marek z sameposty.pl<br>";
    }
}

