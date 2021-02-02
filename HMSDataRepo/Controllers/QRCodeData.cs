using System;

namespace HMSDataRepo.Controllers
{
    public class QRCodeData
    {
        public QRCodeData(int reservation_Id, int person_Id, string hash)
        {
            this.Reservation_Id = reservation_Id;
            this.Person_Id = person_Id;
            this.Hash = hash;
        }
        public static QRCodeData GetQRCodeData(string data)
        {
            string[] spl = data?.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (spl?.Length >= 3)
            {
                PeopleController peopleController = new PeopleController(WebApiConfig.Default);
                int Reservation_Id = int.Parse(spl[0]);
                int Person_Id = int.Parse(spl[1]);
                string Hash = spl[2];
                return new QRCodeData(Reservation_Id, Person_Id, Hash);// reserv;
            }
            else
            {
                throw new ArgumentException("Invaild data provided");
            }
        }
        public override string ToString()
        {
            return $"{Reservation_Id}|{Person_Id}|{Hash}";
        }
        public  string GetData()
        {
            return ToString();
        }
        public int Reservation_Id { get; }
        public int Person_Id { get; }
        public string Hash { get; }
    }
}