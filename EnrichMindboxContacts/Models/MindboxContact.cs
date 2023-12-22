using CsvHelper.Configuration.Attributes;

namespace EnrichMindboxContacts.Models;

[Delimiter(";")]
public class MindboxContact
{
    [Name("CustomerIdsBpmid")]
    public string? BpmId { get; set; }
    [Name("CustomerFirstName")]
    public string? FirstName { get; set; }
    [Name("CustomerMiddleName")]
    public string? MiddleName { get; set; }
    [Name("CustomerLastName")]
    public string? LastName { get; set; }
    [Name("CustomerEmail")]
    public string? Email { get; set; }
    [Name("CustomerMobilePhone")]
    public string? Phone { get; set; }
    
    public string CustomerCustomFieldsBrand { get; set; }
    public string CustomerCustomFieldsCustomerBalance { get; set; }
    public string CustomerCustomFieldsCustomerTown { get; set; }
    public string CustomerCustomFieldsDatePointExpired { get; set; }
    public string CustomerCustomFieldsEmailWaitConfirmation { get; set; }
    public string CustomerCustomFieldsOnDelete { get; set; }
    public string CustomerCustomFieldsPhoneConfirmed { get; set; }
    public string CustomerCustomFieldsRegisterPlDate { get; set; }
    public string CustomerSex { get; set; }
    public string CustomerAreaIdsExternalId { get; set; }
    public string CustomerAreaName { get; set; }
    public string CustomerIanaTimeZone { get; set; }
    public string CustomerTimeZoneSource { get; set; }
    public string CustomerIdsMindboxId { get; set; }
    public string CustomerBirthDate { get; set; }
    public string CustomerIsEmailInvalid { get; set; }
    public string CustomerIsMobilePhoneInvalid { get; set; }
    public string CustomerChangeDateTimeUtc { get; set; }
    public string CustomerIsEmailConfirmed { get; set; }
    public string CustomerIsMobilePhoneConfirmed { get; set; }
    public string CustomerIdsWebsiteID { get; set; }
    public string CustomerCustomerSubscriptionsAskonaruIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsAskonaruSmsIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsAskonaruEmailIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsAskonaruViberIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsAskonaruMobilePushIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsAskonaruWebPushIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsGretherWellsIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsGretherWellsSmsIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsGretherWellsEmailIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsGretherWellsViberIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsGretherWellsMobilePushIsSubscribed { get; set; }
    public string CustomerCustomerSubscriptionsGretherWellsWebPushIsSubscribed { get; set; }
    public string CustomerPendingEmail { get; set; }
    public string CustomerPendingMobilePhone { get; set; }
    public string CustomerBalanceCashBackTotal { get; set; }
    public string CustomerBalanceCashBackAvailable { get; set; }
    public string CustomerBalanceCashBackBlocked { get; set; }
    public string CustomerBalanceMarketingBonusTotal { get; set; }
    public string CustomerBalanceMarketingBonusAvailable { get; set; }
    public string CustomerBalanceMarketingBonusBlocked { get; set; }
}