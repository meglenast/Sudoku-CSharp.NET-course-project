using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;

namespace Sudoku
{
    class TimeTracker
    {
        private Timer timer;
        private int secs, mins, hrs;


        public TimeTracker()
        {
            timer = new Timer();

            secs = 0;
            mins = 0;
            hrs = 0;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimeEvent;
            timer.Start();

            // timeTracker = new TimeTracker(LblTimer, 0, 0, 0);    
        }

        private void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
           
                secs += 1;

                if (secs == 60)
                {
                    secs = 0;
                    mins += 1;
                }
                if (mins == 60)
                {
                    mins = 0;
                    hrs += 1;
                }
                
            GridPage.page.TimeTracker = $"{hrs:d2}:{mins:d2}:{secs:d2}";


        }
    }
}
