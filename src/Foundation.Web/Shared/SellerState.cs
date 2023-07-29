namespace Foundation.Web.Shared
{
    public enum SellerState
    {
        Init = 1,
        Active = 2,
        PaymentOverDue5d = 3,
        PaymentOverDue20d = 4,
        Locked = 5,
        ManuallyAccepted = 6,
        ManuallyLocked = 7
    }
}
