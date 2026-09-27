using System.IO;
using System;

string sourcePath = @"C:\Liv_Synch_Loader\SourcePath";

string destinationPath = @"C:\Liv_Synch_Loader\DestinationPath";

string cloudPath = @"C:\Users\livki\OneDrive\Liv_Synch_loader";

string sourceCloudBackup = sourcePath;

string destinationCloudBackup = destinationPath;

Console.WriteLine("Hello, welcome to Liv_Synch_Loader");

while (true){
    Console.WriteLine(" ");

    Console.WriteLine("Press A for synch A->B");
    Console.WriteLine("Press B for changing source & destination path");
    Console.WriteLine("Press C for automatic folder creation");
    Console.WriteLine("Press D for custom path");
    Console.WriteLine("Press E to swap synch direction");
    Console.WriteLine("Press F to cloud synchronization");
    Console.WriteLine("Press Q to exit program");
    Console.WriteLine(" ");
    Console.WriteLine("Current source path: " + sourcePath);
    Console.WriteLine("Current destination path: " + destinationPath);

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
            

            if (!File.Exists(destinationFile)){
                return true;
            }
    
            
        }
    
        return false;
    }

    bool sourceMismatch = synchStatus(sourcePath, destinationPath);
    bool destinationMismatch = synchStatus(destinationPath, sourcePath);

    try{
        if (sourceMismatch || destinationMismatch){
                Console.WriteLine("!!!Synch needed!!!");
    } else {
        Console.WriteLine("Folders are synched!");   
    }

    }
    catch{
        Console.WriteLine("Something unexpected happened");
    }


    string Synch = Console.ReadLine();
    
    switch (Synch){
        case "A":
            if (Directory.Exists(sourcePath) && Directory.Exists(destinationPath))
            {
                DateTime dt = Directory.GetCreationTime(sourcePath);
        
                string[] files = Directory.GetFiles(
                    sourcePath,
                    "*",
                    SearchOption.AllDirectories
                );
        
                Console.WriteLine(sourcePath + " created at " + dt + " time");
        
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
                            Console.WriteLine("File copied: " + filename);
                        }
                        else
                        {
                            DateTime dt_copy = File.GetLastWriteTimeUtc(destination);
        
                            if (dt_org != dt_copy)
                            {
                                Console.WriteLine("File out of synch: " + filename);
        
                                File.Delete(destination);
                                File.Copy(file, destination);
                            }
                            else
                            {
                                Console.WriteLine("File skipped: " + filename);
                            }
                        }
                    }
                    catch
                    {
                        string filename = Path.GetFileName(file);
                        Console.WriteLine("This file had a copy issue: " + filename);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(sourcePath);
                Directory.CreateDirectory(destinationPath);
            }
        
            break;
        
        case "B":
        Console.WriteLine("What path do you source to be?");
        sourcePath = Console.ReadLine();
        Console.WriteLine("New source path is: " + sourcePath);
        Console.WriteLine("What path do you destination to be?");
        destinationPath = Console.ReadLine();
        Console.WriteLine("New desitnation path is: " + destinationPath);
        break;
    
        case "C":
        sourcePath = @"C:\Liv_Synch_loader\WorldA";
        destinationPath = @"C:\Liv_Synch_loader\WorldB";
        Console.WriteLine("Autocreated these paths: " + sourcePath + " & " + destinationPath);
        break;
        
        case "D":
        sourcePath = @"C:\Eksamensbevis";
        Console.WriteLine("Custom path enabled: " + sourcePath);
        break;
        
        case "E":
        Console.WriteLine("Old order is from " + sourcePath + " to " + destinationPath);
        string backupSource = sourcePath;
        string backupDestination = destinationPath;
        sourcePath = backupDestination;
        destinationPath = backupSource;
        
        Console.WriteLine("Synch direction has reversed!");
        Console.WriteLine("BackupSource is: " + backupSource + " while newSource is: " + sourcePath);
        Console.WriteLine("BackupSource is: " + backupDestination + " while newSource is: " + destinationPath);
        break;

        case "F":
        Console.WriteLine("Transfer or Retrieve?");
        string Answer = Console.ReadLine();
 
        if (Answer == "Transfer"){
            sourcePath = sourceCloudBackup;
            destinationPath = cloudPath;
            Console.WriteLine("Cloud transfer is enabled: " +  destinationPath);
        } else if (Answer == "Retrieve") {
            Console.WriteLine("RETRIEVE SOURCE: " + sourcePath);
            Console.WriteLine("RETRIEVE DESTINATION: " + destinationPath);
            destinationPath = destinationCloudBackup;
            sourcePath = cloudPath;
            Console.WriteLine("Cloud retrival is enabled: " +  destinationPath); 
        } else {
            break;
        }
        
        break;

        case "Q":
        return;
        break;
    
    }
}