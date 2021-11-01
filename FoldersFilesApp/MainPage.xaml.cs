using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FoldersFilesApp.Classes;
using Windows.Storage.AccessCache;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FoldersFilesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        #region Properties

        public ObservableCollection<Classes.ApplicationDataItem> ApplicationDataItems { get; set; } =
            new ObservableCollection<Classes.ApplicationDataItem>();

        public ObservableCollection<Classes.PicruresLibraryItem> PicturesLibraryItems { get; set; } =
            new ObservableCollection<Classes.PicruresLibraryItem>();

        public ObservableCollection<Classes.GroupItem> GroupItems { get; set; } =
            new ObservableCollection<Classes.GroupItem>();

        public ObservableCollection<Classes.ItemOpenPicker> ItemsOpenPicker { get; set; } =
            new ObservableCollection<Classes.ItemOpenPicker>();

        public ObservableCollection<Classes.FutureAccessItem> FutureAccessListItems { get; set; } =
            new ObservableCollection<Classes.FutureAccessItem>();

        public ObservableCollection<Classes.FutureAccessItem> MostRecentlyUsedItems { get; set; } =
            new ObservableCollection<Classes.FutureAccessItem>();

        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            InitializeApplictionDataItems();

            InitializePicturesLibraryItems();

            //InitializeFutureAccessItems();

            //InitializeMostRecentlyUsedItem();
        }

        #region Get File Properties

        private async Task<StorageFile> TryFindPictureAsync()
        {
            Windows.Storage.StorageFolder PicturesFolders =
                Windows.Storage.KnownFolders.CameraRoll;

            var query = PicturesFolders.CreateFileQuery();
            var cameraRoleFiles = await query.GetFilesAsync();
            foreach (Windows.Storage.StorageFile file in cameraRoleFiles)
            {
                if (((Classes.PicruresLibraryItem)listPictures.SelectedItem).FileName == file.Name)
                {
                    return file;
                }
            }

            PicturesFolders = Windows.Storage.KnownFolders.SavedPictures;

            var qurey2 = PicturesFolders.CreateFileQuery();
            var savedPicturesFiles = await qurey2.GetFilesAsync();
            foreach (Windows.Storage.StorageFile file in savedPicturesFiles)
            {
                if (((Classes.PicruresLibraryItem)listPictures.SelectedItem).FileName == file.Name)
                {
                    return file;
                }
            }

            PicturesFolders = Windows.Storage.KnownFolders.PicturesLibrary;

            var qurey3 = PicturesFolders.CreateFileQuery();
            var PicturesLibraryFiles = await qurey3.GetFilesAsync();
            foreach (Windows.Storage.StorageFile file in PicturesLibraryFiles)
            {
                if (((Classes.PicruresLibraryItem)listPictures.SelectedItem).FileName == file.Name)
                {
                    return file;
                }
            }

            return null;
        }

        private async void InitializePicturesLibraryItems()
        {
            Windows.Storage.StorageFolder picturesLibraryFolder =
                Windows.Storage.KnownFolders.PicturesLibrary;

            var PicturesFolders = await picturesLibraryFolder.GetFoldersAsync();

            foreach (Windows.Storage.StorageFolder folder in PicturesFolders)
            {
                var pictureFiles = await folder.GetFilesAsync();
                foreach (Windows.Storage.StorageFile file in pictureFiles)
                {
                    PicturesLibraryItems.Add(new Classes.PicruresLibraryItem()
                    {
                        FileName = file.Name,
                        FileType = (file.FileType == ".PNG" ? "PNG File" : "JPG File"),
                        ImagePath = new Windows.UI.Xaml.Media.Imaging.BitmapImage(
                          (file.FileType == ".PNG" ? new Uri("ms-appx:///Assets/Icons/PNG_40px.png") : new Uri("ms-appx:///Assets/Icons/JPG_40px.png")))
                    });
                }
            }

            var files = await picturesLibraryFolder.GetFilesAsync();

            foreach (Windows.Storage.StorageFile file in files)
            {
                PicturesLibraryItems.Add(new Classes.PicruresLibraryItem()
                {
                    FileName = file.Name,
                    FileType = (file.FileType == ".PNG" ? "PNG File" : "JPG File"),
                    ImagePath = new Windows.UI.Xaml.Media.Imaging.BitmapImage(
                        (file.FileType == ".PNG" ? new Uri("ms-appx:///Assets/Icons/PNG_40px.png") : new Uri("ms-appx:///Assets/Icons/JPG_40px.png")))
                });
            }

        }

        private async void SetExpendedPropertiesPicture(StorageFile pictureFile)
        {
            const string dateAccessedProperty = "System.DateAccessed";
            const string fileOwnerProperty = "System.FileOwner";



            // Define property names to be retrieved
            List<string> propertyNames = new List<string>();
            propertyNames.Add(dateAccessedProperty);
            propertyNames.Add(fileOwnerProperty);

            // Get extended properties.
            IDictionary<string, object> extraProperties =
                await pictureFile.Properties.RetrievePropertiesAsync(propertyNames);

            //Get date-accessed property
            var propValue = extraProperties[dateAccessedProperty];
            if (propValue != null)
            {
                textBlockDateAccessed.Text =
                    string.Format("Date Accessed: {0}", propValue.ToString());
            }

            //Get owner file propery
            var propertyValue2 = extraProperties[fileOwnerProperty];
            if (propertyValue2 != null)
            {
                textBlockOwnerFile.Text =
                    "Owner File: " + propertyValue2;
            }
        }

        private async void SetBasicPropertiesPicture(StorageFile pictureFile)
        {
            // Get file's basic properties.
            Windows.Storage.FileProperties.BasicProperties pictureFileProperties =
                await pictureFile.GetBasicPropertiesAsync();

            textBlockSizeFile.Text =
                string.Format("Size: {0:0n}", pictureFileProperties.Size.ToString());
            textBlockDateModified.Text =
                "Last Modified Date: " + pictureFileProperties.DateModified.ToString();
        }

        private async void listPictures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.Storage.StorageFile PictureFile = await TryFindPictureAsync();

            if (PictureFile != null)
            {
                textBlockFileName.Text =
                    "File Name: " + PictureFile.DisplayName;
                textBlockFileType.Text =
                    "File Type: " + PictureFile.FileType;

                SetBasicPropertiesPicture(PictureFile);

                SetExpendedPropertiesPicture(PictureFile);
            }
        }

        #endregion

        #region Create,Delete,Write and Read Files

        private async void InitializeApplictionDataItems()
        {
            Windows.Storage.StorageFolder localFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;

            var files = await localFolder.GetFilesAsync();

            if (files != null)
            {
                foreach (Windows.Storage.StorageFile file in files)
                {
                    ApplicationDataItems.Add(new Classes.ApplicationDataItem()
                    {
                        FileName = file.Name,
                        FileType = "Text File"
                    });
                }
            }
        }

        private async void btnCreatingFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCreatingFile.Text))
            {
                if (!IsFileExisted())
                {
                    Windows.Storage.StorageFolder storageFolder =
                        Windows.Storage.ApplicationData.Current.LocalFolder;

                    Windows.Storage.StorageFile file =
                        await storageFolder.CreateFileAsync(string.Format("{0}.txt", txtCreatingFile.Text), Windows.Storage.CreationCollisionOption.ReplaceExisting);

                    ApplicationDataItems.Add(new Classes.ApplicationDataItem()
                    {
                        FileName = file.Name,
                        FileType = "Text File"
                    });

                    listApplicationDataFiles.ItemsSource = ApplicationDataItems;
                }
                else
                {
                    await new Windows.UI.Xaml.Controls.ContentDialog()
                    {
                        Title = "Alert",
                        Content = "You already have this file in you local folder",
                        CloseButtonText = "Close",
                        DefaultButton = ContentDialogButton.Close
                    }.ShowAsync();
                }
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Notification",
                    Content = "Please fill textbox first",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
        }

        private bool IsFileExisted()
        {
            for (int i = 0; i < listApplicationDataFiles.Items.Count; i++)
            {
                if (((Classes.ApplicationDataItem)listApplicationDataFiles.Items[i]).FileName == String.Format("{0}.txt",txtCreatingFile.Text))
                {
                    return true;
                }
            }
            return false;
        }

        private async void btnDeletingFile_Click(object sender, RoutedEventArgs e)
        {
            if (listApplicationDataFiles.SelectedItem != null)
            {
                txtBlockFileName.Text = "You Selected: ";
                txtBlockFileName2.Text = "You Selected: ";
                txtBlockDeletingFileName.Text = "You Selected:";

                Windows.Storage.StorageFolder localFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile file =
                    await localFolder.GetFileAsync(((Classes.ApplicationDataItem)listApplicationDataFiles.SelectedItem).FileName);
                await file.DeleteAsync(StorageDeleteOption.Default);

                ApplicationDataItems.RemoveAt(listApplicationDataFiles.SelectedIndex);
                listApplicationDataFiles.SelectedItem = null;

                listApplicationDataFiles.ItemsSource = ApplicationDataItems;
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Notification",
                    Content = "Select an item from listview",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(writeTextOfFile.Text) && listApplicationDataFiles.SelectedItem != null)
            {
                Windows.Storage.StorageFolder localFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;

                Windows.Storage.StorageFile file =
                    await localFolder.GetFileAsync(((Classes.ApplicationDataItem)listApplicationDataFiles.SelectedItem).FileName);

                await Windows.Storage.FileIO.WriteTextAsync(file, writeTextOfFile.Text);
            }
            else if (string.IsNullOrEmpty(writeTextOfFile.Text) && listApplicationDataFiles.SelectedItem == null)
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Alert",
                    Content = "You don't select an item of listview and don't fill textbox",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
            else if (listApplicationDataFiles.SelectedItem is null)
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Notification",
                    Content = "Please Select an item of listview",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
            else 
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Notification",
                    Content = "Please fill textbox first",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            if (listApplicationDataFiles.SelectedItem != null)
            {
                Windows.Storage.StorageFolder localFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;

                Windows.Storage.StorageFile file =
                    await localFolder.GetFileAsync(((Classes.ApplicationDataItem)listApplicationDataFiles.SelectedItem).FileName);

                readTextOfFile.Text = await Windows.Storage.FileIO.ReadTextAsync(file);
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Notification",
                    Content = "Please select on an item of listview",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }

        }

        private void listApplicatoinDataFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listApplicationDataFiles.SelectedItem != null)
            {
                txtBlockFileName.Text = "You selected: " +
                    string.Format("\n{0}", ((Classes.ApplicationDataItem)listApplicationDataFiles.SelectedItem).FileName);

                txtBlockFileName2.Text = "You Selected" +
                    string.Format("\n{0}", ((Classes.ApplicationDataItem)listApplicationDataFiles.SelectedItem).FileName);

                txtBlockDeletingFileName.Text = "You Selected" +
                    string.Format("\n{0}", ((Classes.ApplicationDataItem)listApplicationDataFiles.SelectedItem).FileName);
            }
        }


        #endregion

        #region Enumerate and Qurey File and Folders

        private Windows.UI.Xaml.Media.Imaging.BitmapImage GetImageForSpecifiedFile(StorageFile file)
        {
            if (file.FileType == ".PNG" || file.FileType == ".JPG"|| file.FileType == ".png" || file.FileType == ".jpg")
            {
                return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_Google_Images_40px.png"));
            }
            else if (file.FileType == ".MP3" || file.FileType == ".ZPL" ||file.FileType == ".mp3" || file.FileType == ".zpl")
            {
                return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_Music_Record_40px.png"));
            }
            else if (file.FileType == ".MP4" || file.FileType == ".WMV" || file.FileType == ".mp4" || file.FileType == ".wmv")
            {
                return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_Video_Camera_40px_2.png"));
            }
            else if (file.FileType == ".EXE" || file.FileType == ".MSI" || file.FileType == ".exr" || file.FileType == ".msi")
            {
                return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/Software_40px.png"));
            }
            else
            {
                return new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png"));
            }
        }

        private async void bordAlphaperSorting_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (GroupItems.Count > 0)
            {
                GroupItems.Clear();
            }

            //get the items from FutureAccessList
            var futureAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;

            //We will send futureAccessList and get some process to get files from folder called "FoldersFilesTesting" that existed in DocumentsLibrary
            List<Classes.DocumentsLibraryItem> documentsLibraryItems = await GetFilesFromFutureAccessListAsync(futureAccessList);

            //Here we will send documetnsLibraryItems as a parameter to get the first character to each file and storing it as string 
            List<string> groups = GetHeaderOfGroupedItems(documentsLibraryItems);

            //Help us to fill the collection of each GroupItem
            List<Classes.DocumentsLibraryItem> documents = null; 

            for (int i = 0; i < groups.Count; i++)
            {
                documents = new List<Classes.DocumentsLibraryItem>(); 

                for (int j = 0; j < documentsLibraryItems.Count; j++)
                {
                    //Check if The first Character for each ItemName of DocumentLibraryItem is equal to group item
                    //like (A ==? [C]apture.png)
                    if (groups[i] == Char.ToUpper(documentsLibraryItems[j].ItemName[0]).ToString())
                    {
                        documents.Add(documentsLibraryItems[j]);
                    }
                }

                GroupItems.Add(new Classes.GroupItem()
                {
                    DocumentLibraryItems = documents,
                    Key = groups[i]
                });

                documents = null;
            }
        }

        private List<string> GetHeaderOfGroupedItems(List<DocumentsLibraryItem> documentsLibraryItems)
        { 
            List<string> groups = new List<string>();

            for (int i = 0; i < documentsLibraryItems.Count; i++)
            {
                if (groups.Count > 0)
                {
                    bool isFound = false;

                    for (int j = 0; j < groups.Count; j++)
                    {
                        if (groups[j] == Char.ToUpper(documentsLibraryItems[i].ItemName[0]).ToString())
                        {
                            isFound = true; 
                        }
                    }

                    if (isFound == false)
                        groups.Add(Char.ToUpper(documentsLibraryItems[i].ItemName[0]).ToString());
                }
                else
                {
                    groups.Add(Char.ToUpper( documentsLibraryItems[i].ItemName[0] ).ToString());
                }
            }
            return groups;
        }

        private async System.Threading.Tasks.Task<List<Classes.DocumentsLibraryItem>> GetFilesFromFutureAccessListAsync(StorageItemAccessList futureAccessList)
        {
            List<Classes.DocumentsLibraryItem> documentsLibraryItems =
                  new List<Classes.DocumentsLibraryItem>();

            foreach (Windows.Storage.AccessCache.AccessListEntry futureAccessListEntry in futureAccessList.Entries)
            {
                Windows.Storage.IStorageItem item = await futureAccessList.GetItemAsync(futureAccessListEntry.Token);

                if (item != null)
                {
                    if (item is StorageFolder)
                    {
                        if (((StorageFolder)item).Name == "FilesFoldersTesting")
                        {
                            var files = await ((StorageFolder)item).GetFilesAsync();
                            foreach (Windows.Storage.StorageFile file in files)
                            {
                                if (file != null)
                                {
                                    documentsLibraryItems.Add(new Classes.DocumentsLibraryItem()
                                    {
                                        ItemName = file.Name,
                                        ItemType = file.FileType + " File",
                                        ItemImage = GetImageForSpecifiedFile(file)
                                    });
                                }
                            }
                            return documentsLibraryItems;
                        }
                    }
                }
            }
            return null;
        }

        private void bordSortingByType_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void bordSortingByDate_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        #endregion

        #region File Open Picker

        private void InitializeItemsOpenPicker(IReadOnlyList<StorageFile> pickedFiles)
        {
            if (ItemsOpenPicker.Count > 0)
            {
                ItemsOpenPicker.Clear();

                foreach (Windows.Storage.StorageFile file in pickedFiles)
                {
                    ItemsOpenPicker.Add(new Classes.ItemOpenPicker()
                    {
                        ItemImage =
                             new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png")),
                        ItemName = file.Name,
                        ItemPath = file.Path
                    });
                }
            }
            else
            {
                foreach (Windows.Storage.StorageFile file in pickedFiles)
                {
                    ItemsOpenPicker.Add(new Classes.ItemOpenPicker()
                    {
                        ItemImage =
                             new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png")),
                        ItemName = file.Name,
                        ItemPath = file.Path
                    });
                }
            }
        }

        private async void btnPickSingleFile_Click(object sender, RoutedEventArgs e)
        {
            var pickSingleFile = new Windows.Storage.Pickers.FileOpenPicker();

            pickSingleFile.CommitButtonText = "Open";
            pickSingleFile.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            pickSingleFile.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            pickSingleFile.FileTypeFilter.Add(".png");
            pickSingleFile.FileTypeFilter.Add(".jpg");
            pickSingleFile.FileTypeFilter.Add(".jpeg");
            pickSingleFile.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile pickedFile = await pickSingleFile.PickSingleFileAsync();
            if (pickedFile != null)
            {
                imageDiplayFileFolderType.Source = 
                    new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png"));
                txtDisplayFileFolderNamePath.Text = string.Format("File Name: {0} \n\n File Path: {1}", pickedFile.Name, pickedFile.Path);
            }
            else
            {
                txtDisplayFileFolderNamePath.Text = "This operation is canceled";
            }
        }

        private async void btnPickSingleFolder_Click(object sender, RoutedEventArgs e)
        {
            var pickSingleFolder = new Windows.Storage.Pickers.FolderPicker();

            pickSingleFolder.CommitButtonText = "Open";
            pickSingleFolder.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            pickSingleFolder.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;

            pickSingleFolder.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder pickedFolder = await pickSingleFolder.PickSingleFolderAsync();
            if (pickedFolder != null)
            {
                imageDiplayFileFolderType.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/Folder_40px.png"));
                txtDisplayFileFolderNamePath.Text = string.Format("Folder Name: {0} \n\n Folder Path: {1}", pickedFolder.Name, pickedFolder.Path); 
            }
            else
            {
                txtDisplayFileFolderNamePath.Text = "This operation is canceled";
            }

        }

        private async  void btnPickMultipleFiles_Click(object sender, RoutedEventArgs e)
        {
            var pickMultipleFiles = new Windows.Storage.Pickers.FileOpenPicker();
            pickMultipleFiles.CommitButtonText = "Open";
            pickMultipleFiles.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            pickMultipleFiles.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            pickMultipleFiles.FileTypeFilter.Add(".jpg");
            pickMultipleFiles.FileTypeFilter.Add(".jpeg");
            pickMultipleFiles.FileTypeFilter.Add(".png");
            pickMultipleFiles.FileTypeFilter.Add("*");

            var pickedFiles = await pickMultipleFiles.PickMultipleFilesAsync();
            if (pickedFiles.Count > 0)
            {
                InitializeItemsOpenPicker(pickedFiles);
            }
        }

        #endregion

        #region File Save Picker
        private async void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtForSaving.Text))
            {
                var saveFile = new Windows.Storage.Pickers.FileSavePicker();

                saveFile.CommitButtonText = "Save";
                saveFile.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                if (string.IsNullOrEmpty(txtFileNameForSaving.Text))
                {
                    saveFile.SuggestedFileName = "New Document";
                }
                else
                {
                    saveFile.SuggestedFileName = txtFileNameForSaving.Text;
                }
                saveFile.FileTypeChoices.Add("Text Format(*.txt)", new List<string>() { ".txt" });

                Windows.Storage.StorageFile pickedSaveFile = await saveFile.PickSaveFileAsync();
                if (pickedSaveFile != null)
                {
                    //Prevent updates to the remote version of the file until
                    //we finish making changes and call CompleteUpdatedAsync.
                    Windows.Storage.CachedFileManager.DeferUpdates(pickedSaveFile);
                    //write to file
                    await Windows.Storage.FileIO.WriteTextAsync(pickedSaveFile, txtForSaving.Text);
                    //Let Windows know that we're finished changing the file so
                    //the other app can update the remote version of the file 
                    //Completing updates may require  Windows to ask for user input
                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(pickedSaveFile);
                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        await new Windows.UI.Xaml.Controls.ContentDialog()
                        {
                            Title = "Mission status",
                            Content = "File: " + pickedSaveFile.DisplayName + " was saved.",
                            CloseButtonText = "Close",
                            DefaultButton = ContentDialogButton.Close
                        }.ShowAsync();
                    }

                }
            }
            else
            {
                await new Windows.UI.Xaml.Controls.ContentDialog()
                {
                    Title = "Alert",
                    Content = "You must full the second textbox first",
                    CloseButtonText = "Close",
                    DefaultButton = ContentDialogButton.Close
                }.ShowAsync();
            }
        }


        #endregion

        //FutureAccessList: you can add 1000 item to it but if it would be filled you can't add an item to that list
        //MostRecentlyUsed: you can add 25 item to it and if it would be filled it automatically remove the oldest item and add the new item
        #region Most Recently Used and Future Access List

        private async void InitializeMostRecentlyUsedItem()
        {
            //Get Items from MostRecentlyUsedItems firstly
            var mru = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList;

            foreach (Windows.Storage.AccessCache.AccessListEntry mostRecentlyUsedItem in mru.Entries)
            {
                //token = mostRecentlyUsedItem.Token;
                //metaData = mostRecentlyUsedItem.Metadata;
                Windows.Storage.IStorageItem item = await mru.GetItemAsync(mostRecentlyUsedItem.Token);

                if (item is StorageFile)
                {
                    var basicsProperties = await ((StorageFile)item).GetBasicPropertiesAsync();
                    string lastMod = basicsProperties.DateModified.ToString();

                    MostRecentlyUsedItems.Add(new Classes.FutureAccessItem()
                    {
                        ItemName = ((StorageFile)item).Name,
                        ItemOpenedTime = lastMod,
                        ItemImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png"))
                    });
                }
                else if (item is StorageFolder)
                {
                    var basicsProperties = await ((StorageFolder)item).GetBasicPropertiesAsync();
                    string lastMod = basicsProperties.DateModified.ToString();

                    MostRecentlyUsedItems.Add(new Classes.FutureAccessItem()
                    {
                        ItemName = ((StorageFolder)item).Name,
                        ItemOpenedTime = lastMod,
                        ItemImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/Folder_40px.png"))
                    });
                }
            }
        }

        private async void InitializeFutureAccessItems()
        {
            //Get the items of FutureAccessList
            var futureAccessListItems = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
            foreach (Windows.Storage.AccessCache.AccessListEntry futureAccessListItem in futureAccessListItems.Entries)
            {
                //string token = futureAccessListItem.Token;
                //string metaDate = futureAccessListItem.Metadata;

                Windows.Storage.IStorageItem item =
                    await futureAccessListItems.GetItemAsync(futureAccessListItem.Token);
                if (item is StorageFile)
                {
                    var basicsProperties = await ((StorageFile)item).GetBasicPropertiesAsync();
                    string lastMod = basicsProperties.DateModified.ToString();

                    FutureAccessListItems.Add(new Classes.FutureAccessItem()
                    {
                        ItemName = ((StorageFile)item).Name,
                        ItemOpenedTime = lastMod,
                        ItemImage =
                           new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png"))
                    });
                }
                else if (item is StorageFolder)
                {
                    var basicsProperties = await ((StorageFolder)item).GetBasicPropertiesAsync();
                    string lastMod = basicsProperties.DateModified.ToString();

                    FutureAccessListItems.Add(new Classes.FutureAccessItem()
                    {
                        ItemName = ((StorageFolder)item).Name,
                        ItemOpenedTime = lastMod,
                        ItemImage =
                           new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/Folder_40px.png"))
                    });
                }
            }
        }

        private async void btnFileOpenPicker_Click(object sender, RoutedEventArgs e)
        {
            var fileOpenPicker = new Windows.Storage.Pickers.FileOpenPicker();

            fileOpenPicker.CommitButtonText = "Open";
            fileOpenPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            fileOpenPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;

            fileOpenPicker.FileTypeFilter.Add(".txt");
            fileOpenPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile file = await fileOpenPicker.PickSingleFileAsync();
            if (file != null)
            {
                var futureAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
                futureAccessList.Add(file, "Adding a file");

                var mru = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList;
                mru.Add(file, "Adding a file");

                var basicsProperties = await file.GetBasicPropertiesAsync();
                string lastMod = basicsProperties.DateModified.ToString();

                FutureAccessListItems.Add(new Classes.FutureAccessItem()
                {
                    ItemName = file.Name,
                    ItemOpenedTime = lastMod,
                    ItemImage =
                       new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png"))
                });

                MostRecentlyUsedItems.Add(new Classes.FutureAccessItem()
                {
                    ItemName = file.Name,
                    ItemOpenedTime = lastMod,
                    ItemImage =
                       new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/icons8_File_40px.png"))
                });
            }
        }

        private async void btnFolderPicker_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();

            folderPicker.CommitButtonText = "Open";
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;

            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                var mru =  Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList;
                string mruToken = mru.Add(folder, "Adding a folder");

                var futureAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
                string falToken = futureAccessList.Add(folder, "Adding a folder");

                var basicsProperties = await folder.GetBasicPropertiesAsync();
                string lastMod = basicsProperties.DateModified.ToString();

                FutureAccessListItems.Add(new Classes.FutureAccessItem()
                {
                    ItemName = folder.Name,
                    ItemOpenedTime = lastMod,
                    ItemImage =
                      new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/Folder_40px.png"))
                });

                MostRecentlyUsedItems.Add(new Classes.FutureAccessItem()
                {
                    ItemName = folder.Name,
                    ItemOpenedTime = lastMod,
                    ItemImage =
                       new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Icons/ThirdPivotPictures/Folder_40px.png"))
                });
            }
        }

        private async void listMostRecentlyUsed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mru = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList;
            foreach (Windows.Storage.AccessCache.AccessListEntry mruEntry in mru.Entries)
            {
                string token = mruEntry.Token;
                string metadata = mruEntry.Metadata;
                Windows.Storage.IStorageItem item = await mru.GetItemAsync(token);
                if (item != null)
                {
                    if (item is StorageFile)
                    {
                        if (((StorageFile)item).Name == ((Classes.FutureAccessItem)listMostRecentlyUsed.SelectedItem).ItemName)
                        {
                            txtFutureAccessListFileName.Text = "File Name: " + ((StorageFile)item).DisplayName;
                            txtFutureAccessListMetadata.Text = "Metadata: " + metadata;
                            break;
                        }
                    }
                    else
                    {
                        if (((StorageFolder)item).Name == ((Classes.FutureAccessItem)listMostRecentlyUsed.SelectedItem).ItemName)
                        {
                            txtFutureAccessListFileName.Text = "Folder Name: " +  ((StorageFolder)item).DisplayName;
                            txtFutureAccessListMetadata.Text = "Metadata" + metadata;
                            break;
                        }
                    }
                }
            }
        }

        private async void listFutureAccessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var futureAccessList = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList;
            foreach (Windows.Storage.AccessCache.AccessListEntry futureAccessListEntry in futureAccessList.Entries)
            {
                string token = futureAccessListEntry.Token;
                string metadata = futureAccessListEntry.Metadata;
                Windows.Storage.IStorageItem item = await futureAccessList.GetItemAsync(token);
                if (item != null)
                {
                    if (item is StorageFile)
                    {
                        if (((StorageFile)item).Name == ((Classes.FutureAccessItem)listFutureAccessList.SelectedItem).ItemName)
                        {
                            txtFutureAccessListFileName.Text = "File Name: " + ((StorageFile)item).DisplayName;
                            txtFutureAccessListMetadata.Text = "Metadata: " + metadata;
                            break;
                        }
                    }
                    else
                    {
                        if (((StorageFolder)item).Name == ((Classes.FutureAccessItem)listFutureAccessList.SelectedItem).ItemName)
                        {
                            txtFutureAccessListFileName.Text = "Folder Name: " + ((StorageFolder)item).DisplayName;
                            txtFutureAccessListMetadata.Text = "Metadata: " + metadata;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
