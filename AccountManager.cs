using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DeepDiveIntoOOPPart3
{
    internal class AccountManager<T> : IDeposit<T>
        where T : Account, IAccountConverter<T>, new()
    {
        private void WriteAccount(T account)
        {
            string accountInString = account.ConvertAccountToString(account);            

            using (StreamWriter streamWriter = new StreamWriter
            (Repository.GenerateAccountsFilePath(account.OwnerId), true, Encoding.Unicode))
            {
                streamWriter.WriteLine(accountInString);
            }
        }

        private bool CheckZeroAccountBalance(decimal balance)
        {
            bool status = true;

            if (balance != 0)
            {
                Repository.Message("Баланс закрываемого счета должен быть нулевым. " +
                "Переведите средства на другой счет");                

                status = false;
            }

            return status;
        }       

        private List<Account> GetAccountsFromFile(string ownerId)
        {
            List<Account> accounts = new List<Account>();            

            bool status = Repository.CheckBeforeReading(Repository.GenerateAccountsFilePath(ownerId));

            if (status == true)
            {
                using (StreamReader streamReader =
                new StreamReader(Repository.GenerateAccountsFilePath(ownerId), Encoding.Unicode))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string streamString = $"{streamReader.ReadLine()}";
                        string[] streamStringSplited = streamString.Split('#');

                        if (streamStringSplited[1] == "Депозитный")
                        {
                            DepositAccount account1 = new DepositAccount();
                            account1 = account1.ConvertStringSplitedToAccount(streamStringSplited);
                            accounts.Add(account1);
                        }

                        if (streamStringSplited[1] == "Недепозитный")
                        {
                            NonDepositAccount account2 = new NonDepositAccount();
                            account2 = account2.ConvertStringSplitedToAccount(streamStringSplited);
                            accounts.Add(account2);
                        }
                    }
                }
            }

            return accounts;
        }

        private void WriteAccounts(List<Account> accounts, string ownerId)
        {
            if (accounts != null && accounts.Count != 0)
            {
                using (StreamWriter streamWriter
                    = new StreamWriter(Repository.GenerateAccountsFilePath(ownerId),
                    false, Encoding.Unicode))
                {
                    string accountInString = String.Empty;

                    foreach (Account account in accounts)
                    {
                        if (account is DepositAccount savingsAccount)
                        {
                            accountInString = savingsAccount.ConvertAccountToString(savingsAccount);
                        }

                        if (account is NonDepositAccount paymentAccount)
                        {
                            accountInString = paymentAccount.ConvertAccountToString(paymentAccount);
                        }

                        streamWriter.WriteLine(accountInString);
                    }                    
                }
            }
            else
            {
                File.Delete(Repository.GenerateAccountsFilePath(ownerId));
            }
        }

        private bool CheckAvailabilitySameAccount(T account)
        {
            bool status = false;

            using (StreamReader streamReader =
                new StreamReader(Repository.GenerateAccountsFilePath(account.OwnerId), Encoding.Unicode))
            {
                while (!streamReader.EndOfStream)
                {
                    string streamString = $"{streamReader.ReadLine()}";
                    string[] streamStringSplited = streamString.Split('#');

                    if (streamStringSplited[1] == account.AccountType)
                    {
                        Repository.Message("У клиента уже открыт данный тип счета");
                        Repository.RollBackAccountId(account.Id);
                        status = true;
                        break;
                    }
                }
            }

            return status;
        }

        public void OwerwriteAccount(T account)
        {
            List<Account> accounts = GetAccountsFromFile(account.OwnerId);           

            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].GetType() == account.GetType())
                {
                    accounts[i] = account;

                    WriteAccounts(accounts, account.OwnerId);

                    break;
                }
            }
        }

        public T GetAccountFromFile(string ownerId)
        {
            T account = new T();

            bool status = Repository.CheckBeforeReading(Repository.GenerateAccountsFilePath(ownerId));

            if (status == true)
            {
                using (StreamReader streamReader =
                new StreamReader(Repository.GenerateAccountsFilePath(ownerId), Encoding.Unicode))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string streamString = $"{streamReader.ReadLine()}";
                        string[] streamStringSplited = streamString.Split('#');

                        if (account.GetType() == typeof(DepositAccount))
                        {
                            if (streamStringSplited[1] == "Депозитный")
                            {
                                account = account.ConvertStringSplitedToAccount(streamStringSplited);

                                break;
                            }
                        }

                        if (account.GetType() == typeof(NonDepositAccount))
                        {
                            if (streamStringSplited[1] == "Недепозитный")
                            {
                                account = account.ConvertStringSplitedToAccount(streamStringSplited);

                                break;
                            }
                        }                        
                    }
                }
            }            

            return account;
        }        

        public void OpenAccount(T account)
        {
            bool status = Repository.CheckBeforeReading(Repository.GenerateAccountsFilePath(account.OwnerId));

            if (status == true)
            {
                status = CheckAvailabilitySameAccount(account);

                if (status == false)
                {
                    WriteAccount(account);
                }
            }
            else
            {
                WriteAccount(account);
            }
        }

        public void CloseAccount(T account)
        {
            List<Account> accounts = GetAccountsFromFile(account.OwnerId);

            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].GetType() == account.GetType())
                {
                    if (CheckZeroAccountBalance(accounts[i].Balance) == true)
                    {
                        accounts.Remove(accounts[i]);

                        WriteAccounts(accounts, account.OwnerId);
                    }

                    break;
                }
            }
        }

        public void TransferToYourself(T senderAccount, decimal amount)
        {
            if (senderAccount.Balance >= amount)
            {
                List<Account> accounts = GetAccountsFromFile(senderAccount.OwnerId);

                if (accounts.Count == 2)
                {
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        if (accounts[i].GetType() != senderAccount.GetType())
                        {
                            accounts[i].Deposit(amount);
                        }

                        if (accounts[i].GetType() == senderAccount.GetType())
                        {
                            accounts[i].Withdraw(amount);
                        }
                    }

                    WriteAccounts(accounts, senderAccount.OwnerId);
                }
                else
                {
                    Repository.Message("У клиента отсутствует счет-получатель");
                }
            }
            else
            {
                Repository.Message("Недостаточно средств");
            }
        }
        
        public T DepositAccount(string ownerId, decimal amount)
        {
            T account = GetAccountFromFile(ownerId);

            account.Deposit(amount);

            return account;
        }
    }
}
