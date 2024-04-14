namespace Sameposty.Services.EmailService;
internal static class EmailBodyProvider
{
    public static string NewPostsCreatedBodyEmail()
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Kliknij poniższy przycisk i zobacz, jakie wspaniałe, nowe posty właśnie przygotowalismy! Pamiętaj, aby je zatwierdzić! <br><br>
<a href=https://sameposty.pl style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Sprawdź nowe posty</a>

<p>-- Marek z sameposty.pl<br>";
    }

    public static string RegisterConfirmationEmailBody(string url, string token)
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Witaj w serwisie sameposty.pl! <br>
Aby potwierdzić swoje konto i wygenerować pierwsze, wspaniałe posty na social media, kliknij w poniższy przycisk <br><br>
<a href={url}/confirm?token={token} style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Potwierdź e-mail</a>

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

    public static string SubscriptionCanceledPaymentFailedEmailBody()
    {
        return $@"<p style=""margin-top: 20px;"">Cześć!<br>
<p>Niestety nie udało się pobrać kolejnej płatnosci z karty i subskrypcja na Twoim koncie dobiegła końca. Być może karta płatnicza straciła datę ważności? Mniejsza o to... <br><br>
Jeśli chcesz kontunuować subskrypcję, zaloguj się po prostu na swoje konto sameposty.pl i zamów subskrypcję ponownie :) <br><br>
<a href=https://sameposty.pl style=""background-color: #0042B6; border: none; color: white; padding: 15px 32px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 20px; cursor: pointer;"">Odnów subskrypcję</a>

<p>-- Marek z sameposty.pl<br>";
    }
}

