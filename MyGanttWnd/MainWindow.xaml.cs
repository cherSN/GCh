using nGantt.GanttChart;
using nGantt.PeriodSplitter;
using System;
using System.Collections.Generic;
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

namespace MyGanttWnd
{
   
    public partial class MainWindow : Window
    {
        private GanttChartData ganttChartData = new GanttChartData();
        public delegate string PeriodNameFormatter(Period period);
        public delegate Brush BackgroundFormatter(TimeLineItem timeLineItem);
        public GanttChartData GanttData { get { return ganttChartData; } }
        private int GantLenght { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //this.ganttChartData.MinDate = DateTime.Parse("2018-01-01");
            //this.ganttChartData.MaxDate = this.ganttChartData.MinDate.AddDays(1);
        }

        public void Initialize(DateTime minDate, DateTime maxDate)
        {
            this.ganttChartData.MinDate = minDate;
            this.ganttChartData.MaxDate = maxDate;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //nGanttChart = new nGantt.GanttControl();
            GantLenght = 2;

            //DateTime minDate = DateTime.Parse("2018-02-01");
            //DateTime maxDate = minDate.AddDays(GantLenght);

            //// Set selection -mode
            //nGanttChart.TaskSelectionMode = nGantt.GanttControl.SelectionMode.Single;
            //// Enable GanttTasks to be selected
            //nGanttChart.AllowUserSelection = true;

            //// listen to the GanttRowAreaSelected event
            //nGanttChart.GanttRowAreaSelected += new EventHandler<PeriodEventArgs>(ganttControl1_GanttRowAreaSelected);


            //ganttTaskContextMenuItems.Add(new ContextMenuItem(ViewReport, "View Report"));
            //nGanttChart.GanttTaskContextMenuItems = ganttTaskContextMenuItems;



            //nGanttChart.ClearGantt();
            DateTime MinDate = DateTime.Parse("2018-01-01");
            DateTime MaxDate = MinDate.AddDays(GantLenght);
            this.CreateData(MinDate, MaxDate);


        }
        public TimeLine CreateTimeLine(PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter)
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

        public TimeLine CreateTimeLine(PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter, BackgroundFormatter backgroundFormatter)
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

        public TimeLine CreateTimeLine(PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter,
            BackgroundFormatter backgroundFormatter, string timeLineName, Brush timeLineColor)
        {
            if (splitter.MaxDate != GanttData.MaxDate || splitter.MinDate != GanttData.MinDate)
                throw new ArgumentException("The timeline must have the same max and min -date as the chart");

            var timeLineParts = splitter.Split();

            TimeLine timeline = new TimeLine();
            timeline.Name = timeLineName;
            timeline.BackgroundColor = timeLineColor;
            foreach (var p in timeLineParts)
            {
                TimeLineItem item = new TimeLineItem() { Name = PeriodNameFormatter(p), Start = p.Start, End = p.End.AddSeconds(-1) };
                item.BackgroundColor = backgroundFormatter(item);
                timeline.Items.Add(item);
            }

            ganttChartData.TimeLines.Add(timeline);
            return timeline;
        }


        private void CreateData(DateTime minDate, DateTime maxDate)
        {
            // Set max and min dates
            Initialize(minDate, maxDate);
            GanttData.Name = "G data";


            // Create timelines and define how they should be presented
            //nGanttChart.CreateTimeLine(new PeriodYearSplitter(minDate, maxDate), FormatYear);
            //nGanttChart.CreateTimeLine(new PeriodMonthSplitter(minDate, maxDate), FormatMonth);
            //nGanttChart.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDayName);
            //nGanttChart.CreateTimeLine(new PeriodYearSplitter(minDate, maxDate), FormatYear, DetermineBackground, "Year", new SolidColorBrush(Colors.LightSkyBlue));
            //nGanttChart.CreateTimeLine(new PeriodMonthSplitter(minDate, maxDate), FormatMonth, DetermineBackground, "Month", new SolidColorBrush(Colors.LightCoral));
            CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDay, DetermineBackground, "Day", new SolidColorBrush(Colors.LightGreen));
            // Set the timeline to atatch gridlines to
            //var gridLineTimeLine = nGanttChart.CreateTimeLine(new PeriodDaySplitter(minDate, maxDate), FormatDay, DetermineBackground);
            //nGanttChart.SetGridLinesTimeline(gridLineTimeLine, DetermineBackground);

            //// Create and data
            //var rowgroup1 = nGanttChart.CreateGanttRowGroup("Group");
            //var row1 = nGanttChart.CreateGanttRow(rowgroup1, "GanttRow 1");
            //nGanttChart.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2012-02-01"), End = DateTime.Parse("2012-02-03"), Name = "GanttRow 1:GanttTask 1", TaskProgressVisibility = System.Windows.Visibility.Hidden });
            //nGanttChart.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2012-02-05"), End = DateTime.Parse("2012-03-01"), Name = "GanttRow 1:GanttTask 2" });
            //nGanttChart.AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2012-06-01"), End = DateTime.Parse("2012-06-15"), Name = "GanttRow 1:GanttTask 3" });

            //var rowgroup2 = nGanttChart.CreateGanttRowGroup("ExpandableGanttRowGroup", true);
            //var row2 = nGanttChart.CreateGanttRow(rowgroup2, "GanttRow 2");
            //var row3 = nGanttChart.CreateGanttRow(rowgroup2, "GanttRow 3");
            //nGanttChart.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2012-02-10"), End = DateTime.Parse("2012-03-10"), Name = "GanttRow 2:GanttTask 1" });
            //nGanttChart.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2012-03-25"), End = DateTime.Parse("2012-05-10"), Name = "GanttRow 2:GanttTask 2" });
            //nGanttChart.AddGanttTask(row2, new GanttTask() { Start = DateTime.Parse("2012-06-10"), End = DateTime.Parse("2012-09-15"), Name = "GanttRow 2:GanttTask 3", PercentageCompleted = 0.375 });
            //nGanttChart.AddGanttTask(row3, new GanttTask() { Start = DateTime.Parse("2012-01-07"), End = DateTime.Parse("2012-09-15"), Name = "GanttRow 3:GanttTask 1", PercentageCompleted = 0.5 });

        }

        private string FormatYear(Period period)
        {
            return period.Start.Year.ToString();
        }

        private string FormatMonth(Period period)
        {
            return period.Start.ToString("MMMM") + " " + period.Start.Year.ToString();
        }

        private string FormatDay(Period period)
        {
            return period.Start.Day.ToString();
        }

        private string FormatDayName(Period period)
        {
            return period.Start.DayOfWeek.ToString().Substring(0, 1);
        }

        private System.Windows.Media.Brush DetermineBackground(TimeLineItem timeLineItem)
        {
            if (timeLineItem.End.Date.DayOfWeek == DayOfWeek.Saturday || timeLineItem.End.Date.DayOfWeek == DayOfWeek.Sunday)
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightBlue);
            else
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent);
        }
    }
}
