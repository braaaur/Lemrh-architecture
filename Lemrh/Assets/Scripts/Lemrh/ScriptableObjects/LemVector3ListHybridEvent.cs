//Copyright (c) 2022 Serious Sim. All rights reserved
/*
 * Created by: Marcin
 * 
 * Created on: 3/20/2022 8:45:57 PM#
 * 
 * Edited: -
 *  
 * [LemVector3ListHybridEvent]
 * 
 * ScriptDescription: 
 */

using UnityEngine;
using System.Collections.Generic;

namespace Lemrh
{
	[CreateAssetMenu(menuName = "LemVariables/LemVector3ListHybridEvent")]
	public class LemVector3ListHybridEvent : ScriptableObject
	{ 
        private List<Vector3> vector3ListValue;

        public delegate void ValueChangeDelegate(bool isDebug);
        ValueChangeDelegate valueChangeDelegate;

        private List<LemrhEventListener> listeners = new List<LemrhEventListener>();

        public List<Vector3> defaultValue;

        public bool debugChange;

        [SerializeField]
        [TextArea]
        private string info;

        public List<Vector3> Vector3ListValue
        {
            set
            {
                vector3ListValue = value;

                ValueChanged();
            }
            get
            {
                return vector3ListValue;
            }
        }

        private void ValueChanged()
        {
            if (valueChangeDelegate != null)
            {
                valueChangeDelegate(debugChange);
            }

            RiseListeners();
        }

        //delegate part
        public void RegisterDelegate(ValueChangeDelegate givenDelegate)
        {
            valueChangeDelegate += givenDelegate;
        }

        public void UnregisterDelegate(ValueChangeDelegate givenDelegate)
        {
            valueChangeDelegate -= givenDelegate;
        }

        //event part
        private void RiseListeners()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRised(debugChange);
            }
        }

        public void RegisterListener(LemrhEventListener givenListener)
        {
            listeners.Add(givenListener);
        }

        public void UnregisterListener(LemrhEventListener givenListener)
        {
            listeners.Remove(givenListener);
        }

        public void Reset()
        {
            Vector3ListValue = new List<Vector3>(defaultValue);
        }

        public string Id
        {
            get
            {
                return name;
            }
        }
    }

    public interface ILemVector3List
    {
        List<Vector3> Vector3ListValue
        {
            get;
            set;
        }
    }

}