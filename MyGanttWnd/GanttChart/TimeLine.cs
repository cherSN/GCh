using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace nGantt.GanttChart
{
    public class TimeLine
    {
        public TimeLine()
        {
            Items = new ObservableCollection<TimeLineItem>();
        }
        public string Name { get; set; }
        public Brush BackgroundColor { get; set; }
        public ObservableCollection<TimeLineItem> Items { get; set; }
        
    }
}
