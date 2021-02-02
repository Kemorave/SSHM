using System;

namespace HMSDataRepo.Model
{
    public class People : ICloneable,IDBModel
    {
        private const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _Password;

        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate)]
        public int ID { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public string PersonName { get; set; }

        [Newtonsoft.Json.JsonProperty]
        public string PasswordHash { get; set; }

        [Newtonsoft.Json.JsonProperty(DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore)]
        public string Password { get => _Password; set { _Password = value; PasswordHash = HashString(value); } }

        [Newtonsoft.Json.JsonProperty]
        public string AccountType { get; set; } = "User";

        [Newtonsoft.Json.JsonProperty]
        public string Email { get; set; }


        public static string HashString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            using (System.Security.Cryptography.SHA256Managed hashstring = new System.Security.Cryptography.SHA256Managed())
            { 
                byte[] hash = hashstring.ComputeHash(bytes);

                char[] hash2 = new char[16];

                for (int i = 0; i < hash2.Length; i++)
                {
                    hash2[i] = chars[hash[i] % chars.Length];
                } 
                return new string(hash2);
            }
        }

        public object Clone()
        {
            return new People() { AccountType = this.AccountType, Email = this.Email, ID = this.ID, PersonName = this.PersonName, Password = this.Password, PasswordHash = this.PasswordHash };
        }
    }
}