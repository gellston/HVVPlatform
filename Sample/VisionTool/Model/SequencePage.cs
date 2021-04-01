using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SequencePage
    {
        public SequencePage()
        {

        }


        public string Content
        {
            get; set;
        } = "";



        public int StartRow
        {
            get; set;
        } = 0;



        public int EndRow
        {
            get; set;
        } = 0;
    }
}
