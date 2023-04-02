/*
 * Jakub Bukała
 * 
 * Created: 11.02.2021 r.
 * 
 * Edited: 2.04.2023 r.
 *  
 * [LemDateHybridEvent]
 * 
 * in LEMRH framework
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using System;
//using Sirenix.OdinInspector;

namespace Lemrh
{
    [CreateAssetMenu(menuName = "LemVariables/LemrhCollector")]
    //public class LemCollector : SerializedScriptableObject, IResetable, ISavable
    public class LemCollector : ScriptableObject, IResetable, ISavable
    {
        [Header("Warning! When used as saved/loaded entity, it must have a unique name")]
        public List<ISavable> lemEntities;

        public void Reset()
        {
            foreach (IResetable collectorEntity in lemEntities)
            {
                collectorEntity.Reset();
            }
        }

        public ISave Save()
        {
            List<ISave> saveList = new List<ISave>();

            foreach(ISavable collectorEntity in lemEntities)
            {
                saveList.Add(collectorEntity.Save());
            }

            return new LemListSave(saveList, name);
        }

        public void Load(ISave givenValue)
        {
            foreach (ISavable collectorEntity in lemEntities)
            {
                bool tempSuccess = false;

                foreach (ISave save in ((LemListSave)givenValue).listValue)
                {
                    if (save.Id == collectorEntity.Id)
                    {
                        collectorEntity.Load(save);

                        if (!tempSuccess)
                        {
                            tempSuccess = true;
                        }
                        else
                        {
                            Debug.LogError("Collector entity doubled in Isave file - setting to default!");

                            ((IResetable)collectorEntity).Reset();
                        }
                    }
                }

                if (!tempSuccess)
                {
                    Debug.LogWarning("Collector entity not found in Isave file - setting to default.");

                    ((IResetable)collectorEntity).Reset();
                }
            }
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    [System.Serializable]
    public class LemListSave : ISave
    {
        public LemListSave(List<ISave> givenValue, string givenId)
        {
            listValue = givenValue;
            id = givenId;
        }

        public string id;
        public List<ISave> listValue;

        public string Id
        {
            get
            {
                return id;
            }
        }
    }
}
