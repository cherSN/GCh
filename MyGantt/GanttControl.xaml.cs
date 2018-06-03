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
            this.ganttChartData.MinDate = DateTime.Parse("2018-01-01");
            this.ganttChartData.MaxDate = this.ganttChartData.MinDate.AddDays(1);
        }
        public void Initialize(DateTime minDate, DateTime maxDate)
        {
            this.ganttChartData.MinDate = minDate;
            this.ganttChartData.MaxDate = maxDate;
        }
        public void AddGanttTask(GanttRow row, GanttTask task)
        {
            if (task.Start < ganttChartData.MaxDate && task.End > ganttChartData.MinDate)
                row.Tasks.Add(task);
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

        public TimeLine CreateTimeLine(PeriodSplitter.PeriodSplitter splitter, PeriodNameFormatter PeriodNameFormatter, 
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
        public HeaderedGanttRowGroup CreateGanttRowGroup(string name)
        {
            var rowGroup = new HeaderedGanttRowGroup() { Name = name };
            ganttChartData.RowGroups.Add(rowGroup);
            return rowGroup;
        }

        public ExpandableGanttRowGroup CreateGanttRowGroup(string name, bool isExpanded)
        {
            var rowGroup = new ExpandableGanttRowGroup() { Name = name, IsExpanded = isExpanded };
            ganttChartData.RowGroups.Add(rowGroup);
            return rowGroup;
        }

        public GanttRow CreateGanttRow(GanttRowGroup rowGroup, string name)
        {
            var rowHeader = new GanttRowHeader() { Name = name };
            var row = new GanttRow() { RowHeader = rowHeader, Tasks = new ObservableCollection<GanttTask>() };
            rowGroup.Rows.Add(row);
            return row;
        }

        private void selectionRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            //ChangeSelectionRectangleSize(sender, e);
        }

        private void selectionCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            //StopSelection(sender, e);
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (!AllowUserSelection)
            //    return;

            //// TODO:: Set visibillity to hidden for all selectionRectangles
            //var canvas = ((Canvas)UIHelper.FindVisualParent<Grid>(((DependencyObject)sender)).FindName("selectionCanvas"));
            //Border selectionRectangle = (Border)canvas.FindName("selectionRectangle");
            //selectionStartX = e.GetPosition(canvas).X;
            //selectionRectangle.Margin = new Thickness(selectionStartX, 0, 0, 5);
            //selectionRectangle.Visibility = Visibility.Visible;
            //selectionRectangle.IsEnabled = true;
            //selectionRectangle.IsHitTestVisible = false;
            //selectionRectangle.Width = 0;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //StopSelection(sender, e);
        }

        private void selectionRectangle_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            //((Border)sender).ContextMenu.IsOpen = true;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (TaskSelectionMode == SelectionMode.None)
            //    return;

            //if (TaskSelectionMode == SelectionMode.Single)
            //    DeselectAllTasks();

            //var gantTask = ((GanttTask)((FrameworkElement)(sender)).DataContext);
            //gantTask.IsSelected = !gantTask.IsSelected;

            //if (SelectedItemChanged != null)
            //    SelectedItemChanged(this, EventArgs.Empty);
        }
    }
}
