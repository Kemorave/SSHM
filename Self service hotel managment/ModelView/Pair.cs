using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Self_service_hotel_managment.ModelView
{
    public class Pair : GalaSoft.MvvmLight.ObservableObject
    {
        private decimal mp;

        public Pair(string key)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        Decimal Value
        {
            get { return mp; }
            set { mp = value; RaisePropertyChanged(); }
        }
        string Key { get; }
    }
}