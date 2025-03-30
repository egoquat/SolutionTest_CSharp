using System;
using System.Collections.Generic;

namespace _200310_Tests
{
    public class NotesStore
    {
        List<string> States = new List<string>();
        List<string> Names = new List<string>();
        
        public NotesStore() {}
        public void AddNote(string state, string name) {
            try {
                if (name == string.Empty) throw new Exception();
                if (state is null || state == string.Empty) throw new Exception();
                States.Add(state);
                Names.Add(name);
            } catch ( Exception e) {
                if (name == string.Empty)
                    Console.WriteLine("Name cannot be empty");
                if (state is null || state == string.Empty)
                    Console.WriteLine("Invalid state {0}", state);
            }
        }
        public List<string> GetNotes(string state) {
            List<string> notes = new List<string>();
            for (int i = 0; i < States.Count; ++i) {
                string s = States[i];
                if (s != state) continue;
                notes.Add(Names[i]);
            }
            return notes;
        }
    } 
    
}