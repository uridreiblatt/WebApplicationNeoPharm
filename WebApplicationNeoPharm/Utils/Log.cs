
    using System;
    
    using System.IO;


    namespace WebApplicationNeoPharm.Utils
    {
        public static class NeoPharmLog
        {

            #region Severity Level

            /// <summary>
            /// Severity Level
            /// </summary>
            public enum SeverityLevel
            {
                Debug,
                Info,
                Warn,
                Error,
                Fatal,
                Always,
                /// <summary>
                /// None - For internatl use
                /// </summary>
                None
            }

            #endregion


            public static int SocketPort = 9820;
            public static int Err_Priority_ResExp = 700;




            public static void Write(NeoPharmLog.SeverityLevel errLevel, Exception Er, string Sinf)
            {
                string FileName = @"c:\temp\elogy\" + DateTime.Now.ToShortDateString().Replace("/", "") + ".txt";
                try
                {


                    TextWriter tw = new StreamWriter(FileName, true);
                    // write a line of text to the file
                    tw.WriteLine(DateTime.Now + "(" + DateTime.Now.Ticks.ToString() + ") " + Sinf);
                    if (Er != null)
                    {
                        tw.WriteLine(DateTime.Now + "(" + DateTime.Now.Ticks.ToString() + ") " + Er.Message);
                        tw.WriteLine(DateTime.Now + "(" + DateTime.Now.Ticks.ToString() + ") " + Er.StackTrace);
                    }
                    // close the stream
                    tw.Close();


                }
                catch (Exception)
                {

                }
            }
           


        

        }
    }

