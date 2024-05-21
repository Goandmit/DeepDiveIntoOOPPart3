namespace DeepDiveIntoOOPPart3
{
    internal interface IAccountConverter<T>
    {
        string ConvertAccountToString(T account);
        T ConvertStringSplitedToAccount(string[] streamStringSplited);
    }
}
