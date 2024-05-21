namespace DeepDiveIntoOOPPart3
{
    internal interface IDeposit<out T>
        where T : Account
    {        
        T DepositAccount(string ownerId, decimal amount);
    }
}
