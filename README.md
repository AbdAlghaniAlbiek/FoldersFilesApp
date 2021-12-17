# FoldersFilesApp
<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/FoldersFilesApp/blob/master/FoldersFilesApp/Assets/FilesFoldersAppIcon.png"/>
</p>

![Twitter Follow](https://img.shields.io/twitter/follow/AbdAlbiek?style=social) ![GitHub](https://img.shields.io/github/license/AbdAlghaniAlbiek/FoldersFilesApp)

## Table of content
* [General Info](#general-information)
* [Technologies Used](#uwp-apis-had-used)
* [Features](#features)
* [Screenshots](#screenshots)
* [Setup](#setup)
* [Project Status](#project-status)


## General Information![FilesFoldersAppIcon](https://user-images.githubusercontent.com/37735487/146604597-081d2424-11af-471a-898b-4483f78a9cfd.png)![FilesFoldersAppIcon](https://user-images.githubusercontent.com/37735487/146604593-57f3d77b-af77-4ffb-a241-8d6aa6d70cdc.png)![FilesFoldersAppIcon](https://user-images.githubusercontent.com/37735487/146604600-3cae45a2-5060-46d5-94ca-40e3426c40c3.png)![FilesFoldersAppIcon](https://user-images.githubusercontent.com/37735487/146604598-d70292d6-b3e4-43e9-8a0c-314bc4ed50e4.png)




This is a UWP application and had used for demoing purposes and showing how to use Windows.Storage APIs and how to deal with them

## UWP APIs had Used
Many Windows.Storage APIs are used here like:
* StorageFile
* StorageFolder
* OpenFilePicker
* SaveFilePicker
* MostRecentlyUsed
* FutureAccessList 
* LocalFolder
* KnownFolder (like: PicturesLibrary, SavedPictures, CameraRoll ...)

## Features
1. Creating, Updating or Deleting `.txt` files and reading of its properties like (name, description, date taken and size ...etc) and these files inside Local folder of the app.
2. Save a file with `.txt` format in specified location using SaveFilePicker.
3. Open folders and files with any format and read therir properties.
4. Dealing with MostRecentlyUsed and FutureAccessList and you can see the difference between them from [here.](https://docs.microsoft.com/en-us/windows/uwp/files/how-to-track-recently-used-files-and-folders)
5. Can this app get files (in most cases images) from Pictures folder directly, because it has the permissions to access to this folder and its subfolders like (CameraRoll, SavedPictures and Screenshots).

## Screenshots
> You can see all screenshots from [here.](https://github.com/AbdAlghaniAlbiek/FoldersFilesApp/tree/master/FoldersFilesApp/Assets/Screenshots)
<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/FoldersFilesApp/blob/master/FoldersFilesApp/Assets/Screenshots/Create%2CRead%2CWriteFile.jpg"/>
</p>

<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/FoldersFilesApp/blob/master/FoldersFilesApp/Assets/Screenshots/PropertiesFile.jpg"/>
</p>


## Setup
* Visual Studio 2019 at least.
* Windows 10 OS, Version: Fall Creator Update (16299) and above.
* Windows 10 Fall Creator (16299) SDK.
* There is no dependecies to install them. 😊

## Version
**version [1.0.0]**
* Is the stable version.
* All the features that descriped above is in this version.

## Project status
This project is `no longer being worked on` but the contributions are still welcome.

