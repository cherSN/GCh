using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using nGantt.GanttChart;
using nGantt.PeriodSplitter;

namespace nGantt
{
    /// <summary>
    /// Логика взаимодействия для GanttControl.xaml
    /// </summary>
    public partial class GanttControl : UserControl
    {
        private GanttChartData ganttChartData = new GanttChartData();
        //private TimeLine gridLineTimeLine;
        ////private ObservableCollection<TimeLine> gridLineTimeLines = new ObservableCollection<TimeLine>();

        public delegate string PeriodNameFormatter(Period period);
        public delegate Brush BackgroundFormatter(TimeLineItem timeLineItem);


        public GanttChartData GanttData { get { return ganttChartData; } }
        ////public ObservableCollection<TimeLine> GridLineTimeLine { get { return gridLineTimeLines; } }
        ////public ObservableCollection<TimeLine> TimeLines { get; private set; }
        public Period SelectionPeriod { get; private set; }


        public GanttControl()
        {
            InitializeComponent();
            DataContext = this;
            SelectionPeriod = new Period();
        }
        public void Initialize(DateTime minDate, DateTime maxDate)
        {
            this.ganttChartData.MinDate = minDate;
            this.ganttChartData.MaxDate = maxDate;
        }
        public TimeLine CreateTimeLine(PeriodSplitter.PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter)
        {
            if (splitter.MaxDate != GanttData.MaxDate || splitter.MinDate != GanttData.MinDate)
                throw new ArgumentException("The timeline must have the same max and min -date as the chart");

            var timeLineParts = splitter.Split();

            var timeline = new TimeLine();
            foreach (var p in timeLineParts)
            {
                timeline.Items.Add(new TimeLineItem() { Name = PeriodNameFormatter(p), Start = p.Start, End = p.End.AddSeconds(-1) });
            }

            ganttChartData.TimeLines.Add(timeline);
            return timeline;
        }

        public TimeLine CreateTimeLine(PeriodSplitter.PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter, BackgroundFormatter backgroundFormatter)
        {
            if (splitter.MaxDate != GanttData.MaxDate || splitter.MinDate != GanttData.MinDate)
                throw new ArgumentException("The timeline must have the same max and min -date as the chart");

            var timeLineParts = splitter.Split();

            var timeline = new TimeLine();
            foreach (var p in timeLineParts)
            {
                TimeLineItem item = new TimeLineItem() { Name = PeriodNameFormatter(p), Start = p.Start, End = p.End.AddSeconds(-1) };
                item.BackgroundColor = backgroundFormatter(item);
                timeline.Items.Add(item);
            }

            ganttChartData.TimeLines.Add(timeline);
            return timeline;
        }

        ////public void SetGridLinesTimeline(TimeLine timeline)
        ////{
        ////    if (!ganttChartData.TimeLines.Contains(timeline))
        ////        throw new Exception("Invalid timeline");

        ////    //gridLineTimeLine = timeline;
        ////}

        ////public void SetGridLinesTimeline(TimeLine timeline, BackgroundFormatter backgroundFormatter)
        ////{
        ////    if (!ganttChartData.TimeLines.Contains(timeline))
        ////        throw new Exception("Invalid timeline");

        ////    foreach (var item in timeline.Items)
        ////        item.BackgroundColor = backgroundFormatter(item);

        ////    gridLineTimeLines.Clear();
        ////    gridLineTimeLines.Add(timeline);
        ////    //gridLineTimeLine = timeline;
        ////}



    }
}
