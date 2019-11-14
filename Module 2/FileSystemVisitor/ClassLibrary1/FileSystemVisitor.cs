using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitor
{
    public class FileSystemVisitor
    {
        private readonly string searchPatternDir;
        private readonly string searchPatternFile;
        private readonly bool stopSearch;
        private readonly bool ignoreFile;

        public delegate void UserAction();

        public FileSystemVisitor(/*string searchPatternDir, string searchPatternFile, string stopSearch, string ignoreFile*/)
        {
            //this.searchPatternDir = searchPatternDir;
            //this.searchPatternFile = searchPatternFile;

            //if (ignoreFile == "+") this.ignoreFile = true;
            //else this.ignoreFile = false;

            //if (stopSearch == "+") this.stopSearch = true;
            //else this.stopSearch = false;
        }

        public FileSystemVisitor(UserAction deleg)
        {
            deleg?.Invoke();
        }

        public delegate void Notification(string typeOfAction);
        public event Notification OnStart;
        public event Notification OnFinish;
        public event Notification OnFileFinded;
        public event Notification OnDirectoryFinded;
        public event Notification OnFilteredFileFinded;
        public event Notification OnFilteredDirectoryFinded;

        private static FileInfo[] SearchFiles(DirectoryInfo dir, string searchPatternFile) => dir.GetFiles(searchPatternFile, SearchOption.AllDirectories);
        private static DirectoryInfo[] SearchDirectories(DirectoryInfo dir, string searchPatternDir) => dir.GetDirectories(searchPatternDir, SearchOption.AllDirectories);

        public void Searching(string startPos)
        {
            var notContinue = false;
            List<string> listOfDir = new List<string>();
            List<string> listOfFile = new List<string>();
            StartSearch();
            DirectoryInfo dir = new DirectoryInfo(startPos);
            foreach (var item in SearchDirectories(dir, searchPatternDir))
            {
                listOfDir.Add(item.Name);
                foreach (var element in SearchDirectories(dir, searchPatternDir))
                    listOfDir.Add(element.Name);
            }
            foreach (var item in SearchFiles(dir, searchPatternDir))
                listOfFile.Add(item.Name);

            foreach (var item in SearchDirectories(dir, "*"))
            {
                if (listOfDir.IndexOf(item.Name) != -1)
                {
                    if (ignoreFile == false)
                    {
                        FindADirectory(item.Name);
                        FindAFilteredDirectory(item.Name);
                        //Console.WriteLine(item.Name);
                    }
                }
                else
                {
                    FindADirectory(item.Name);
                    //Console.WriteLine(item.Name);
                }
                if (stopSearch)
                {
                    notContinue = true;
                    break;
                }
            }
            if (!notContinue)
                foreach (var item in SearchFiles(dir, "*"))
                {
                    FindAFile(item.Name);
                    if (listOfDir.IndexOf(item.Name) != -1) FindAFilteredFile(item.Name);
                    Console.WriteLine(item.Name);
                    if (stopSearch) break;
                }
            FinishSearch();
        }

        private void StartSearch()
        {
            OnStart?.Invoke("Start");
        }
        private void FinishSearch()
        {
            OnFinish?.Invoke("Finish");
        }
        private void FindAFile(string fileName)
        {
            OnFileFinded?.Invoke("FileFinded: " + fileName);
        }
        private void FindADirectory(string dirName)
        {
            OnDirectoryFinded?.Invoke("DirectoryFinded" + dirName);
        }
        private void FindAFilteredFile(string fileName)
        {
            OnFilteredFileFinded?.Invoke("FilteredFileFinded" + fileName);
        }
        private void FindAFilteredDirectory(string dirName)
        {
            OnFilteredDirectoryFinded?.Invoke("FilteredDirectoryFinded" + dirName);
        }
    }
}
