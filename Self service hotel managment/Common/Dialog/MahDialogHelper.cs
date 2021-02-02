
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using KemoTools.Commands;
using Self_service_hotel_managment;

namespace WinPace.Helper
{
  
  
 public static class MahDialogHelper
 {
  static MahDialogHelper()
  {
   OpenDialogCommand = new ObjectCommand(OpenDialog);
   OpenDialogCommandAsync = new ObjectCommand(OpeningDialogAsync);
   CloseDialogCommand = new ObjectCommand(CloseDialog);
  }

  private static async void CloseDialog(object obj)
  {
   if (obj is BaseMetroDialog dialog)
   {
    await CloseDialogAsync(dialog);
   }
  }

  private static async void OpeningDialogAsync(object obj)
  {
   if (obj is BaseMetroDialog dialog)
   {
    await OpenDialogAsync(dialog);
   }
  }
  private async static void OpenDialog(object obj)
  {
   if (obj is BaseMetroDialog dialog)
   {
    await OpenDialogAsync(dialog);
   }
  }

  public static ObjectCommand CloseDialogCommand { get; private set; }
  public static ObjectCommand OpenDialogCommand { get; private set; }
  public static ObjectCommand OpenDialogCommandAsync { get; private set; }
  
  public static CustomDialog GetCustomDialogInstance()
  {
   var dialog = new CustomDialog() { };
   dialog.Style = (Style)App.Current.TryFindResource("FullDialogStyle");
   return dialog;
  }
  public static async Task OpenAutoCloseDialogAsync(this BaseMetroDialog dialog, int millisecondsDelay = 5000)
  {
   await OpenDialogAsync(dialog);
   await Task.Delay(millisecondsDelay);
   await CloseDialogAsync(dialog);
  }
  public static async Task CloseDialogAsync(this BaseMetroDialog dialog)
  {
   await App.Current.Dispatcher.Invoke(async () =>
   {
    await MahApps.Metro.Controls.Dialogs.DialogManager.HideMetroDialogAsync( MahDialogHelper.GetActiveMahWindow(), dialog);
   });
  }
  public static async Task OpenDialogAsync(this BaseMetroDialog dialog)
  {
   await App.Current.Dispatcher.Invoke(async () =>
   {
    await GetActiveMahWindow().ShowMetroDialogAsync(dialog);
    await dialog.WaitUntilUnloadedAsync();
   }, System.Windows.Threading.DispatcherPriority.Send);
  }
  public static async void OpenDialog(this BaseMetroDialog dialog)
  {
   await App.Current.Dispatcher.Invoke(async () =>
   {
    await GetActiveMahWindow().ShowMetroDialogAsync(dialog);
   });
  }
  private static void ReportProgress(this ProgressDialogController controller, int perc, string title, string message)
  {
   controller.SetProgress((double)perc);
   controller.SetMessage(message);
   controller.SetTitle(title);

  }

  public static Window GetActiveWindow()
  {
   if (App.Current == null)
   {
    return null;
   }

   if (App.Current.MainWindow != null)
   {
    return App.Current.MainWindow;
   }

   return null;
  }
  public static MetroWindow GetActiveMahWindow()
  {
   if (App.Current == null)
   {
    return null;
   }

   if (App.Current.MainWindow != null)
   {
    return (MetroWindow)App.Current.MainWindow;
   }

   return null;
  }
  public static async Task<ProgressDialogController> ProgressDialoge(string title, string message, string cancelButtonText, bool cancelable = false, MetroWindow window = null, MetroDialogSettings settings = null)
  {
   if (window == null)
   {
    window = (GetActiveWindow() as MetroWindow);
   }
   if (settings == null)
   {
    settings = new MetroDialogSettings() { AnimateHide = true, AnimateShow = true };
   }
   settings.NegativeButtonText = cancelButtonText;
            var dialog = await MahApps.Metro.Controls.Dialogs.DialogManager.ShowProgressAsync(window, title, message, cancelable, settings); ;
            if (!cancelable)
            {
                dialog.SetIndeterminate();
            }
            return dialog;
  }
  public static async Task<MessageDialogResult> ShowMessage(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, MetroDialogSettings settings = null, MetroWindow window = null)
  {
   return await App.Current.Dispatcher.Invoke(async () =>
   {
    if (window == null)
    {
     window = (GetActiveWindow() as MetroWindow);

    }
    if (settings == null)
    {
     settings = new MetroDialogSettings() { AnimateHide = true, AnimateShow = true };
     settings.AffirmativeButtonText = ("Ok");
     settings.NegativeButtonText = ("No");
     settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
     settings.FirstAuxiliaryButtonText = ("Cancel");
     settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
    }

    return await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(window, title, message, style, settings);
   });
  }
  public static async Task<string> ShowInputDialog(string title, string message, MetroWindow window = null, MetroDialogSettings settings = null)
  {
   return await App.Current.Dispatcher.Invoke(async () =>
   {
    if (window == null)
    {
     window = (GetActiveWindow() as MetroWindow);

    }
    if (settings == null)
    {
     settings = new MetroDialogSettings() { AnimateHide = true, AnimateShow = true };
     settings.AffirmativeButtonText = ("Yes");
     settings.NegativeButtonText = ("No");
     settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
     settings.FirstAuxiliaryButtonText = ("Cancel");
     settings.DefaultButtonFocus = MessageDialogResult.Affirmative;
    }

    return await MahApps.Metro.Controls.Dialogs.DialogManager.ShowInputAsync(window, title, message, settings);
   });
  }

 }
}