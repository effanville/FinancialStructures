# To Do things for the program

## Bug issues


## Implementation Changes

1. Separate out Edit Windows into multiple files
	1.1 Make debugging easier as less in one place
	1.2 Make flow of data a bit more complicated 
	1.3 May enable reusing common code a bit more.
2. Sorting of reports by various statistics

## Major Features

4. More in depth portfolio analytics.
5. Graphs of current status of portfolio, and displaying history. e.g 

using System.Windows.Forms.DataVisualization.Charting;

private void CreateChart()
{
    var series = new Series("Finance");
    series.ChartType = SeriesChartType.Line;
    // Frist parameter is X-Axis and Second is Collection of Y- Axis
    series.Points.DataBindXY(new[] { 2001, 2002, 2003, 2004 }, new[] { 100, 200, 90, 150 });
    chart1.Series.Add(series);

}

7. Stats creator tabs for the different options ( and for html reports)
8. Method to clear data if have too much

# Fixed issues/Completed Tasks

1. Methodology behing what is a new investment is currently flawed
	- It doesn't currently update itself when new data is added
	- Requires user to add investments in date order.
2. Order Securities based upon company and name
3.In statistics pages add a total valuation.
1. Add in download data from internet
5. On closing, prompt user to save database.
3. Include sectors within securities, and implement comparisons with the sector.
2. Update deleting and editing data to be more flexible (enable date data to be changed)
2. Allow user to select statistics for export
6. Use of display in different currencies
7. Read data from a csv file.
6. Use Tabs in various displays to collate different information
3. Provide ability to create custom reports
4. Reduce amount of spacing in reports.