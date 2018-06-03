using nGantt.GanttChart;
using nGantt.PeriodSplitter;
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
            SetDatas();
            InitializeComponent();
            this.DataContext = this;

        }


        private void SetDatas()
        {
            GantLenght = 60;
            this.ganttChartData.MinDate = DateTime.Parse("2018-01-01");
            this.ganttChartData.MaxDate = this.ganttChartData.MinDate.AddDays(GantLenght);
            this.ganttChartData.NName = "G data";
            CreateTimeLine(new PeriodMonthSplitter(this.ganttChartData.MinDate, this.ganttChartData.MaxDate), FormatMonth, DetermineBackground, "Month", new SolidColorBrush(Colors.LightCoral));
            CreateTimeLine(new PeriodDaySplitter(this.ganttChartData.MinDate, this.ganttChartData.MaxDate), FormatDay, DetermineBackground, "Day", new SolidColorBrush(Colors.LightGreen));

            // Create and data
            var rowgroup1 = CreateGanttRowGroup("Group");
            var row1 = CreateGanttRow(rowgroup1, "GanttRow 1");
            AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2018-01-03"), End = DateTime.Parse("2018-01-05"), Name = "GanttRow 1:GanttTask 1", TaskProgressVisibility = System.Windows.Visibility.Hidden });
            AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2018-01-05"), End = DateTime.Parse("2018-01-06"), Name = "GanttRow 1:GanttTask 2" });
            AddGanttTask(row1, new GanttTask() { Start = DateTime.Parse("2018-01-03"), End = DateTime.Parse("2018-01-08"), Name = "GanttRow 1:GanttTask 3" });


        }

        public HeaderedGanttRowGroup CreateGanttRowGroup(string name)
        {
            var rowGroup = new HeaderedGanttRowGroup() { Name = name };
            this.ganttChartData.RowGroups.Add(rowGroup);
            return rowGroup;
        }

        public GanttRow CreateGanttRow(GanttRowGroup rowGroup, string name)
        {
            var rowHeader = new GanttRowHeader() { Name = name };
            var row = new GanttRow() { RowHeader = rowHeader, Tasks = new ObservableCollection<GanttTask>() };
            rowGroup.Rows.Add(row);
            return row;
        }
        public void AddGanttTask(GanttRow row, GanttTask task)
        {
            if (task.Start < ganttChartData.MaxDate && task.End > ganttChartData.MinDate)
                row.Tasks.Add(task);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //SetDatas();
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
