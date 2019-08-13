using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace SecurityDirectory_ServerConsole
{
    class Program
    {
        static string PATH = @"E:\Test";

        // JSON string
        static string jsnFolder;

        // After filling this list, convert to a JSON string.
        static List<FolderAndFiles> listFolderAndFiles = new List<FolderAndFiles>();
        static void Main(string[] args)
        {
            // Main Folder
            listFolderAndFiles.Add(new FolderAndFiles() {
                Name = "Root", ImageIndex = 0, ParentID = 0, IsFolder = true
            });

            LoadSubDirectory(PATH, 1);
            LoadFiles(PATH, 1);

            // Converting list to JSON string
            jsnFolder = JsonConvert.SerializeObject(listFolderAndFiles);
            //Console.WriteLine(jsnFolder);


            
            /*
             * Now convert back to list
             */

            // Clear items list 
            listFolderAndFiles.Clear();

            listFolderAndFiles = JsonConvert.DeserializeObject<List<FolderAndFiles>>(jsnFolder);
            DrawTreeView();

            Console.ReadLine();
        }


        /// <summary>
        /// Load directory folders
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="parent_id">Directory ID</param>
        static void LoadSubDirectory(string path, int parent_id)
        {
            foreach (string sub_path in Directory.GetDirectories(path))
            {
                DirectoryInfo di = new DirectoryInfo(sub_path);

                FolderAndFiles folderAndFiles = new FolderAndFiles() {
                    Name = di.Name,
                    IsFolder = true,
                    ImageIndex = 1,
                    ParentID = parent_id
                };

                listFolderAndFiles.Add(folderAndFiles);

                LoadSubDirectory(di.FullName, folderAndFiles.ID);
                LoadFiles(di.FullName, folderAndFiles.ID);
            }
        }


        /// <summary>
        /// Load directory files
        /// </summary>
        /// <param name="path">Directory path</param>
        /// <param name="parent_id">Directory ID</param>
        static void LoadFiles(string path, int parent_id)
        {
            foreach (string file_path in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(file_path);

                FolderAndFiles folderAndFiles = new FolderAndFiles() {
                    Name = fi.Name,
                    IsFolder = false,
                    ImageIndex = FromTypeToImageIndex(fi.Extension),
                    ParentID = parent_id
                };

                listFolderAndFiles.Add(folderAndFiles);
            }
        }


        /// <summary>
        /// Set image index to files
        /// </summary>
        /// <param name="type">File extension</param>
        /// <returns>Image index</returns>
        static int FromTypeToImageIndex(string type)
        {
            try
            {
                switch (type)
                {
                    case ".txt": return 10;
                    case ".png": return 9;
                    case ".mp3": return 8;
                    case ".zip": return 13;
                    case ".rar": return 12;
                    case ".avi":
                    case ".3gp":
                    case ".mpeg":
                    case ".mp4": return 11;
                    case ".jpg":
                    case ".jpeg": return 3;
                    case ".accdb":
                    case ".accde":
                    case ".accdt":
                    case ".accdr": return 4;
                    case ".ppt":
                    case ".pot":
                    case ".pps":
                    case ".pptx":
                    case ".pptm":
                    case ".potx":
                    case ".potm":
                    case ".ppam":
                    case ".ppsx":
                    case ".ppsm":
                    case ".sldx":
                    case ".sldm": return 6;
                    case ".xls":
                    case ".xlt":
                    case ".xlm":
                    case ".xlsx":
                    case ".xlsm":
                    case ".xltx":
                    case ".xltm": return 5;
                    case ".doc":
                    case ".dot":
                    case ".wbk":
                    case ".docx":
                    case ".docm":
                    case ".dotx":
                    case ".dotm":
                    case ".docb": return 7;
                    default: return 2;
                }
            }
            catch
            {
                return 2;
            }

        }


        /// <summary>
        /// Draw Tree View on Console
        /// </summary>
        static void DrawTreeView()
        {
            foreach (FolderAndFiles item in listFolderAndFiles)
            {
                if (item.IsFolder)
                {
                    Console.WriteLine(item.Name);

                    foreach (FolderAndFiles sub_item in listFolderAndFiles)
                    {
                        if (item.ID == sub_item.ParentID)
                        {
                            Console.WriteLine("\t" + sub_item.Name);
                        }
                    }
                }
            }
        }
    }

    class FolderAndFiles
    {
        public static int ID_sequence = default(int);

        public int ID;
        public string Name;
        public bool IsFolder;
        public int ImageIndex;
        public int ParentID;

        public FolderAndFiles()
        {
            this.ID = ++ID_sequence;
        }
    }
}
