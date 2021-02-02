using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DeviceUtil;
using GalaSoft.MvvmLight.CommandWpf;
using HMSDataRepo.Controllers;
using HMSDataRepo.Model;
using Self_service_hotel_managment;
using Self_service_hotel_managment.ModelView;
using Self_service_hotel_managment.View.Dialog;
using Services;

namespace VM
{
    public class AppData : GalaSoft.MvvmLight.ViewModelBase
    {
        public enum ControllerTask { Add, Delete, Edit }
        public static AppData Instance { get; } = new AppData();

        private AppData()
        { 
            TotalHotelInfo = new TotalHotelInfo();
            _RoomDevicesController = new RoomDevicesController();
            _recordsController = new RecordsController(HMSDataRepo.WebApiConfig.Default);
            _peopleController = new PeopleController(HMSDataRepo.WebApiConfig.Default);
            _reservationsController = new ReservationsController(HMSDataRepo.WebApiConfig.Default);
            _roomsController = new RoomsController(HMSDataRepo.WebApiConfig.Default);


            RoomsList = new KemoTools.Collections.ObservableCollection<Rooms>();
            Cameras = new KemoTools.Collections.ObservableCollection<Camera>();
            RoomDevicesList = new KemoTools.Collections.ObservableCollection<RoomDevices>();
            ReservationsList = new KemoTools.Collections.ObservableCollection<Reservations>();
            RecordsList = new KemoTools.Collections.ObservableCollection<Records>();
            UsersList = new KemoTools.Collections.ObservableCollection<People>();
            ReservationErrorRecordsList = new KemoTools.Collections.ThreadSafeCollection<Records>();

            RefreshTotalHotelInfoCommand = new RelayCommand(async () => { await RefreshTotalHotelInfo(); });
            RefreshUsersCommand = new RelayCommand(async () => { await RefreshUsers(); });
            RefreshReservationsCommand = new RelayCommand(async () => { await RefreshReservations(); });
            RefreshRoomsCommand = new RelayCommand(async () => { await RefreshRooms(); });
            RefreshRecordsCommand = new RelayCommand(async () => { await RefreshRecords(); });
            RefreshCamerasCommand = new RelayCommand(async () => { await RefreshCameras(); });
            RefreshRoomDevicesCommand = new RelayCommand(async () => { await RefreshRoomDevices(); });
            DismissReservationErrorRecordsListCommand = new RelayCommand(() =>
            {
                Task.Run(() =>
                {
                    foreach (Records item in ReservationErrorRecordsList.ToList())
                    {
                        Thread.Sleep(100);
                        ReservationErrorRecordsList.Remove(item);
                    }
                });
            });

            EditCommand = new RelayCommand<object>(EditItem, (object obj) =>
            {
                if (obj is Records || obj is Reservations)
                {
                    return false;
                }
                return true;
            });
            AddCommand = new RelayCommand<object>(AddItem);
            DeleteCommand = new RelayCommand<object>((object obj) =>
            {
                if (MessageBox.Show(App.Current.MainWindow, "This action cant be reversed", "Are you sure ?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
                {
                    return;
                }
                DeleteItem(obj);
            }, (object obj) =>
            {
                if (obj is Records)
                {
                    return false;
                }
                return true;
            });

            DeviceUtil.Recognition.LiveRecognitionUnit.ResultFound += LiveRecognitionUnit_ResultFound;

        }

        private bool DDialogOpen;
        private async void LiveRecognitionUnit_ResultFound(object sender, DeviceUtil.Recognition.RecognitionResult e)
        {
            if (DDialogOpen)
            {
                return;
            }
            int Person_ID = CurrentLoginInfo.ID;
            DDialogOpen = true;
            MahApps.Metro.Controls.Dialogs.ProgressDialogController waitd = null;
            await App.Current.Dispatcher.Invoke(async () =>
             {
                 waitd = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Resolving data please wait", "Checking reservations this won't take long", null);
             }); // waitd.SetIndeterminate();
            waitd.Maximum = 10;
            try
            {
                QRCodeData res = HMSDataRepo.Controllers.QRCodeData.GetQRCodeData(e.Result.Text);
                Person_ID = res.Person_Id;
                if (await VaildateQRData(res))
                {
                    waitd.SetProgress(3);
                    People per = await _peopleController.GetByID(res.Person_Id);
                    waitd.SetProgress(5);
                    Reservations reserv = await _reservationsController.GetByID(res.Reservation_Id);
                    if (reserv.Room_Id != e.Key || reserv.Person_Id != per.ID || !reserv.IsAvailable || res.Hash != per.PasswordHash)
                    {
                        throw new InvalidOperationException($"Invaild login attempt for room {reserv.Room_Id} with reservation {res.Reservation_Id} , user {res.Person_Id}");
                    }
                    waitd.SetProgress(9);
                    await RecordEvent("Door activity", $"Reservation {res.Reservation_Id} Person {res.Person_Id} door is open", res.Person_Id);
                    DoorActivityDialog dialog = null;
                    await waitd.CloseAsync();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        dialog = new DoorActivityDialog(per, reserv, e.CameraName);
                    });
                    await WinPace.Helper.MahDialogHelper.OpenDialogAsync(dialog);
                }
                else
                {
                    throw new InvalidOperationException($"Data not found for reservation {res.Reservation_Id} , user {res.Person_Id}");
                }
            }
            catch (Exception ex)
            {
                await waitd.CloseAsync();
                await WinPace.Helper.MahDialogHelper.ShowMessage("Invaild data", "This reservation is expired or not vaild for current room" + (!string.IsNullOrEmpty(ex.Message) ? (" see details below: \n" + ex.Message) : null));

                ReservationErrorRecordsList.Add(await RecordEvent("Invaild Door activity", ex.Message,Person_ID));
            }
            finally
            {
                DDialogOpen = false;
            }
        }

        private readonly Dictionary<int, Reservations> SavedReservations = new Dictionary<int, Reservations>();

        private async Task<bool> VaildateQRData(QRCodeData data)
        {
            if (SavedReservations.ContainsKey(data.Reservation_Id))
            {
                return true;
            }
            else
            {
                await RefreshReservations();
                if (ReservationsList.FirstOrDefault(s => s.ID == data.Reservation_Id) is Reservations reservations)
                {
                    SavedReservations[reservations.ID] = reservations;
                    return true;
                }
            }
            return false;
        }
        private async void DeleteItem(object obj)
        {
            MahApps.Metro.Controls.Dialogs.ProgressDialogController dialog = null;
            try
            {
                dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", "Cancel", true);
                if (obj is IList list)
                {
                    dialog.Maximum = list.Count;
                    int c = 0;
                    foreach (object item in list)
                    {
                        if (dialog.IsCanceled)
                        {
                            return;
                        }
                        DeleteItem(obj);
                        c++;
                        dialog.SetProgress(c);
                    }
                    return;
                }
                switch (obj)
                {
                    case People item:
                        if (item?.ID == CurrentLoginInfo?.ID) { ErrorDialog("Cant remove admin", "Invaild operation"); return; }
                        await PeopleTask(item, ControllerTask.Delete); break;
                    case Reservations item: await ReservationsTask(item, ControllerTask.Delete); break;
                    case RoomDevices item: await RoomDevicesTask(item, ControllerTask.Delete); break;
                    case Rooms item: await RoomsTask(item, ControllerTask.Delete); break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (dialog != null)
                {
                    await dialog?.CloseAsync();
                }
            }

        }

        private async void AddItem(object obj)
        {
            MahApps.Metro.Controls.Dialogs.ProgressDialogController dialog = null;
            try
            {
                switch (obj)
                {
                    case "People":
                        {
                            People item = await Self_service_hotel_managment.View.Dialog.PeopleDialog.ShowAddDialog();
                            if (item != null)
                            {
                                dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", null);
                                await PeopleTask(item, ControllerTask.Add);
                            }
                            break;
                        }
                    case "Rooms":
                        {
                            Rooms item = await Self_service_hotel_managment.View.Dialog.RoomDialog.ShowAddDialog();
                            if (item != null)
                            {
                                dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", null);
                                await RoomsTask(item, ControllerTask.Add);
                            }
                            break;
                        }
                    case Camera item:
                        {
                            RoomDevices caminfo = await Self_service_hotel_managment.View.Dialog.RoomCamerInfoDialog.ShowAddDialog(item, RoomsList.Select(s => s.ID).ToList());
                            if (caminfo != null)
                            {
                                dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", null);
                                await RoomDevicesTask(caminfo, ControllerTask.Add);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (dialog != null)
                {
                    await dialog?.CloseAsync();
                }
            }
        }

        private async void EditItem(object obj)
        {
            MahApps.Metro.Controls.Dialogs.ProgressDialogController dialog = null;
            try
            {
                switch (obj)
                {
                    case People item:
                        item = await Self_service_hotel_managment.View.Dialog.PeopleDialog.ShowEditDialog(item);
                        if (item != null)
                        {
                            dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", null);
                            await PeopleTask(item, ControllerTask.Edit);
                        }

                        break;
                    case Rooms item:
                        item = await Self_service_hotel_managment.View.Dialog.RoomDialog.ShowEditDialog(item);
                        if (item != null)
                        {
                            dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", null);
                            await RoomsTask(item, ControllerTask.Edit);
                        }

                        break;
                    case RoomDevices item:
                        item = await Self_service_hotel_managment.View.Dialog.RoomCamerInfoDialog.ShowEditDialog(item, RoomsList.Select(s => s.ID).ToList());
                        if (item != null)
                        {
                            dialog = await WinPace.Helper.MahDialogHelper.ProgressDialoge("Please wait", "Updating data", null);
                            await RoomDevicesTask(item, ControllerTask.Edit);
                        }

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (dialog != null)
                {
                    await dialog?.CloseAsync();
                }
            }
        }

        private async Task RefreshTotalHotelInfo()
        {
            if (IsLoadingHotelInfo)
            {
                return;
            }
            IsLoadingHotelInfo = true;
            await Task.Run(async () =>
            {
                try
                {
                    await RefreshRooms(true);
                    await RefreshReservations(true);
                    await RefreshRoomDevices();
                    TotalHotelInfo.RefreshCalculations(ReservationsList, RoomsList);
                }
                catch (Exception e)
                {
                    Toast = e.Message;
                }
                finally
                {
                    IsLoadingHotelInfo = false;
                }
            });
        }

        private async Task RefreshCameras(bool silent = false)
        {
            if (IsLoadingCameras)
            {
                return;
            }
            IsLoadingCameras = silent ? false : true;
            await Task.Run(() =>
            {
                try
                {
                    Cameras.AddRange(Util.GetCameras().Where(s => RoomDevicesList.FirstOrDefault(c => c.Camera_Path == s.DevicePath) == null), true);
                }
                catch (Exception e)
                {
                    Toast = e.Message;
                }
                finally
                {
                    IsLoadingCameras = false;
                }
            });
        }
        private async Task RefreshRoomDevices(bool silent = false)
        {
            if (IsLoadingCameras)
            {
                return;
            }
            IsLoadingCameras = silent ? false : true;
            await Task.Run(async () =>
            {
                try
                {
                    foreach (RoomDevices item in RoomDevicesList.ToList())
                    {
                        Thread.Sleep(100);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {


                                item.LiveRecognitionUnit.Stop();
                                RoomDevicesList.Remove(item);
                            }
                            catch
                            {

                            }
                        });
                    }
                    RoomDevicesList.AddRange(await _RoomDevicesController.GetAllEntries(), true);
                    foreach (RoomDevices item in RoomDevicesList)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                item.LiveRecognitionUnit.Start();
                            }
                            catch
                            {

                            }
                        });
                    }
                    Cameras.AddRange(Util.GetCameras().Where(s => RoomDevicesList.FirstOrDefault(c => c.Camera_Path == s.DevicePath) == null), true);
                }
                catch (Exception e)
                {
                    Toast = e.Message;
                }
                finally
                {
                    IsLoadingCameras = false;
                }
            });
        }

        private async Task RefreshRecords(bool silent = false)
        {
            if (IsLoadingRecords)
            {
                return;
            }
            IsLoadingRecords = silent ? false : true;
            await Task.Run(async () =>
            {
                try
                {
                    List<Records> records = (await _recordsController.GetAllEntries())?.Where(r => r.Person_Id == CurrentLoginInfo.ID).ToList();

                    if (records == null || records?.Count == 0)
                    {
                        if (!silent)
                        {
                            Toast = "No records available";
                        }
                        //  ErrorDialog("Please try later", "No room available");
                    }
                    else
                    {
                        this.RecordsList.AddRange(records, true);
                    }
                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = "Bad network \n" + e.Message;
                    }
                }
                finally
                {
                    IsLoadingRecords = false;
                }
            });
        }

        public async Task RefreshRooms(bool silent = false)
        {
            if (IsLoadingRooms)
            {
                return;
            }
            IsLoadingRooms = silent ? false : true;
            await Task.Run(async () =>
            {
                try
                {
                    List<Rooms> tempRo = (await _roomsController.GetAllEntries())?.ToList();


                    this.RoomsList.AddRange(tempRo, true);

                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = $"Bad network {e.Message}";
                    }
                }
                finally
                {
                    IsLoadingRooms = false;
                }
            });
        }

        public async Task RefreshReservations(bool silent = false)
        {
            if (IsLoadingResrvations)
            {
                return;
            }
            IsLoadingResrvations = silent ? false : true; ;
            await Task.Run(async () =>
            {
                try
                {
                    List<Reservations> reservations = (await _reservationsController.GetAllEntries()).ToList();
                    if (reservations == null || reservations?.Count <= 0)
                    {
                        if (!silent)
                        {
                            Toast = "No reservations available";
                        }
                    }
                    else
                    {
                        this.ReservationsList.AddRange(reservations, true);
                    }
                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = "Bad network \n" + e.Message;
                    }
                }
                finally
                {
                    IsLoadingResrvations = false;
                }
            });
        }
        public async Task RefreshUsers(bool silent = false)
        {
            if (IsLoadingUsers)
            {
                return;
            }
            IsLoadingUsers = silent ? false : true; ;
            await Task.Run(async () =>
            {
                try
                {
                    List<People> users = (await _peopleController.GetAllEntries()).ToList();
                    if (users == null || users?.Count <= 0)
                    {
                        if (!silent)
                        {
                            Toast = "No accounts available";
                        }
                    }
                    else
                    {
                        this.UsersList.AddRange(users, true);
                    }
                }
                catch (Exception e)
                {
                    if (!silent)
                    {
                        Toast = "Bad network \n" + e.Message;
                    }
                }
                finally
                {
                    IsLoadingUsers = false;
                }
            });
        }

        private async void Initialize(bool silent = false)
        {
            try
            {
                await RefreshUsers();
                await RefreshRecords(silent);
                // await RefreshRooms(silent);
                // await RefreshRoomDevices();
                await RefreshTotalHotelInfo();
                //await RefreshReservations(silent);
                //IsBusy = false;
            }
            catch (Exception) { }
        }





        private async Task<Records> RecordEvent(string type, string content, int? id = null)
        {
            Records _ = new Records() { Type = type, Content = content, Person_Id = id ?? CurrentLoginInfo.ID };

            try
            {
                if (id == null)
                {
                    id = CurrentLoginInfo.ID;
                }
                RecordsList.Add(_);
                await _recordsController.AddEntry(_);
            }
            catch (Exception e)
            {
                Toast = "Records error " + e.Message;
            }
            return _;
        }


        public async Task<bool> ReservationsTask(Reservations obj, ControllerTask task)
        {
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    System.Net.HttpStatusCode res = System.Net.HttpStatusCode.Unused;
                    switch (task)
                    {
                        case ControllerTask.Add:
                            res = await _reservationsController.AddEntry(obj);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                await RefreshReservations(true);
                            }

                            break;
                        case ControllerTask.Delete:

                            res = await _reservationsController.DeleteByID(obj.ID);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                this.ReservationsList.Remove(obj);
                            }

                            break;
                        case ControllerTask.Edit:
                            res = await _reservationsController.SetByID(obj.ID, obj);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                await RefreshReservations(true);
                            }

                            break;
                        default:
                            break;
                    }
                    if (res != System.Net.HttpStatusCode.OK)
                    {
                        Toast = $"Bad network please try again {res}";
                    }
                    return res == System.Net.HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Registration error");
                    return false;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
        public async Task<bool> RoomsTask(Rooms obj, ControllerTask task)
        {
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    System.Net.HttpStatusCode res = System.Net.HttpStatusCode.Unused;
                    switch (task)
                    {
                        case ControllerTask.Add:
                            res = await _roomsController.AddEntry(obj);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                await RefreshRooms(true);
                            }

                            break;
                        case ControllerTask.Delete:

                            res = await _roomsController.DeleteByID(obj.ID);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                this.RoomsList.Remove(obj);
                            }

                            break;
                        case ControllerTask.Edit:
                            res = await _roomsController.SetByID(obj.ID, obj);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                await RefreshRooms(true);
                            }

                            break;
                        default:
                            break;
                    }
                    if (res != System.Net.HttpStatusCode.OK)
                    {
                        Toast = $"Bad network please try again {res}";
                    }
                    return res == System.Net.HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Registration error");
                    return false;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
        public async Task<bool> PeopleTask(People obj, ControllerTask task)
        {
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    System.Net.HttpStatusCode res = System.Net.HttpStatusCode.Unused;
                    switch (task)
                    {
                        case ControllerTask.Add:
                            res = await _peopleController.AddEntry(obj);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                await RefreshUsers();
                            }

                            break;
                        case ControllerTask.Delete:

                            res = await _peopleController.DeleteByID(obj.ID);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                UsersList.Remove(obj);
                            }

                            break;
                        case ControllerTask.Edit:
                            res = await _peopleController.SetByID(obj.ID, obj);
                            if (res == System.Net.HttpStatusCode.OK)
                            {
                                await RefreshUsers();
                            }

                            break;
                        default:
                            break;
                    }
                    if (res != System.Net.HttpStatusCode.OK)
                    {
                        Toast = $"Bad network please try again {res}";
                    }
                    return res == System.Net.HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Registration error");
                    return false;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
        public async Task<bool> RoomDevicesTask(RoomDevices obj, ControllerTask task)
        {
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {

                    System.Data.SQLite.SQLiteErrorCode res = System.Data.SQLite.SQLiteErrorCode.Abort;
                    switch (task)
                    {
                        case ControllerTask.Add:
                            res = await _RoomDevicesController.AddEntry(obj);
                            if (res == System.Data.SQLite.SQLiteErrorCode.Ok)
                            {
                                await RefreshRoomDevices(true);
                            }

                            break;
                        case ControllerTask.Delete:


                            res = await _RoomDevicesController.DeleteByID(obj.ID);
                            if (res == System.Data.SQLite.SQLiteErrorCode.Ok)
                            {
                                this.RoomDevicesList.Remove(obj);
                            }
                            try
                            {
                                await RefreshCameras(true);

                                obj.LiveRecognitionUnit.VideoElement.Dispatcher.Invoke(() =>
                                {

                                    obj.LiveRecognitionUnit.Stop();
                                    obj.LiveRecognitionUnit.IsActive = false;
                                });
                            }
                            catch (Exception)
                            {

                            }

                            break;
                        case ControllerTask.Edit:
                            res = await _RoomDevicesController.SetByID(obj.ID, obj);
                            if (res == System.Data.SQLite.SQLiteErrorCode.Ok)
                            {
                                await RefreshRoomDevices(true);
                            }

                            break;
                        default:
                            break;
                    }
                    if (res != System.Data.SQLite.SQLiteErrorCode.Ok)
                    {
                        Toast = $"Error occured {res}";
                    }
                    return res == System.Data.SQLite.SQLiteErrorCode.Ok;
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Registration error");
                    return false;
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
        public async Task<bool?> Login(People obj)
        {
            return await Login(obj.Email, obj.PasswordHash);
        }
        public async Task<bool> DeleteReservation(Reservations reserv)
        {
            if (IsBusy)
            {
                return false;
            }
            IsBusy = true;
            return await Task.Run(async () =>
            {
                try
                {
                    System.Net.HttpStatusCode res = await _reservationsController.DeleteByID(reserv.ID);
                    if (res == System.Net.HttpStatusCode.OK)
                    {
                        Toast = "Successfully deleted";
                        this.ReservationsList.Remove(reserv);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    ErrorDialog(e.Message, "Task faild");
                }
                finally
                {
                    IsBusy = false;
                }
                return true;
            });
        }
        public async Task<bool?> Login(string email, string hash)
        {
            IsBusy = true;
            return await Task.Run(new Func<bool?>(() =>
            {
                try
                {

                    RespondeMessage resm = _peopleController.Authorize(email, hash).Result;
                    if (resm.Error)
                    {
                        ErrorDialog(resm.Message, "Login failed");
                        return false;
                    }
                    People item = _peopleController.GetByID(resm.ID).Result;
                    Services.LocalData.SaveLoginInfo(item);
                    IsBusy = false;
                    IsLoggedIn = true;
                    CurrentLoginInfo = item;
                    //Initialize();
                    Toast = "Loading";
                    RecordEvent("Login", "Login detected on device " + Environment.MachineName).Wait();

                    return true;
                    //    return null;
                }
                catch (Exception e)
                {
                    ErrorDialog("Please check you network \n" + e.Message, "connection faild");
                    return false;
                }
                finally
                {
                    IsBusy = false;
                } 
            }));
        }



        public async Task<bool?> AutoLoginAsync()
        {
            People person = LocalData.GetLoginInfo();
            if (person == null)
            {
                return null;
            }
            else
            {
                return await Login(person); ;
            }
        }

        private void ErrorDialog(string value, string title = "Error")
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(App.Current.MainWindow, value, title, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
            });
        }

        #region Prop


        public string Toast
        {
            get => toast;
            set
            {
                toast = value; RaisePropertyChanged();
            }
        }
        public string Total
        {
            get => toast;
            set
            {
                toast = value; RaisePropertyChanged();
            }
        }

        public string RecordsFilter { get => _RecordsFilter; set { _RecordsFilter = value; FilterRecordsAsync(value); } }
        private async void FilterRecordsAsync(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value) && value != "None")
                {
                    await RefreshRecords();
                    RecordsList.AddRange(RecordsList.Where(s => s.Type.Equals(value, StringComparison.OrdinalIgnoreCase)).ToList(), true);
                }
                else
                {
                    await RefreshRecords();
                }
            }
            catch (Exception e)
            {
                Toast = e.Message;
            }
        }

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingRecords
        {
            get => isLoadingRecords;
            set
            {
                isLoadingRecords = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingCameras
        {
            get => isLoadingCameras;
            set
            {
                isLoadingCameras = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingResrvations
        {
            get => isLoadingResrvations;
            set
            {
                isLoadingResrvations = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingUsers
        {
            get => isLoadingUsers;
            set
            {
                isLoadingUsers = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingRooms
        {
            get => isLoadingRooms;
            set
            {
                isLoadingRooms = value; RaisePropertyChanged();
            }
        }
        public bool IsLoadingHotelInfo
        {
            get => isLoadingHotelInfo;
            set
            {
                isLoadingHotelInfo = value; RaisePropertyChanged();
            }
        }
        public bool DevicesWarning => RoomDevicesList.Count < this.RoomsList.Count || RoomDevicesList.Count == 0;

        public bool IsLoggedIn
        {
            get => isLoaggedIn;
            set { isLoaggedIn = value; RaisePropertyChanged(); if (value) { Initialize(true); } }
        }
        public KemoTools.Collections.ObservableCollection<Reservations> ReservationsList { get; }
        //  public     KemoTools.Collections.ObservableCollection<ActuatorFuncData> Actuators { get; }
        public KemoTools.Collections.ThreadSafeCollection<Records> ReservationErrorRecordsList { get; }
        public KemoTools.Collections.ObservableCollection<Records> RecordsList { get; }
        public RelayCommand RefreshTotalHotelInfoCommand { get; }
        public RelayCommand RefreshUsersCommand { get; }
        public KemoTools.Collections.ObservableCollection<Rooms> RoomsList { get; }
        public KemoTools.Collections.ObservableCollection<People> UsersList { get; }

        public KemoTools.Collections.ObservableCollection<Camera> Cameras { get; }
        public KemoTools.Collections.ObservableCollection<RoomDevices> RoomDevicesList { get; }
        public People CurrentLoginInfo
        {
            get => currentLoginInfo;
            set { currentLoginInfo = value; RaisePropertyChanged(); }
        }

        public RelayCommand LogoutCommand { get; }
        public RelayCommand RefreshRoomsCommand { get; }
        public RelayCommand RefreshRecordsCommand { get; }
        public RelayCommand RefreshCamerasCommand { get; }
        public RelayCommand RefreshRoomDevicesCommand { get; }
        public RelayCommand DismissReservationErrorRecordsListCommand { get; }
        public RelayCommand<object> EditCommand { get; }
        public RelayCommand<object> AddCommand { get; }
        public RelayCommand<object> DeleteCommand { get; }
        public RelayCommand RefreshReservationsCommand { get; }

        #endregion
        #region Vars
        private People currentLoginInfo;

        public TotalHotelInfo TotalHotelInfo { get; }

        private readonly RoomDevicesController _RoomDevicesController;
        private readonly RecordsController _recordsController;
        private readonly PeopleController _peopleController;
        private readonly ReservationsController _reservationsController;
        private readonly RoomsController _roomsController;

        private bool isBusy, isLoaggedIn;
        private string _RecordsFilter;
        private bool isLoadingRecords;
        private bool isLoadingResrvations;
        private bool isLoadingRooms;
        private string toast;
        private bool isLoadingCameras;
        private bool isLoadingHotelInfo;
        private bool isLoadingUsers;




        #endregion
    }
}