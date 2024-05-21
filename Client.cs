using System;

namespace DeepDiveIntoOOPPart3
{
    internal class Client
    {
        private string id;
        private string orgForm;
        private string fullName;
        private string requisites;        

        public string Id { get { return this.id; } }
        public string OrgForm { get { return this.orgForm; } }        

        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }                
        public string INN { get; set; }
        public string KPP { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public bool VIP { get; set; }

        public string Requisites { get { return this.requisites; } }
        public string FullName { get { return this.fullName; } }

        /// <summary>
        /// Карточка клиента
        /// </summary>
        /// <param name="Id">Идентификатор</param>
        /// <param name="OrgForm">Организационно-правовая форма</param>   
        /// <param name="Surname">Фамилия</param>
        /// <param name="Name">Имя</param>
        /// <param name="Patronymic">Отчество</param>
        /// <param name="INN">ИНН</param> 
        /// <param name="KPP">КПП</param>
        /// <param name="PhoneNumber">Номер телефона</param>
        /// <param name="PassportSeries">Серия паспорта</param> 
        /// <param name="PassportNumber">Номер паспорта</param>
        /// <param name="VIP">Статус</param>
        public Client(string Id, string OrgForm, string Surname, string Name, string Patronymic,
            string INN, string KPP, string PhoneNumber, string PassportSeries, string PassportNumber,
            bool VIP)
        {
            id = Id;
            orgForm = OrgForm;

            this.Surname = Surname;
            this.Name = Name;            
            this.Patronymic = Patronymic;
            this.INN = INN;
            this.KPP = KPP;
            this.PhoneNumber = PhoneNumber;
            this.PassportSeries = PassportSeries;
            this.PassportNumber = PassportNumber;
            this.VIP = VIP;

            if (OrgForm == "Индивидуальный предприниматель")
            {
                fullName = $"ИП ";
            }
            else
            {
                fullName = String.Empty;
            }

            if (OrgForm == "Юридическое лицо")
            {
                requisites = $"{this.INN}/{this.KPP}";
                fullName = $"{this.Name}";
            }
            else
            {
                requisites = $"{this.INN}";

                if (this.Patronymic.Length == 0)
                {
                    fullName += $"{this.Surname} {this.Name}";
                }
                else
                {
                    fullName += $"{this.Surname} {this.Name} {this.Patronymic}";
                }
            }            
        }        
    }
}
