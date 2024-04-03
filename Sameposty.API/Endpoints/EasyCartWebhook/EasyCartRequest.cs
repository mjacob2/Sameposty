using System.Text.Json.Serialization;

namespace Sameposty.API.Endpoints.EasyCartWebhook;

public class InvoiceData
{
    [JsonPropertyName("nip")]
    public string Nip { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("street_number")]
    public string StreetNumber { get; set; }

    [JsonPropertyName("house_number")]
    public string HouseNumber { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("post_code")]
    public string PostCode { get; set; }

    [JsonPropertyName("post_city")]
    public string PostCity { get; set; }
}

public class Shipping
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("post_code")]
    public string PostCode { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }
}

public class EasyCartRequest
{
    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("is_subscription")]
    public bool IsSubscription { get; set; }

    [JsonPropertyName("is_renew")]
    public bool IsRenew { get; set; }

    [JsonPropertyName("is_new")]
    public bool IsNew { get; set; }

    [JsonPropertyName("last_chance")]
    public bool LastChance { get; set; }

    [JsonPropertyName("error_message")]
    public object ErrorMessage { get; set; }

    [JsonPropertyName("trial_ends_at")]
    public string TrialEndsAt { get; set; }

    [JsonPropertyName("subscription_renews_at")]
    public object SubscriptionRenewsAt { get; set; }

    [JsonPropertyName("subscription_canceled")]
    public bool SubscriptionCanceled { get; set; }

    [JsonPropertyName("subscription_expired")]
    public bool SubscriptionExpired { get; set; }

    [JsonPropertyName("subscription_new_plan_name")]
    public object SubscriptionNewPlanName { get; set; }

    [JsonPropertyName("amount_paid")]
    public double AmountPaid { get; set; }

    [JsonPropertyName("customer_name")]
    public string CustomerName { get; set; }

    [JsonPropertyName("customer_first_name")]
    public string CustomerFirstName { get; set; }

    [JsonPropertyName("customer_last_name")]
    public string CustomerLastName { get; set; }

    [JsonPropertyName("customer_email")]
    public string CustomerEmail { get; set; }

    [JsonPropertyName("customer_password")]
    public string CustomerPassword { get; set; }

    [JsonPropertyName("customer_id")]
    public int CustomerId { get; set; }

    [JsonPropertyName("customer_stripe_id")]
    public string CustomerStripeId { get; set; }

    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }

    [JsonPropertyName("order_amount")]
    public double OrderAmount { get; set; }

    [JsonPropertyName("order_has_invoice")]
    public bool OrderHasInvoice { get; set; }

    [JsonPropertyName("invoice_stripe_id")]
    public string InvoiceStripeId { get; set; }

    [JsonPropertyName("invoice_data")]
    public InvoiceData InvoiceData { get; set; }

    [JsonPropertyName("invoice_api_scope")]
    public string InvoiceApiScope { get; set; }

    [JsonPropertyName("shipping")]
    public Shipping Shipping { get; set; }

    [JsonPropertyName("product_name")]
    public string ProductName { get; set; }

    [JsonPropertyName("product_description")]
    public string ProductDescription { get; set; }

    [JsonPropertyName("price_name")]
    public string PriceName { get; set; }

    [JsonPropertyName("price_description")]
    public string PriceDescription { get; set; }

    [JsonPropertyName("subscription_id")]
    public int SubscriptionId { get; set; }

    [JsonPropertyName("subscription_stripe_id")]
    public string SubscriptionStripeId { get; set; }

    [JsonPropertyName("subscription_plan_name")]
    public string SubscriptionPlanName { get; set; }

    [JsonPropertyName("subscription_plan_price")]
    public int SubscriptionPlanPrice { get; set; }

    [JsonPropertyName("subscription_current_period_start")]
    public string SubscriptionCurrentPeriodStart { get; set; }

    [JsonPropertyName("subscription_current_period_end")]
    public string SubscriptionCurrentPeriodEnd { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }
}
