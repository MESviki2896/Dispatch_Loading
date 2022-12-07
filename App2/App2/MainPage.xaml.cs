using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.OS;
using Android.Content.PM;
using Android.Content;
using Java.IO;
using Android.App;
using Android.Runtime;
using System.Reflection;


namespace App2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var entry = new Entry { Text = "I am an Entry" };
            
        }
        public static List<string> Datalist=new List<string>();
        string DirPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
        string  textPath, DuplicatetextPath;
        private void LoadTextDB()
        {
            //AssetManager assets = ApplicationContext.Assets;
            //Stream stream = assets.Open(ConfigurationFilePath);
            //DirPath=System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            textPath = Path.Combine(DirPath, "FGSerialNumbers.txt");
            DuplicatetextPath = Path.Combine(DirPath, "DuplicateFGSerialNumbers.txt");

            //textPath = Path.Combine(Environment.GetExternalStoragePublicDirectory), "FGSerialNumbers.txt");
            // textPath = Xamarin.Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();
            if (!System.IO.File.Exists(textPath))
            {
                var value=System.IO.File.Create(textPath);
               
                return;
            }

            
            using (var str = new StreamReader(System.IO.File.OpenRead(textPath)))
            {
                Datalist = str.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            ReadFilePath();
        }

        private void ReadFilePath()
        {

           
        }

        void OnEntryCompleted(object sender, EventArgs e)
        {
            var value = ((Entry)sender);
            string text = value.Text;
            
            if (text.Contains(";"))
            {
                LoadTextDB();
                var ValueArray = text.Split(new char[] { ';' });
                if (ValueArray.Count() != 6) {
                    DisplayAlert("WAIT", "Scanned Barcode Improper Format", "Close");
                       return; 
                };
                ModelResult.Text = ValueArray[0];
                SRNOResult.Text = ValueArray[4];
                AssetResult.Text = ValueArray[5];

               string FGSerial=ValueArray[4].ToString();
                if(Datalist.Count==0)
                {
                    Datalist.Add(ValueArray[0] + ";" + ValueArray[4]);
                    using (var StreamValue = System.IO.File.AppendText(textPath))
                    {
                        StreamValue.WriteLine(ValueArray[0] + ";" + ValueArray[4] + "\r");
                    }

                    if (!System.IO.File.Exists(Path.Combine(DirPath, "TEMPFGSerialNumbers.txt")))
                    {
                        using (var stream = new StreamWriter(System.IO.File.Create(Path.Combine(DirPath, "TEMPFGSerialNumbers.txt"))))
                        {
                            stream.WriteLine(ValueArray[0] + ";" + ValueArray[4] + ";" + ValueArray[5]);
                        }
                    }
                    
                }
               else if (!Datalist.Contains(ValueArray[0] + ";" + ValueArray[4]))
                {
                    using (var StreamValue = System.IO.File.AppendText(textPath))
                    {
                        StreamValue.WriteLine(ValueArray[0] + ";" + ValueArray[4]+"\r");
                    }
                    using (var StreamValue = new StreamWriter(System.IO.File.AppendText(Path.Combine(DirPath, "TEMPFGSerialNumbers.txt")).BaseStream))
                    {
                        StreamValue.WriteLine(ValueArray[0] + ";" + ValueArray[4] + ";" + ValueArray[5]);
                    }
                }
                else
                {
                    if (System.IO.File.Exists(DuplicatetextPath))
                    {
                        using (var StreamValue = new StreamWriter(System.IO.File.AppendText(DuplicatetextPath).BaseStream))
                        {
                            StreamValue.WriteLine(ValueArray[0] + ";" + ValueArray[4] + ";" + ValueArray[5]);
                        }
                    }
                    DisplayAlert("WAIT", "Barcode already Exists", "Close");
                }

               
                //Clearing and Focusing
                TEntry.Text = "";

                 TEntry.Focus();
               // value.Text.Remove(0,value.Text.Length);
            }
            else
            {
                DisplayAlert("WAIT", "This is not a Valid Barcode", "Close");
            }
        }
        void ExportFile(object sender,EventArgs e)
        {
            if(System.IO.File.Exists(Path.Combine(DirPath, "TEMPFGSerialNumbers.txt")))
            {
                var value = new FileInfo(Path.Combine(DirPath, "TEMPFGSerialNumbers.txt")).Length;
                if (value > 0)
                {
                    System.IO.File.Move(Path.Combine(DirPath, "TEMPFGSerialNumbers.txt"), Path.Combine(DirPath, "FGSerialNumbers" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt"));
                    DisplayAlert("Done", "File Exported Successfully", "Close");
                }
            }
                
        }
        void OnEntryReturn(object sender,EventArgs e)
        {

        }
       
    }


}
