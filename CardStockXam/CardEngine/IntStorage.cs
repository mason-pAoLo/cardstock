using System.Collections.Generic;
using System;

namespace CardEngine{
	public class IntStorage{
		public int this[string key]
		{
		    get
		    {
				if (!binDict.ContainsKey(key)) {
					AddKey(key);
				}
		        return storage[binDict[key]];
		    }
		    set
		    {
				
				if (!binDict.ContainsKey(key)) {
					AddKey(key);
				}
	       		storage[binDict[key]] = value;
				if (triggerDict.ContainsKey(key)){
					foreach (var trigger in triggerDict[key]){
						trigger.Evaluate(storage[binDict[key]]);
					}
				}
	    	}
		}
        public Player owner;
        public Team teamOwner;
        public int[] storage;
		public int binCounter = 0;
		Dictionary<string,List<Trigger>> triggerDict = new Dictionary<string, List<Trigger>>();
		public Dictionary<string,int> binDict = new Dictionary<string,int>();
		public IntStorage(){
			storage = new int[32];
		}
		public IntStorage Clone(){
			var raw = new IntStorage ();
			raw.storage = storage.Clone () as int[];
			foreach (var key in binDict.Keys) {
				raw.AddKey (key);
			}
			foreach (var key in triggerDict.Keys) {
				foreach (var trig in triggerDict[key]) {
					raw.AddTrigger (trig, key);
				}
			}
			return raw;
		}
		public void AddTrigger(Trigger trig, string key){
			if (!triggerDict.ContainsKey(key)){
				triggerDict[key] = new List<Trigger>();
			}
			triggerDict[key].Add(trig);
		}
        public void AddKey(string key)
        {
            binDict.Add(key, binCounter);
            binCounter++;
        }
        public override bool Equals(System.Object obj)
        {

            if (obj == null)
            {
                return false;
            }
            IntStorage p = obj as IntStorage;
            if ((System.Object)p == null)
            {
                return false;
            }

            if (p.binCounter != binCounter) // if storage locations are not the same size
            {
                return false;
            }

            foreach (var bin in binDict.Keys)
            {
                {
                    if ((storage[binDict[bin]] !=(p.storage[binDict[bin]]))) // For each storage location, points must be equal
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public override int GetHashCode()
            {
            int hash = 0;
            foreach (var bin in binDict.Keys)
            {
                hash ^= storage[binDict[bin]].GetHashCode();
            }
            return hash;
            }
    
	}
	public class Trigger{
		public TriggerException exception;
		public string op;
		public int value;
		public void Evaluate(int v){
			var shouldTrigger = false;
			if (op == ">="){
				if (v >= value){
					shouldTrigger = true;
				}
			}
			if (shouldTrigger){
				throw exception;
			}
		}
		
	}
	public class TriggerException : Exception
    {
		public string Level {get; set;}
        public TriggerException()
            : base() { }

        public TriggerException(string message)
            : base(message) { }

        public TriggerException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public TriggerException(string message, Exception innerException)
            : base(message, innerException) { }

        public TriggerException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}