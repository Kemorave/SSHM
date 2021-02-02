using System;
using System.ComponentModel;

namespace HMSDataRepo.ViewModel
{
    //[Notify]
    public abstract class ControllerVMBase<Model, Result> : Kemo.ObservableObject, INotifyPropertyChanged where Result : Enum where Model : class,HMSDataRepo.Model.IDBModel
    {
        public const string AddEntityTokken = "Add";
        public const string DeleteEntityTokken = "Delete";
        public const string EditEntityTokken = "Edit";
        public const string SuccessTokken = "SuccessTokken";
        public const string FailTokken = "FailTokken";

        public ControllerVMBase()
        {
            AllEntitiesList = new Kemo.Collection.ObservableList<Model>();
            AddEntityCommand = new Kemo.Command.RelayCommand<Model>(AddEntiry);
            EditEntityCommand = new Kemo.Command.RelayCommand<Model>(EditEntiry);
            DeleteEntityCommand = new Kemo.Command.RelayCommand<Model>(DeleteEntiry);
            RefreshListCommand = new Kemo.Command.RelayCommand(RefreshList, CanRefreshList);
        }

        private bool CanRefreshList()
        {
            return !IsBusy;
        }

        public virtual async void RefreshList()
        {
            if (IsBusy)
            {
                return;
            }
            Message = "Loading please wait";
            IsBusy = true;
            await System.Threading.Tasks.Task.Run(async () =>
             {
                 try
                 {
                     System.Collections.Generic.IEnumerable<Model> list = await Controller.GetAllEntries();

                     AllEntitiesList.Clear();
                     AllEntitiesList.AddRange(list);

                     Message = "";
                 }
                 catch (Exception e)
                 {
                     Message = e.Message;
                 }
                 finally
                 {
                     IsBusy = false;
                 }
             });
        }


        protected async void AddEntiry(Model obj)
        {
            if (obj != null)
            {
                Result res = await Controller.AddEntry(obj);
                AllEntitiesList.Add(obj);
                //MessengerInstance.Send(new Tuple<Model, Result>(obj, res), SuccessTokken);
            }
        }

        protected async void DeleteEntiry(Model obj)
        {
            if (obj != null)
            {
                Result res = await Controller.DeleteByID(obj.ID);

                AllEntitiesList.Remove(obj);
                //MessengerInstance.Send(new Tuple<Model, Result>(obj, res));
            }
        }

        protected async void EditEntiry(Model obj)
        {
            if (obj != null)
            {
                Result res = await Controller.SetByID(obj.ID, obj);

                AllEntitiesList.Remove(obj);

                AllEntitiesList.Add(obj);
                //MessengerInstance.Send(new Tuple<Model, Result>(obj, res));
            }
        }
        public HMSDataRepo.Controllers.IController<Model, Result> Controller { get; set; }
        public Kemo.Command.RelayCommand<Model> AddEntityCommand { get; }
        public Kemo.Command.RelayCommand<Model> DeleteEntityCommand { get; }
        public Kemo.Command.RelayCommand<Model> EditEntityCommand { get; }
        public Kemo.Command.RelayCommand RefreshListCommand { get; }
        public Kemo.Collection.ObservableList<Model> AllEntitiesList { get; }
        public string Message { get => message; private set => SetProperty(ref message, value, messagePropertyChangedEventArgs); }
        public bool IsBusy { get => isBusy; private set => SetProperty(ref isBusy, value, isBusyPropertyChangedEventArgs); }
        public bool HasError { get => hasError; set => SetProperty(ref hasError, value, hasErrorPropertyChangedEventArgs); }

        #region NotifyPropertyChangedGenerator

        public new event PropertyChangedEventHandler PropertyChanged;

        private string message;
        private static readonly PropertyChangedEventArgs messagePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Message));
        private bool isBusy;
        private static readonly PropertyChangedEventArgs isBusyPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsBusy));
        private bool hasError;
        private static readonly PropertyChangedEventArgs hasErrorPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(HasError));

        private void SetProperty<X>(ref X field, X value, PropertyChangedEventArgs ev)
        {
            if (!System.Collections.Generic.EqualityComparer<X>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, ev);
            }
        }

        #endregion
    }
}