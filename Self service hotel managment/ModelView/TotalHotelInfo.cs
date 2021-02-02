using System;
using System.Collections.Generic;
using System.Linq;
using HMSDataRepo.Model;
using KemoTools.Collections;

namespace Self_service_hotel_managment.ModelView
{
    public class TotalHotelInfo : GalaSoft.MvvmLight.ObservableObject
    {
        public TotalHotelInfo()
        {
            FreeRoomsList = new ObservableCollection<Rooms>();
        }
        private decimal tp, dp, mp, exp;
        private int pr, ad, fr, ru;

        public void RefreshCalculations(IList<Reservations> reservations, IList<Rooms> rooms)
        {
            try
            {
                IsLoaded = false;
                Message = "Refreshing data please wait";
                FreeRoomsList.Clear(); bool free = true;
                foreach (var room in rooms)
                {
                    free = true;
                    foreach (var res in reservations)
                    {
                        if (res.Room_Id == room.ID && res.IsAvailable)
                        {
                            free = false;
                            break;
                        }
                    }
                    if (free)
                    {
                        FreeRoomsList.Add(room);
                    }
                }
                RoomsUsage = (int)KemoTools.Converters.ToPercentageValue.ToPercentage(this.FreeRoomsList.Count, rooms.Count);
                TotalExpectedProfit = rooms.Sum(r => r.Price);
                TotalProfits = reservations.Sum(r => r.TotalPrice);
                TotalProgress = (int)KemoTools.Converters.ToPercentageValue.ToPercentage((long)TotalProfits, (long)TotalExpectedProfit);
                 MonthProfits = 0;
                DayProfits = 0;
                foreach (Reservations item in reservations)
                {
                   
                    if (item.StartDate.Month == DateTime.Now.Month)
                    {
                        MonthProfits += item.TotalPrice;
                    }
                    if (item.StartDate.Day == DateTime.Now.Day)
                    {
                        DayProfits += item.TotalPrice;
                    }
                }
                IsLoaded = true;
                Message = null;
            }
            catch (Exception e)
            {
                Message = "Could not retrieve data \n" + e.Message;
            }
        }
        private string message;
        private bool loaded;

        public bool IsLoaded
        {
            get { return loaded; }
            set { loaded = value; RaisePropertyChanged(); }
        }

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public int RoomsUsage
        {
            get => ru;
            set { ru = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<Rooms> FreeRoomsList { get; set; }
       
        public int TotalProgress
        {
            get => pr;
            set { pr = value; RaisePropertyChanged(); }
        }
        public decimal TotalExpectedProfit
        {
            get => exp;
            set { if (exp == value) { return; } exp = value; RaisePropertyChanged(); }
        }
        public decimal MonthProfits
        {
            get => mp;
            set { mp = value; RaisePropertyChanged(); }
        }
        public decimal DayProfits
        {
            get => dp;
            set { dp = value; RaisePropertyChanged(); }
        }
        public decimal TotalProfits
        {
            get => tp;
            set { tp = value; RaisePropertyChanged(); }
        }
    }
}