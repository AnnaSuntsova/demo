// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace NUnit.Tests1
{
    [TestFixture]
    public class TestClass
    {       
        private int countOfEvents;
        private int countOfFilteredFiles;

        private static bool FindAFile(string itemName)
        {
            if (itemName.IndexOf("34.txt") != -1) return true;
            else return false;
        }

        private void _systemVisitor_OnFinish(object sender, System.EventArgs e)
        {
            countOfEvents++;
        }

        private void _systemVisitor_OnStart(object sender, System.EventArgs e)
        {
            countOfEvents++;
        }

        private void SystemVisitor_OnFilteredFileFinded(string itemName, FileSystemVisitor.FileSystemVisitor.CharachteristicsOfItems arg)
        {
            arg.cancelSearch = true;
            arg.excludeItem = false;
            countOfEvents++;
            countOfFilteredFiles++;
        }

        private void SystemVisitor_OnFilteredDirectoryFinded(string itemName, FileSystemVisitor.FileSystemVisitor.CharachteristicsOfItems arg)
        {
            arg.cancelSearch = true;
            arg.excludeItem = false;
            countOfEvents++;
            countOfFilteredFiles++;
        }

        private void SystemVisitor_OnDirectoryFinded(object sender, string e)
        {
            countOfEvents++;
        }

        private void SystemVisitor_OnFileFinded(object sender, string e)
        {
            countOfEvents++;
        }

        [SetUp]
        public void PrepareEnvironment()
        {
            Directory.CreateDirectory(@"D:\Info");
            Directory.CreateDirectory(@"D:\Info\Folder1");
            Directory.CreateDirectory(@"D:\Info\Folder1\Folder2");

            FileStream file1 = new FileStream(@"D:\Info\12.txt", FileMode.CreateNew);
            file1.Close();
            file1= new FileStream(@"D:\Info\Folder1\34.txt", FileMode.CreateNew);
            file1.Close();
            file1 = new FileStream(@"D:\Info\Folder1\56.txt", FileMode.CreateNew);
            file1.Close();
        }

        [Test]
        public void SearchWithoutFilters()
        {
            var countOfFile = 0;
            FileSystemVisitor.FileSystemVisitor _systemVisitor = new FileSystemVisitor.FileSystemVisitor(@"D:\Info");
            IEnumerable str=_systemVisitor.Searching();
            foreach (var item in str)
                countOfFile++;
            Assert.AreEqual(5, countOfFile);
        }

        [Test]
        public void SearchFileWith34txt()
         {
            countOfFilteredFiles = 0;
            FileSystemVisitor.FileSystemVisitor _systemVisitor = new FileSystemVisitor.FileSystemVisitor(@"D:\Info", FindAFile);
            _systemVisitor.OnFilteredDirectoryFinded += SystemVisitor_OnFilteredDirectoryFinded;
            _systemVisitor.OnFilteredFileFinded += SystemVisitor_OnFilteredFileFinded;
            IEnumerable str = _systemVisitor.Searching();
            Assert.AreEqual(1, countOfFilteredFiles);
        }

        [Test]
        public void CheckOfEventsCountWithoutFilters()
        {
            countOfEvents = 0;
            FileSystemVisitor.FileSystemVisitor _systemVisitor = new FileSystemVisitor.FileSystemVisitor(@"D:\Info");
            _systemVisitor.OnStart += _systemVisitor_OnStart;
            _systemVisitor.OnFinish += _systemVisitor_OnFinish;
            _systemVisitor.OnFileFinded += SystemVisitor_OnFileFinded;
            _systemVisitor.OnDirectoryFinded += SystemVisitor_OnDirectoryFinded;
            _systemVisitor.OnFilteredDirectoryFinded += SystemVisitor_OnFilteredDirectoryFinded;
            _systemVisitor.OnFilteredFileFinded += SystemVisitor_OnFilteredFileFinded;
            IEnumerable str = _systemVisitor.Searching();
            Assert.AreEqual(7, countOfEvents);
        }        

        [TearDown]
        public void FinalizeTest()
        {
            Directory.Delete(@"D:\Info", true);
        }
    }
}
