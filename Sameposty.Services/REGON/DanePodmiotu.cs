﻿using System.Xml.Serialization;

namespace Sameposty.Services.REGON;

[XmlRoot(ElementName = "dane", Namespace = "")]
public class DanePodmiotu
{
    [XmlElement("Regon")]
    public string Regon { get; set; }

    [XmlElement("Nip")]
    public string Nip { get; set; }

    [XmlElement("StatusNip")]
    public string StatusNip { get; set; }

    [XmlElement("Nazwa")]
    public string Nazwa { get; set; }

    [XmlElement("Wojewodztwo")]
    public string Wojewodztwo { get; set; }

    [XmlElement("Powiat")]
    public string Powiat { get; set; }

    [XmlElement("Gmina")]
    public string Gmina { get; set; }

    [XmlElement("Miejscowosc")]
    public string Miejscowosc { get; set; }

    [XmlElement("KodPocztowy")]
    public string KodPocztowy { get; set; }

    [XmlElement("Ulica")]
    public string Ulica { get; set; }

    [XmlElement("NrNieruchomosci")]
    public string NrNieruchomosci { get; set; }

    [XmlElement("NrLokalu")]
    public string NrLokalu { get; set; }

    [XmlElement("Typ")]
    public string Typ { get; set; }

    [XmlElement("SilosID")]
    public int SilosID { get; set; }

    [XmlElement("DataZakonczeniaDzialalnosci")]
    public string DataZakonczeniaDzialalnosci { get; set; }

    [XmlElement("MiejscowoscPoczty")]
    public string MiejscowoscPoczty { get; set; }

    [XmlIgnore]
    public List<ErrorModel> Errors { get; set; } = [];

}

[XmlRoot(ElementName = "dane", Namespace = "")]
public class ErrorModel
{
    [XmlElement("ErrorCode")]
    public string ErrorCode { get; set; }

    [XmlElement("ErrorMessagePl")]
    public string ErrorMessagePl { get; set; }

    [XmlElement("ErrorMessageEn")]
    public string ErrorMessageEn { get; set; }
}