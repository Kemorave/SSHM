using HMSDataRepo.Model;
using HMSDataRepo.ViewModel;

namespace Self_service_hotel_managment.ViewModel.DBData.ControllerVM
{
    public class ManagersControllerVM :  ControllerVMBase<HMSDataRepo.Model.People, System.Net.HttpStatusCode>
    {
        public const string EditInDialogTokken = "EditInDialog";
        public static ManagersControllerVM Instance { get; } = new ManagersControllerVM();
        public ManagersControllerVM()
        {
            this.Controller = new HMSDataRepo.Controllers.PeopleController(HMSDataRepo.WebApiConfig.Default);
            //MessengerInstance.Register<People>(this, EditInDialogTokken, EditInDialog);
            EditInDialogCommand = new KemoTools.Commands.TypeCommand<People>(EditInDialog);
            RefreshList();
        }
        public override void RefreshList()
        {
            if (AllEntitiesList==null)
            {
                return;
            }
            base.RefreshList();
            if (AllEntitiesList.Count == 0)
            {
                AllEntitiesList.Add(new People() { PersonName = "Hema", Email = "Homadirar@hotmail.com" });
            }
        }
        private async void EditInDialog(People obj)
        {
            await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMetroDialogAsync(App.Current.MainWindow as MahApps.Metro.Controls.MetroWindow
                , new ViewModel.DBData.ControllrDialog.ManagerAccountEdit() { DataContext = obj });
        }



      
        public KemoTools.Commands.TypeCommand<People> EditInDialogCommand { get; }
    }
}