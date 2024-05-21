using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace DeepDiveIntoOOPPart3
{
    internal static class Repository
    {
        private static readonly string clientsIdFilePath = "ClientsId.txt";
        private static readonly string accountsIdFilePath = "AccountsId.txt";        
        public static string ClientsFilePath { get { return "Clients.txt"; } }
        
        public static string CurrentOrgForm { get; set; }
        public static ClientFormsVM CurrentClientFormsVM { get; set; }
        public static ClientListVM CurrentClientListVM { get; set; }

        public static string RecipientId { get; set; }
        public static string RecipientName { get; set; }         
        public static string RecipientAcountType { get; set; }
        public static string RecipientAccountNumber { get; set; }      
        
        public static AccountManager<DepositAccount> DepositAccountManager
        { get { return new AccountManager<DepositAccount>(); } }
        public static AccountManager<NonDepositAccount> NonDepositAccountManager
        { get { return new AccountManager<NonDepositAccount>(); } }
               
        public static string GenerateAccountsFilePath(string ownerId)
        {
            string accountsFilePath = $"Accounts{ownerId}.txt";

            return accountsFilePath;
        }

        public static bool CheckBeforeReading(string filePath)
        {
            bool status = false;

            if (File.Exists(filePath) && new FileInfo(filePath).Length > 6)
            {
                status = true;
            }

            return status;
        }

        public static int AssignId(string filePath)
        {
            int id;
            string streamString;

            if (File.Exists(filePath) && new FileInfo(filePath).Length > 6)
            {
                using (StreamReader streamReader =
                    new StreamReader(filePath, Encoding.Unicode))
                {
                    streamString = $"{streamReader.ReadLine()}";
                }

                id = Convert.ToInt32(streamString) + 1;
                streamString = id.ToString();
            }
            else
            {
                streamString = "1";
                id = Convert.ToInt32(streamString);
            }

            using (StreamWriter streamWriter =
                new StreamWriter(filePath, false, Encoding.Unicode))
            {
                streamWriter.WriteLine(streamString);
            }

            return id;
        }

        public static int AssignClientId()
        {
            int id = AssignId(clientsIdFilePath);
            return id;
        }

        public static string AssignOrReadClientId(string possibleId)
        {
            string clientId;

            if (possibleId != null && possibleId.Length > 0)
            {
                clientId = possibleId;
            }
            else
            {
                clientId = (Repository.AssignClientId()).ToString();
            }

            return clientId;
        }        

        public static int AssignAccountId()
        {            
            int id = AssignId(accountsIdFilePath);
            return id;
        }

        public static void RollBackAccountId(string accountId)
        {
            int num = Convert.ToInt32(accountId) - 1;
            accountId = num.ToString();

            using (StreamWriter streamWriter =
                new StreamWriter(accountsIdFilePath, false, Encoding.Unicode))
            {
                streamWriter.WriteLine(accountId);
            }
        }

        public static void Message(string text)
        {
            MessageBox.Show(text,
                        "Операция не выполнена",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error,
                        MessageBoxResult.OK,
                        MessageBoxOptions.DefaultDesktopOnly);
        }

        public static List<ClientListItem> GetСlientListItems()
        {
            List<ClientListItem> list = new List<ClientListItem>();

            bool status = CheckBeforeReading(ClientsFilePath);

            if (status == true)
            {
                using (StreamReader streamReader =
                new StreamReader(ClientsFilePath, Encoding.Unicode))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string streamString = $"{streamReader.ReadLine()}";
                        string[] streamStringSplited = streamString.Split('#');

                        ClientListItem listItem = new ClientListItem()
                        {
                            Id = streamStringSplited[0],
                            OrgForm = streamStringSplited[1],
                            Requisites = streamStringSplited[11],
                            FullName = streamStringSplited[12]
                        };

                        list.Add(listItem);
                    }
                }
            }

            return list;
        }

        public static Client GetClientFromFile(string clientId)
        {
            Client client = null;

            using (StreamReader streamReader =
                new StreamReader(ClientsFilePath, Encoding.Unicode))
            {
                while (!streamReader.EndOfStream)
                {
                    string streamString = $"{streamReader.ReadLine()}";
                    string[] streamStringSplited = streamString.Split('#');

                    if (streamStringSplited[0] == clientId)
                    {
                        client = new Client(streamStringSplited[0], streamStringSplited[1],
                            streamStringSplited[2], streamStringSplited[3], streamStringSplited[4],
                            streamStringSplited[5], streamStringSplited[6], streamStringSplited[7],
                            streamStringSplited[8], streamStringSplited[9],
                            Convert.ToBoolean(streamStringSplited[10]));

                        break;
                    }
                }
            }

            return client;
        }        

        private static string ConvertClientToString(Client client)
        {
            string clientInString = $"{client.Id}#" +
                $"{client.OrgForm}#" +
                $"{client.Surname}#" +
                $"{client.Name}#" +
                $"{client.Patronymic}#" +
                $"{client.INN}#" +
                $"{client.KPP}#" +
                $"{client.PhoneNumber}#" +
                $"{client.PassportSeries}#" +
                $"{client.PassportNumber}#" +
                $"{client.VIP}#" +
                $"{client.Requisites}#" +
                $"{client.FullName}#";

            return clientInString;
        }

        private static void WriteClient(Client client)
        {
            string clientInString = ConvertClientToString(client);            

            using (StreamWriter streamWriter =
                new StreamWriter(ClientsFilePath, true, Encoding.Unicode))
            {                
                streamWriter.WriteLine(clientInString);
            }
        }

        private static List<Client> GetAllClientsFromFile()
        {
            List<Client> clients = new List<Client>();

            using (StreamReader streamReader =
                new StreamReader(ClientsFilePath, Encoding.Unicode))
            {
                while (!streamReader.EndOfStream)
                {
                    string streamString = $"{streamReader.ReadLine()}";
                    string[] streamStringSplited = streamString.Split('#');

                    Client client = new Client(streamStringSplited[0], streamStringSplited[1],
                            streamStringSplited[2], streamStringSplited[3], streamStringSplited[4],
                            streamStringSplited[5], streamStringSplited[6], streamStringSplited[7],
                            streamStringSplited[8], streamStringSplited[9],
                            Convert.ToBoolean(streamStringSplited[10]));

                    clients.Add(client);
                }
            }

            return clients;
        }

        private static void WriteAllClientsFromFile(List<Client> clients)
        {
            using (StreamWriter streamWriter =
                        new StreamWriter(ClientsFilePath, false, Encoding.Unicode))
            {
                foreach (Client client in clients)
                {
                    string clientInString = ConvertClientToString(client);
                    streamWriter.WriteLine(clientInString);
                }
            }
        }

        private static void OwerwriteClient(Client client)
        {
            List<Client> clients = GetAllClientsFromFile();

            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == client.Id)
                {
                    clients[i] = client;
                    break;
                }
            }

            WriteAllClientsFromFile(clients);
        }

        public static void OwerwriteOrWriteClient(Client clientFromForm)
        {
            bool status = CheckBeforeReading(ClientsFilePath);

            if (status == true)
            {
                Client clientFromFile = GetClientFromFile(clientFromForm.Id);
                
                if (clientFromFile != null)
                {
                    if (clientFromFile != clientFromForm)
                    {
                        OwerwriteClient(clientFromForm);
                    }
                }
                else
                {
                    WriteClient(clientFromForm);
                }                
            }
            else
            {
                WriteClient(clientFromForm);
            }
        }

        public static bool CheckAvailabilityOfAccounts(string clientId)
        {            
            bool status = CheckBeforeReading(GenerateAccountsFilePath(clientId));

            if (status == true)
            {
                Message("Клиент, имеющий незакрытые счета не может быть удален");                
            }

            return status;
        }

        public static void DeleteClient(string clientId)
        {
            bool status = CheckBeforeReading(ClientsFilePath);

            if (status == true)
            {
                status = CheckAvailabilityOfAccounts(clientId);

                if (status == false)
                {
                    List<Client> clients = GetAllClientsFromFile();

                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].Id == clientId)
                        {
                            clients.Remove(clients[i]);
                            break;
                        }
                    }

                    WriteAllClientsFromFile(clients);
                }                
            }
        }        
        
        public static string GetOnlyConvertibleIntoDecimal(string userInput)
        {
            Regex regex = new Regex("[^0-9,.]");

            if (regex.IsMatch(userInput))
            {
                userInput = userInput.Remove(userInput.Length - 1);
            }

            if (userInput.Contains(','))
            {
                int decimalPlaces = userInput.IndexOf(',') + 3;

                if (userInput.Length > decimalPlaces)
                {
                    userInput = userInput.Remove(userInput.Length - 1);
                }
            }

            if (userInput.Contains('.'))
            {
                userInput = userInput.Replace('.', ',');
            }

            return userInput;
        }        

        public static void ResetTransferInformation()
        {
            RecipientId = String.Empty;
            RecipientName = String.Empty;           
            RecipientAcountType = String.Empty;
            RecipientAccountNumber = String.Empty;            
        }

        public static void OwerwriteAccount(Account account)
        {
            if (account is DepositAccount depositAccount)
            {
                DepositAccountManager.OwerwriteAccount(depositAccount);
            }

            if (account is NonDepositAccount nonDepositAccount)
            {
                NonDepositAccountManager.OwerwriteAccount(nonDepositAccount);
            }
        }

        public static void CloseAccountWindow()
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window is DepositAccountForm || window is NonDepositAccountForm)
                {
                    window.Close();
                }
            }
        }
    }
}
