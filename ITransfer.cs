namespace DeepDiveIntoOOPPart3
{
    internal interface ITransfer<in T>
        where T : Account
    {        
        void Transfer(T senderAccount, T recipientAccount, decimal amount);
    }
}
