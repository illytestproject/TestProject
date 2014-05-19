using System;
using System.Collections.Generic;
using System.Reflection;


namespace TestProject
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome\n\n");
            var myFirstOb = new SomeType();
            myFirstOb.Forename = "DavId";
            myFirstOb.DateOfBirth = new DateTime(1990, 5, 2);
            myFirstOb.SpecialNumber = 23;

            var mySecondOb = new SomeType();
            mySecondOb.Forename = "Janet";
            mySecondOb.DateOfBirth = new DateTime(2000, 11, 22);
            mySecondOb.SpecialNumber = 23;

            var myThirdOb = new SomeType();
            myThirdOb.Forename = "Janet";
            myThirdOb.Surname = "Newman";
            myThirdOb.DateOfBirth = new DateTime(2000, 11, 22);
            myThirdOb.SpecialNumber = 21;

            var myFourthOb = new AnotherType();
            myFourthOb.Forename = "Janet";
            myFourthOb.DateOfBirth = new DateTime(2000, 11, 22);
            myFourthOb.SpecialNumber = 23;

            
            List<string> ls = new List<string>();
            ls.Add("Compare myFirstOb with mySecondOb");
            ls.AddRange(CompareObjects.DoCompare(myFirstOb, mySecondOb));
            ls.Add("");
            ls.Add("Compare mySecondOb with myThirdOb");
            ls.AddRange(CompareObjects.DoCompare(mySecondOb, myThirdOb));
            ls.Add("");
            ls.Add("Compare myFirstOb with myThirdOb");
            ls.AddRange(CompareObjects.DoCompare(myFirstOb, myThirdOb));
            ls.Add("");
            ls.Add("Compare myThirdOb with myFourthOb");
            ls.AddRange(CompareObjects.DoCompare(myThirdOb, myFourthOb));
            ls.Add("");

            var subClass1 = new SubClass();
            subClass1.Id = 5;
            subClass1.Name = "Janet";

            var subClass2 = new SubClass();
            subClass2.Id = 7;
            subClass2.Name = "Janet";

            var myFifthOb = new SubClassType();
            myFifthOb.Id = 5;
            myFifthOb.subClass = subClass1;

            var mySixthOb = new SubClassType();
            mySixthOb.Id = 6;
            mySixthOb.subClass = subClass1;

            var mySeventhOb = new SubClassType();
            mySeventhOb.Id = 5;
            mySeventhOb.subClass = subClass2;

            var myEightOb = new SubClassType();
            myEightOb.Id = 5;
            

            ls.Add("Compare myFifthOb with mySixthOb");
            ls.AddRange(CompareObjects.DoCompare(myFifthOb, mySixthOb));
            ls.Add("");
            ls.Add("Compare mySixthOb with mySeventhOb");
            ls.AddRange(CompareObjects.DoCompare(mySixthOb, mySeventhOb));
            ls.Add("");
            ls.Add("Compare mySeventhOb with myEightOb");
            ls.AddRange(CompareObjects.DoCompare(mySeventhOb, myEightOb));

            foreach (string s in ls)
            {
                Console.WriteLine(s);
            }
        }


        public class SomeType
        {
            public string Forename;
            public string Surname;
            public DateTime DateOfBirth;
            public int SpecialNumber;
        }

        public class AnotherType
        {
            public string Forename;
            public DateTime DateOfBirth;
            public int SpecialNumber;
        }

        public class SubClassType
        {
            public int Id;
            public SubClass subClass;
        }

        public class SubClass : object
        {
            public int Id;
            public string Name;

            public override string ToString()
            {
                return Id.ToString() + " " + Name; 
            }
        }

        public class CompareObjects
        {
            public static List<string> DoCompare(object object1, object object2)
            {
                
                Type t1 = object1.GetType();
                Type t2 = object2.GetType();

                List<string> ls = new List<string>();
                if (t1 != t2)
                { // if types are different return error message
                    ls.Add("Their classes are different!");
                    return ls;
                }
                FieldInfo[] fia = t1.GetFields(); // Get the FieldInfo Array from the type
                for (int i = 0; i < fia.Length; i++)
                {
                    if (fia[i].IsPublic)
                    { // go through all public properties, compare their values for both input parameters (object1, object2) and return if different (consider one null and other not as a different)
                        if (fia[i].GetValue(object1) != null && fia[i].GetValue(object2) != null && !fia[i].GetValue(object1).Equals(fia[i].GetValue(object2)))
                        {
                            ls.Add(SplitString(fia[i].Name) + " changed from '" + fia[i].GetValue(object1).ToString() + "' to '" + fia[i].GetValue(object2).ToString() + "'");
                        }
                        if (fia[i].GetValue(object1) == null && fia[i].GetValue(object2) != null)
                        {
                            ls.Add(SplitString(fia[i].Name) + " changed from NULL to '" + fia[i].GetValue(object2).ToString() + "'");
                        }
                        if (fia[i].GetValue(object1) != null && fia[i].GetValue(object2) == null)
                        {
                            ls.Add(SplitString(fia[i].Name) + " changed from '" + fia[i].GetValue(object1).ToString() + "' to NULL");
                        }
                    }
                }
                return ls;
            }

            private static string SplitString(string s)
            {
                return System.Text.RegularExpressions.Regex.Replace(s, "([A-Z])", " $1").Trim();
            }
        }
    }
}
