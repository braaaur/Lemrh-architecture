/*
 * Jakub Bukała
 * 
 * Created: 13.01.2018 r.
 * 
 * Edited: 11.04.2023 r.
 *  
 * [LemrhSaveGame]
 * 
 * in LEMRH framework > VP
 * 
 */

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine.Events;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    //public class LemrhSaveGame : SerializedMonoBehaviour //does both config and saving on slots
    public class LemrhSaveGame : MonoBehaviour //does both config and saving on slots
    {
        private const string configFileName = "/config.dat";
        private const string slotsDirectorySufix = "/Slots/";
        private const string slotsFileSufix = ".slo";
        
        private string[] currentSlotFiles = new string[5]; //no of slots

        private static BinaryFormatter binaryFormatter;
        private static FileStream fileStream;
        private static string slotSaveDate;

        [Header("Config")] //single ISavables to avoid prefab changes, just ISavable collectors nesting
        [SerializeField] private ISavable configCollector; 

        [Header("Save")] //single ISavables to avoid prefab changes, just ISavable collectors nesting
        [SerializeField] private ISavable saveCollector; //all slot

        [Header("Events")]
        [SerializeField] private UnityEvent configSaved;
        [SerializeField] private UnityEvent configLoaded;
        [SerializeField] private UnityEvent configResetedToDefault;  
        [SerializeField] private UnityEvent slotSaved;
        [SerializeField] private UnityEvent slotLoaded;

        [Header("Current values")]
        [SerializeField] private LemString currentSlotFileName;

        [Header("Language setters")]
        [SerializeField] private bool setSystemLanguage;
        [SerializeField] private ILemInt languageEntity;
        [SerializeField] private List<LanguageSet> languageSets;

        //not sure about the Awake, but should be Ok
        private void Awake()
        {
            Debug.Log("Awake() in LemrhSaveGame.cs");

            DirectoryCheck();
        
            //1st
            LoadConfig();

            //2nd
            LoadCurrentSlot(false); //true = createNewWhenFail (was true in the old system)
        }

        private void DirectoryCheck()
        {
            //Debug.Log("DirectoryCheck() in LemrhSaveGame.cs");

            if (!Directory.Exists(SlotsDirectory))
            {
                Directory.CreateDirectory(SlotsDirectory);
            }
        }

		#region Config
		
        //loads Config file
		//if not possible it calls CreateAndSaveConfig()
		private void LoadConfig()
        {
            Debug.Log("LoadConfig() in LemrhSaveGame.cs");

            if (File.Exists(ConfigDirectory))
            {
                Config toLoad = null;

                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    FileStream fileStream = File.Open(ConfigDirectory, FileMode.Open);

                    toLoad = (Config)binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                }
                catch
                {
                    Debug.LogError("Config deserialization error!");

                    ResetConfigToDefaultAndSave();
                    return; //ok?
                }

                if (toLoad != null)
                {
                    ApplyConfigValues(toLoad);
                }
            }
            else
            {
                Debug.LogWarning("Config not found. Creating one...");

                ResetConfigToDefaultAndSave();
            }
        }

        private void ApplyConfigValues(Config toApply)
        {
            try
            {
                configCollector.Load(toApply.saveEntities);
            }
            catch
            {
                Debug.LogError("Config applying error!");

                ResetConfigToDefaultAndSave();

                return; //blocks event below
            }

            Debug.Log("Config loaded successfuly in ApplyConfigValues() in LemrhSaveGame.cs");
            configLoaded.Invoke();
        }

        //Config does not exist, or is broken
        private void ResetConfigToDefaultAndSave()
        {
            Debug.Log("CreateAndSaveDefaultConfig() in LemrhSaveGame.cs");

            SetConfigDefaults();

            //get slots, maybe Config is missing/broken but slots are Ok?
            {
                GetSlots();
                int tempSlotIndex;

                if (GetValidSlotFile(out tempSlotIndex))
                {
                    CurrentSlotFileName = currentSlotFiles[tempSlotIndex];
                }
                else
                {
                    Debug.LogWarning("No valid slot files in Slots Directory...");

                    CurrentSlotFileName = null;
                    SetSlotDefaults(); //ok? this is important since it will set Save_IsCurrentRunActive to false

                    //CurrentSlotFileName = GetSlotFileName(0); //0 default
                    //CreateAndSaveDefaultSlot();
                }
            }

            configResetedToDefault.Invoke();

            SaveConfig();
        }

        private bool GetValidSlotFile(out int validSlotIndex)
        {
            for (int i = 0; i < currentSlotFiles.Length; i++)
            {
                if (currentSlotFiles[i] != null && File.Exists(SlotsDirectory + currentSlotFiles[i]))
                {
                    validSlotIndex = i;
                    return true;
                }
            }

            //if not
            validSlotIndex = 0;
            return false;
        }

        public void SetConfigDefaults()
        {
            Debug.Log("SetConfigDefaults() in LemrhSaveGame.cs");

            configCollector.Reset(); //defaults

            //setting default language
            if (setSystemLanguage)
            {
                if (languageEntity != null)
                {
                    Debug.Log("SystemLanguage = " + Application.systemLanguage.ToString());

                    foreach (LanguageSet lanSet in languageSets)
                    {
                        if (lanSet.systemLanguage == Application.systemLanguage)
                        {
                            languageEntity.IntValue = lanSet.entityInt;
                            return;
                        }
                    }

                    //if not found above then... set default
                    languageEntity.IntValue = 0;
                }
            }
        }

        //saves SlotConfig file
        private void SaveConfig()
        {
            Debug.Log("SaveConfig() in LemrhSaveGame.cs");

            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = File.Create(ConfigDirectory);

                Config toSave = new Config();

                toSave.saveEntities = configCollector.Save();

                binaryFormatter.Serialize(fileStream, toSave);
                fileStream.Close();
            }
            catch
            {
                Debug.LogError("Save Config error!");

                //any solution?

                return; //blocks event below
            }

            configSaved.Invoke();
        }

        public void SaveConfigEventCall() //event call
        {
            Debug.Log("SaveConfigEventCall() in LemrhSaveGame.cs");

            SaveConfig();
        }

        #endregion

        #region Slots

        private bool LoadCurrentSlot(bool createNewWhenFail)
        {
            Debug.Log("LoadCurrentSlot() in LemrhSaveGame.cs");

            Slot toLoad = null;

            if (CurrentSlotFileName != null)
            {
                toLoad = LoadSlot(CurrentSlotFilePath, createNewWhenFail, out slotSaveDate);
            }

            if (toLoad != null)
            {
                //if (toLoad.saveEntities != null) //when?
                //{
                try
                {
                    saveCollector.Load(toLoad.saveEntities);
                }
                catch
                {
                    Debug.LogError("Slot entities applying error!");

                    if (createNewWhenFail)
                    {
                        Debug.Log("createNewWhenFail == true, thus creating default one...");

                        CreateAndSaveDefaultSlot();
                    }

                    return false;
                }
                //}
                //else
                //{
                //    Debug.LogWarning("currentSlot.saveEntities == null in LoadCurrentSlot() in LemrhSaveGame.cs");
                //
                //    return false;//?
                //}

                slotLoaded.Invoke();
                Debug.Log("Slot loaded successfuly in LoadCurrentSlot() in LemrhSaveGame.cs");

                return true;
            }
            else
            {
                //Debug.LogError("currentSlot == null in LoadCurrentSlot() in LemrhSaveGame.cs");
                //it is not an error since toLoad == null when CreateAndSaveDefaultSlot();
                //it doesn't matter (probably)

                SetSlotDefaults(); //ok? this is important since it will set Save_IsCurrentRunActive to false

                return false;
            }
        }

        private Slot LoadSlot(string path, bool createNewWhenFail, out string slotSaveDate)
        {
            Slot toReturn = null;
            slotSaveDate = "";

            if (File.Exists(path))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();

                    FileStream fileStream = File.Open(path, FileMode.Open);

                    slotSaveDate = File.GetLastWriteTime(path).ToString(); //formatting?
                    toReturn = (Slot)binaryFormatter.Deserialize(fileStream);
                    fileStream.Close();
                }
                catch
                {
                    Debug.LogError("Slot deserialization error!");

                    if (createNewWhenFail)
                    {
                        CreateAndSaveDefaultSlot();
                    }
                }
            }
            else
            {
                Debug.LogError("Slot file not found at " + path);

                if (createNewWhenFail)
                {
                    Debug.Log("Creating default one...");

                    CreateAndSaveDefaultSlot();
                }
                else
                {
                    Debug.Log("Reseting state...");

                    SetSlotDefaults();
                }
            }

            return toReturn;
        }

        //addon
        public Slot LoadSlotByName(string fileName, bool createNewWhenFail, out string slotDateString)
        {
            //Debug.Log("LoadSlotByName() - fileName = " + fileName);

            return LoadSlot(SlotsDirectory + fileName, createNewWhenFail, out slotDateString);
        }

        //Called by event LoadSlotEvent (e.g. when quick loading)
        public void LoadCurrentSlot()
        {
            LoadCurrentSlot(false);
        }

        //creates default slot file and saves its name
        private void CreateAndSaveDefaultSlot()
        {
            Debug.Log("CreateAndSaveDefaultSlot() in LemrhSaveGame.cs");

            SetSlotDefaults();

            //isCurrentRunActive.BoolValue = false; //this SO should reset itself when SetSlotDefaults()

            //CurrentSlotFileName = GetFirstAvailableSlotId(); //new 

            if (SaveCurrentSlot())
            {
                SaveConfig(); //save slot file name to config
            }
        }

        public void SetSlotDefaults()
        {
            Debug.Log("SetSlotDefaults() in LemrhSaveGame.cs");

			saveCollector.Reset();
        }

        //[Button("Save Current Slot")]
        private bool SaveCurrentSlot()
        {
            Debug.Log("SaveCurrentSlot() in LemrhSaveGame.cs");

            try
            {
                binaryFormatter = new BinaryFormatter();
                fileStream = File.Create(CurrentSlotFilePath); //

                Slot toSave = new Slot();

                toSave.saveEntities = saveCollector.Save(); 

                binaryFormatter.Serialize(fileStream, toSave);
                fileStream.Close();
            }
            catch
            {
                Debug.LogError("SaveCurrentSlot() serialization error!");
                
                return false; //blocks event below
            }

            slotSaved.Invoke();
            return true;
        }

        public void SaveRunEventCall() //event call
        {
            //Debug.Log("SaveRunEventCall() in LemrhSaveGame.cs");

            SaveCurrentSlot();
        }

        //search for files in Slots Directory, checks them and polulates currentSlotFiles 
        //+ creates Default Slot when no slots are avaiable
        private void GetSlots()
        {
            Debug.Log("GetSlots() in LemrhSaveGame.cs");

            for (int i = 0; i < currentSlotFiles.Length; i++)
            {
                currentSlotFiles[i] = null;

                if (File.Exists(SlotsDirectory + GetSlotFileName(i)))
                {
                    string extension = Path.GetExtension(SlotsDirectory + GetSlotFileName(i));

                    //Debug.Log("extension = " + extension);

                    if (extension == slotsFileSufix)//split?
                    {
                        currentSlotFiles[i] = GetSlotFileName(i);
                    }
                }
            }

                        /* more elaboarte version, but it requires opening and deserialization of all slot files...
                        try
                        {
                            binaryFormatter = new BinaryFormatter();

                            fileStream = File.Open(fileEntry, FileMode.Open);

                            Slot toLoad = (Slot)binaryFormatter.Deserialize(fileStream);
                            fileStream.Close();

                            currentSlotFiles.Add(Path.GetFileName(fileEntry));
                        }
                        catch
                        {
                            Debug.LogError("Slot deserialization error! File: " + Path.GetFileName(fileEntry));
                        }
                        
            */
        }

        private void DeleteSlotFile(string givenSlotFileName)
        {
            File.Delete(SlotsDirectory + givenSlotFileName);
        }

        #endregion

        #region Run
        public void StartNewRun() //called from event listener - event NewRunStarts
        {
            Debug.Log("StartNewRun() - ResetAndSaveRunStatus in LemrhSaveGame.cs");

            SetRunDefaults();

            SaveCurrentSlot();
        }

        public void SetRunDefaults()
        {
            Debug.Log("SetRunDefaults() in LemrhSaveGame.cs");

			saveCollector.Reset();
        }
		#endregion

		#region Slots GUI
		public string[] GetCurrentSlotFiles()
        {
            GetSlots();

            return currentSlotFiles;
        }

        public bool IsCurrentSlot(string givenFileName)
        {
            if (givenFileName == CurrentSlotFileName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateAndSetNewSlot(int givenSlotIndex)
        {
            Debug.Log("CreateAndSetNewSlot() in LemrhSaveGame.cs");

            CurrentSlotFileName = GetSlotFileName(givenSlotIndex); //new

            CreateAndSaveDefaultSlot();
            //SaveConfig(); //already in CreateAndSaveDefaultSlot();
        }

        public bool SetSlot(string givenSlotFileName)
        {
            Debug.Log("SetSlot() in LemrhSaveGame.cs");

            string previousSlotFileName = CurrentSlotFileName;
            CurrentSlotFileName = givenSlotFileName;

            if (LoadCurrentSlot(false))
            {
                SaveConfig();
                return true;
            }
            else
            {
                CurrentSlotFileName = previousSlotFileName;
                return false;
            }
        }

        public bool ClearSlot(string givenSlotFileName)
        {
            Debug.Log("ClearSlot() in LemrhSaveGame.cs");

            SetSlotDefaults();

            CurrentSlotFileName = null;

            DeleteSlotFile(givenSlotFileName);
            SaveConfig();

            return true;
        }
        
        /* old
        public bool ClearAndSetSlot(string givenSlotFileName)
        {
            Debug.Log("ClearAndSetSlot() in LemrhSaveGame.cs");

            SetSlotDefaults();
            //SetRunDefaults(); //already in SetSlotDefaults();

            CurrentSlotFileName = givenSlotFileName;

            //isCurrentRunActive.BoolValue = false; //this SO should reset itself when SetSlotDefaults()

            SaveCurrentSlot();
            SaveConfig();

            return true;
        }
        */
        #endregion

        #region Utilities
        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }

        //Recursion used!
        public static ISave GetEntityISaveFromNested(ISave givenRoot, ISavable givenISavable)
        {
            if (givenRoot == null)
            {
                Debug.LogError("givenRoot == null in GetEntityISaveFromNested()");
                return null;
            }
            
            if (givenISavable == null)
            {
                Debug.LogError("givenISavable == null in GetEntityISaveFromNested()");
                return null;
            }

            if (givenISavable.Id == givenRoot.Id)
            {
                //Debug.Log("GetEntityISaveFromNested() - match found, returning...");
                return givenRoot;
            }
            else
            {
                LemListSave listSave = null;

                try
                {
                    listSave = (LemListSave)givenRoot;
                }
                catch
                {
                    //Debug.LogWarning("Cannot cast " + givenRoot.Id + " on LemCollector!");
                }

                if (listSave != null)
                {
                    //Debug.Log("GetEntityISaveFromNested() - it's a collector - " + listSave.Id);

                    foreach (ISave entity in listSave.listValue)
                    {
                        ISave tempEntity = GetEntityISaveFromNested(entity, givenISavable);

                        if (tempEntity != null)
                        {
                            return tempEntity;
                        }
                    }

                    //Debug.Log("GetEntityISaveFromNested() - end reached in - " + listSave.Id + ", thus returning null");
                    return null; //?
                }
                else
                {
                    //Debug.Log("GetEntityISaveFromNested() - it's NOT a collector - " + givenRoot.Id);
                    return null;
                }
            }
        }
        #endregion

        #region Paths
        private string CurrentSlotFileName
        {
            get
            {
                return currentSlotFileName.StringValue;
            }
            set
            {
                currentSlotFileName.StringValue = value;
            }
        }

        private string GetSlotFileName(int givenId)
        {
            return "S" + givenId.ToString() + slotsFileSufix;
        }
        
        private string ConfigDirectory
        {
            get
            {
                return Application.persistentDataPath + configFileName;
            }
        }

        private string CurrentSlotFilePath
        {
            get
            {
                return SlotsDirectory + CurrentSlotFileName;
            }
        }

        private string SlotsDirectory
        {
            get
            {
                return Application.persistentDataPath + slotsDirectorySufix;
            }
        }
        #endregion

        #region Clearing - not used
        public static void ClearDataFolder()
        {
            Debug.Log("ClearDataFolder()");

            ClearFolder(Application.persistentDataPath);
        }

        private static void ClearFolder(string folderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);

            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception e)
                {
                    Debug.Log("{0} Exception caught." + e.ToString());
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }
        #endregion
    }

    #region Support classes
    public interface ISavable
    {
        ISave Save();

        void Load(ISave givenValue);

        string Id
        {
            get;
        }

        void Reset();
    }

    public interface ISave
    {
        string Id
        {
            get;
        }
    }

    [Serializable]
    public class Slot // == RunStatus
    {
        public ISave saveEntities;
    }

    [Serializable]
    public class Config
    {
        public ISave saveEntities;
    }
    
    [Serializable]
    public struct LanguageSet
    {
        public SystemLanguage systemLanguage;
        public int entityInt;
    }
    #endregion

    #region Encryption
    public class Encryption
    {
        static readonly string secretKey = "Kowalski";
        readonly static byte[] Key = Encoding.UTF8.GetBytes(secretKey);
        readonly static byte[] IV = Encoding.UTF8.GetBytes(secretKey);

        static DESCryptoServiceProvider des;

        static ICryptoTransform desencrypt;

        public static Stream Encrypt(object dataObject)
        {
            Stream serializedStream = new MemoryStream();
            IFormatter formatterEn = new BinaryFormatter();
            formatterEn.Serialize(serializedStream, dataObject);

            serializedStream.Seek(0, SeekOrigin.Begin);

            MemoryStream memoryStream = new MemoryStream();

            byte[] byteArray = new byte[serializedStream.Length];

            serializedStream.Read(byteArray, 0, byteArray.Length);

            des = new DESCryptoServiceProvider
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = IV
            };

            desencrypt = des.CreateEncryptor();

            CryptoStream cryptStream = new CryptoStream(memoryStream, desencrypt, CryptoStreamMode.Write);

            cryptStream.Write(byteArray, 0, byteArray.Length);

            cryptStream.FlushFinalBlock();
            memoryStream.Seek(0, SeekOrigin.Begin);
            //cryptStream.Close();
            serializedStream.Flush();
            serializedStream.Close();
            return memoryStream;
        }

        public static object Decrypt(Stream stream)
        {
            des = new DESCryptoServiceProvider
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = IV
            };

            desencrypt = des.CreateDecryptor();
            CryptoStream cryptStream = new CryptoStream(stream, desencrypt, CryptoStreamMode.Read);

            byte[] byteArray = new byte[stream.Length];

            cryptStream.Read(byteArray, 0, byteArray.Length);

            MemoryStream memoryStream = new MemoryStream();

            memoryStream.Write(byteArray, 0, byteArray.Length);

            IFormatter formatter = new BinaryFormatter();

            memoryStream.Seek(0, SeekOrigin.Begin);

            object dataObject = formatter.Deserialize(memoryStream);
            cryptStream.Flush();
            cryptStream.Close();
            stream.Close();
            memoryStream.Flush();
            memoryStream.Close();

            return dataObject;
        }
    }
    #endregion
}

