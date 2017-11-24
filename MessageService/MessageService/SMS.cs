using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.IO;

namespace MessageService
{
    public class SMS : Message//set up the SMS classes to setup the json file for sms system
    {
        private string ID;
        private string sender;
        private string body;

        public string _ID
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }//end _ID

        public string _Sender
        {
            get
            {
                return this.sender;
            }
            set
            {
                if (Regex.Match(value, @"^(\+(0-9)(9))$").Success)
                {
                    this.sender = value;
                }
                else
                {
                    //throw new Exception("Please enter 9 digits");
                }
            }
        }//end _sender

        public string _Body
        {
            get
            {
                return this.body;
            }
            set
            {
                string message = "";
                if(value.Length > 140 || value == null)
                {
                    throw new Exception("please enter message");
                }
                using (var reader = new StreamReader(@"F:\Uni\Software Engineering\CourseWork\Work\MessageService\textwords.csv"))//change if the source is different
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        listA.Add(values[0]);
                        listB.Add(values[1]);
                    }
                    var words = value.Split(' ');
                    foreach (var word in words)
                    {
                        int i = 0;
                        foreach (var val in listA)
                        {
                            if(val == word)
                            {
                                message += "c" + listB[i] + "> ";
                                break;
                            }
                            else if(val == listA.Last())
                            {
                                message += word + " ";
                            }
                            i++;
                        }
                    }
                    this._Body = message;
                }//end using
            }//end set
        }//end _body method
    }//end class
}//end namespace
