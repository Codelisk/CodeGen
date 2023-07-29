namespace Foundation.Web.Shared
{
    using System;
    using System.Threading.Tasks;

    public interface IUserContext
    {
        int UserId { get; }
        string UserName { get; }
        string Email { get; }
        int SellerId { get; }
        int CashboxId { get; }
        string CurrencyCode { get; }
        string CountryCode { get; }
        bool ExemptTax { get; }
        int SellerTypId { get; }
        int SignatureType { get; }
        int? PrinterId { get; }
        string CultureCode { get; }
        bool UserLocked { get; }
        SellerState SellerState { get; }
        object Cache { get; }
        Task ResetCache();

    }
}
