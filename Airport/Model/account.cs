using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Model
{
    class Account
    {
        public class Rootobject
        {
            public string sate { get; set; }
            public string sum { get; set; }
            public string enable { get; set; }
            public string status { get; set; }
            public string quantity_ss { get; set; }
            public Ss_Pro ss_pro { get; set; }
            public Ss ss { get; set; }
        }

        public class Ss_Pro
        {
            public US US { get; set; }
            public JP JP { get; set; }
            public KR KR { get; set; }
            public HK HK { get; set; }
        }
        public class Ss
        {
            public US US { get; set; }
            public JP JP { get; set; }
            public KR KR { get; set; }
            public HK HK { get; set; }
        }

        public class US
        {
            public string US_num { get; set; }
            public US_List[] US_list { get; set; }
        }

        public class US_List
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

        public class JP
        {
            public string JP_num { get; set; }
            public JP_List[] JP_list { get; set; }
        }

        public class JP_List
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

        public class KR
        {
            public string KR_num { get; set; }
            public KR_List[] KR_list { get; set; }
        }

        public class KR_List
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

        public class HK
        {
            public string HK_num { get; set; }
            public HK_List[] HK_list { get; set; }
        }

        public class HK_List
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

       
        /*
        public class US1
        {
            public string US_num { get; set; }
            public US_List1[] US_list { get; set; }
        }

        public class US_List1
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

        public class JP1
        {
            public string JP_num { get; set; }
            public JP_List1[] JP_list { get; set; }
        }

        public class JP_List1
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

        public class KR1
        {
            public string KR_num { get; set; }
            public KR_List1[] KR_list { get; set; }
        }

        public class KR_List1
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }

        public class HK1
        {
            public string HK_num { get; set; }
            public HK_List1[] HK_list { get; set; }
        }

        public class HK_List1
        {
            public string ip { get; set; }
            public string port { get; set; }
            public string pw { get; set; }
            public string jm { get; set; }
        }
        */
    }
}
