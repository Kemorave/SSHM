using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Self_service_hotel_managment;

namespace WPFTestApp.Services
{
 public class MainUIThreadIInvoker //: Kemo.IMainUIThreadIInvoker
 {
  public void Invoke(Action action)
  {
   App.Current.Dispatcher.Invoke(action);
  }
 }
}
