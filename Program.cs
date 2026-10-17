using System.IO;
using System;


SynchManager syncManager = new SynchManager();

Console.WriteLine("Hello, welcome to Liv_Synch_Loader");

syncManager.IntroDialogue();

while (true){
    syncManager.AutoPathCreator();
    syncManager.AutoCreateBackupFolder();
    Console.WriteLine(" ");
    syncManager.PathInfo();
    Console.WriteLine(" ");
    syncManager.SyncStatus();
    syncManager.AutoBackupDetector();
    
    string Synch = Console.ReadLine();
    
    switch (Synch){
        case "A":
        syncManager.SyncFiles();
        break;
        
        case "B":
        syncManager.CustomPath();
        break;
    
        case "C":
        syncManager.SynchToBackup();
        break;
        
        case "D":
        syncManager.PremadePath();
        break;
        
        case "E":
        syncManager.DirectionSwap();
        break;

        case "F":
        syncManager.CloudPath();     
        break;

        case "G":
        syncManager.IntroDialogue();
        break;

        case "H":
        syncManager.EnableSpam();
        break;

        case "Q":
        syncManager.EmergencySave();
        return;
        break;
    
    }
}
    public class SynchManager{
        
        string cloudPath;

        string OneDrivePath;

        string sourcePath;

        string destinationPath;
        
        string sourceCloudBackup;
        
        string destinationCloudBackup;

        string localAppData;
        
        string backupPath;

        string emergencyPath;

        string lastTime_emergencyPath;

        string programName = "Liv_Synch_Loader";

        bool x = false;

        public SynchManager(){
        localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        backupPath = Path.Combine(localAppData, programName, "Backup");
        sourcePath = Path.Combine(localAppData, programName, "userSource");
        destinationPath = Path.Combine(localAppData, programName, "userDestination");
        emergencyPath = Path.Combine(localAppData, programName, "emergencyPath");
        OneDrivePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        cloudPath = Path.Combine(OneDrivePath, programName, "userOneDrive");
        }
    
        public void SyncManager(){
            sourceCloudBackup = sourcePath;
            destinationCloudBackup = destinationPath;
        }

        public bool EnableSpam(){
            if (x == true){
                x = false;
                return false;
            } else {
                x = true;
                return true;
            }      
        }
        
        
        public void SyncFiles(){
                   
            if (Directory.Exists(sourcePath) && Directory.Exists(destinationPath))
            {
                DateTime dt = Directory.GetCreationTime(sourcePath);
        
                string[] files = Directory.GetFiles(
                    sourcePath,
                    "*",
                    SearchOption.AllDirectories
                );

                if (x == true){
                    Console.WriteLine(sourcePath + " created at " + dt + " time");
                }
        
                foreach (string file in files)
                {
                    try
                    {
                        string filename = Path.GetRelativePath(sourcePath, file);
                        string destination = Path.Combine(destinationPath, filename);
                        string destinationFolder = Path.GetDirectoryName(destination);
                        DateTime dt_org = File.GetLastWriteTimeUtc(file);
 
                        if (!Directory.Exists(destinationFolder))
                        {
                            Directory.CreateDirectory(destinationFolder);
                        }

                           
                        if (!File.Exists(destination))
                        {
                            File.Copy(file, destination);
                            if (x == true){
                            Console.WriteLine("File copied: " + filename);
                            }
                        }
                        else
                        {
                            DateTime dt_copy = File.GetLastWriteTimeUtc(destination);
        
                            if (dt_org > dt_copy)
                            {
                                if (x == true){
                                Console.WriteLine("Source is newer: " + filename);
                                }
        
                                File.Delete(destination);
                                File.Copy(file, destination);
                            }
                            else if (dt_org < dt_copy && (x == true))
                            {
                                Console.WriteLine("Destination is newer: " + filename);
                                Console.WriteLine("Risk of overwriting!!! ");
        
                            }
                            else
                            {
                                if (x == true){
                                Console.WriteLine("File skipped: " + filename);
                                }
                            }
                        }
                    }
                    catch
                    {
                        string filename = Path.GetFileName(file);
                        if (x == true){
                        Console.WriteLine("This file had a copy issue: " + filename);
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(sourcePath);
                Directory.CreateDirectory(destinationPath);
            }
        }

        public void CustomPath(){
            Console.WriteLine("What path do you source to be?");
            sourcePath = Console.ReadLine();
            Console.WriteLine("New source path is: " + sourcePath);
            Console.WriteLine("What path do you destination to be?");
            destinationPath = Console.ReadLine();
            Console.WriteLine("New desitnation path is: " + destinationPath);
        }

        public void PremadePath(){
            sourcePath = @"C:\Eksamensbevis";
            Console.WriteLine("Custom path enabled: " + sourcePath);
        }

        public void DirectionSwap(){
            if (x == true){
            Console.WriteLine("Old order is from " + sourcePath + " to " + destinationPath);
            }
            string backupSource = sourcePath;
            string backupDestination = destinationPath;
            sourcePath = backupDestination;
            destinationPath = backupSource;
            if (x == true){
            Console.WriteLine("Synch direction has reversed!");
            Console.WriteLine("BackupSource is: " + backupSource + " while newSource is: " + sourcePath);
            Console.WriteLine("BackupSource is: " + backupDestination + " while newSource is: " + destinationPath);
            }
        }

        public void SynchToBackup(){
            destinationPath = backupPath;
            SyncFiles();
        }

        public void CloudPath()
        {
            Console.WriteLine("Transfer or Retrieve?");
            SyncManager();
            string answer = Console.ReadLine();
            
        
            if (answer == "Transfer")
            {
                sourcePath = sourceCloudBackup;
                destinationPath = cloudPath;
            }
            else if (answer == "Retrieve")
            {
                sourcePath = cloudPath;
                destinationPath = destinationCloudBackup;
            }
        }

        public void PathInfo(){
            Console.WriteLine("Current source path: " + sourcePath);
            Console.WriteLine("Current destination path: " + destinationPath);
        }

        public void SyncStatus(){
            bool synchStatus(string sourcePath, string destinationPath){
                string[] files = Directory.GetFiles(
                    sourcePath,
                    "*",
                    SearchOption.AllDirectories
                );
            
                foreach (string file in files)
                {
                    string relativePath = Path.GetRelativePath(sourcePath, file);
                    string destinationFile = Path.Combine(destinationPath, relativePath);

                    string fileName = Path.GetFileName(file);
                    
                    if (fileName.StartsWith("~$"))
                    {
                        continue;
                    }
                    
                    if (!File.Exists(destinationFile))
                    {
                        if (x == true){
                        Console.WriteLine(" ");
                        Console.WriteLine("Mismatch: " + relativePath + " located at: " + destinationFile);
                        }
                    
                        return true;
                    }
                
                            
        
                    if (!File.Exists(destinationFile)){
                        return true;
                    }
            
                    
                }
            
                return false;
            }
        
            bool sourceMismatch = synchStatus(sourcePath, destinationPath);
            bool destinationMismatch = synchStatus(destinationPath, sourcePath);


            if (sourcePath == null || destinationPath == null)
            {
                Console.WriteLine("A path has not been configured.");
                return;
            }
        
            try{
                if (sourceMismatch || destinationMismatch){
                        Console.WriteLine("!!!Synch needed!!!");
            } else {
                if (x == true){
                Console.WriteLine("Folders are synched!");  
               } 
            }
            
        
            }
            catch{
                if (x == true){
                Console.WriteLine("Something unexpected happened");
                }
            }
        }

        public void AutoPathDetector(){
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if(sourcePath != Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)){
                Console.WriteLine("User path detected: {0}, do you want to change it?",
                                 Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                Console.WriteLine("Yes or No");
                string answer_AutoPathDetector = Console.ReadLine();
                if(answer_AutoPathDetector == "Yes"){
                    sourcePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                } else {
                    Console.WriteLine("No path is changed");
                }
        }

    }

        public void AutoPathCreator(){
            if (!Directory.Exists(sourcePath)){
                Directory.CreateDirectory(sourcePath);
            }  

            if (!Directory.Exists(destinationPath)){
                Directory.CreateDirectory(destinationPath);        
            }
        }

        public void AutoCreateBackupFolder(){
            if(!Directory.Exists(backupPath)){
                Directory.CreateDirectory(backupPath);
                Console.WriteLine("Backup folder ready: {0}", backupPath);
            }
        }

        public void EmergencySave(){
            if(!Directory.Exists(emergencyPath)){
                Directory.CreateDirectory(emergencyPath);
                Console.WriteLine("Backup folder ready: {0}", emergencyPath);   
            }
            string overridePath = destinationPath;
            destinationPath = emergencyPath;
            SyncFiles();
            destinationPath = overridePath;
            DateTime lastTime_emergencyPath = DateTime.Now;
         
        }

        public void TimeTracker(){
            DateTime saveNow = DateTime.Now;
            string old_time_stamp;
            old_time_stamp = $"{saveNow}";
            Console.WriteLine("Time: " + old_time_stamp + " when action done");
            Console.WriteLine("Emergency backup at : " + lastTime_emergencyPath);
        }

        public void IntroDialogue(){
            Console.WriteLine(" ");
        
            Console.WriteLine("Press A for synch A->B");
            Console.WriteLine("Press B for changing source & destination path");
            Console.WriteLine("Press C sync to backup immediately");
            Console.WriteLine("Press D for custom path");
            Console.WriteLine("Press E to swap synch direction");
            Console.WriteLine("Press F to cloud synchronization");
            Console.WriteLine("Press G to make options reappear");
            Console.WriteLine("Press Q to exit program");
            Console.WriteLine("Press H to toogle file info");
            Console.WriteLine(" "); 
        }


        public void AutoBackupDetector(){

            if (!Directory.Exists(sourcePath) && !Directory.Exists(backupPath)){
                Console.WriteLine("No paths detecteced between sourcePath and backupPath");
            }

            if (Directory.Exists(sourcePath) && Directory.Exists(backupPath)){      
                string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
                
                foreach (string file in files)
                {
                        string filename = Path.GetRelativePath(sourcePath, file);
                        string backupFilePath = Path.Combine(backupPath, filename);
                        string backupPathFolder = Path.GetDirectoryName(backupFilePath);
                        DateTime dt_org = File.GetLastWriteTimeUtc(file);
                        DateTime dt_copy = File.GetLastWriteTimeUtc(backupFilePath);

                        if(dt_org > dt_copy){
                            if (x == true){
                            Console.WriteLine("Backup is outdated");
                            }
                            break;
                        } else if (dt_org < dt_copy){
                            EmergencySave();
                            Console.WriteLine("Backup is newer");
                            Console.WriteLine("Risk of overwriting!!!");
                            break;
                        } else {
                            break;
                        }

                        
            }
            }
        }
}

