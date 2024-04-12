using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using System.Xml.Serialization;
using RegonApiService;
using WcfCoreMtomEncoder;


namespace Sameposty.Services.REGON;
public class RegonService : IRegonService
{
    private readonly string _serviceKey = "c1983509b95e445cb350";
    private readonly bool _isProduction = true;

    public async Task<DanePodmiotu> GetCompanyData(string nip)
    {
        var searchParameters = new ParametryWyszukiwania
        {
            Nip = nip,
        };
        return await GetSearchResultModelAsync<DanePodmiotu>(searchParameters);

    }

    private async Task<T> GetSearchResultModelAsync<T>(ParametryWyszukiwania searchParameters) where T : class, new()
    {
        var seachResult = await GetSearchResultAsync(searchParameters);

        var doc = XDocument.Parse(seachResult);

        if (doc.Descendants("ErrorCode").Any())
        {
            return GetServiceErrors<T>(doc);
        }

        return DeserializeXMLElement<T>(doc.Descendants("dane").First());
    }

    private async Task<string> GetSearchResultAsync(ParametryWyszukiwania searchParameters)
    {
        string requestResult = string.Empty;

        var encoding = new MtomMessageEncoderBindingElement(new TextMessageEncodingBindingElement());
        var transport = new HttpsTransportBindingElement();
        var customBinding = new CustomBinding(encoding, transport);

        EndpointAddress endPoint = new("https://wyszukiwarkaregon.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc");
        if (!_isProduction)
        {
            endPoint = new EndpointAddress("https://wyszukiwarkaregontest.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc"); // test
        }

        UslugaBIRzewnPublClient client = new(customBinding, endPoint);
        await client.OpenAsync();
        var session = await client.ZalogujAsync(_serviceKey);

        using (new OperationContextScope(client.InnerChannel))
        {
            HttpRequestMessageProperty requestMessage = new();
            requestMessage.Headers["sid"] = session.ZalogujResult;
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
            var result = client.DaneSzukajPodmiotyAsync(searchParameters).GetAwaiter().GetResult();
            requestResult = result.DaneSzukajPodmiotyResult;
        }
        await client.WylogujAsync(session.ZalogujResult);
        await client.CloseAsync();

        return requestResult;
    }
    private static T DeserializeXMLElement<T>(XElement element) where T : class
    {
        if (element == null)
        {
            return null;
        }

        var serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(element.CreateReader()) as T;

    }

    private static T GetServiceErrors<T>(XDocument doc) where T : class, new()
    {
        var model = new T();

        if (!doc.Descendants("ErrorCode").Any())
        {
            return model;
        }

        var errors = doc.Descendants("dane");

        var property = model.GetType().GetProperty("Errors");

        Type element = typeof(ErrorModel);
        Type listType = typeof(List<>).MakeGenericType(element);
        var errorsList = Activator.CreateInstance(listType) as IList;

        foreach (var item in errors)
        {
            var error = DeserializeXMLElement<ErrorModel>(item);
            errorsList.Add(error);
        }

        property?.SetValue(model, errorsList, null);

        return model;
    }

}
