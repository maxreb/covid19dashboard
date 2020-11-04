﻿using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes.Ticks;
using ChartJs.Blazor.ChartJS.Common.Enums;
using ChartJs.Blazor.ChartJS.Common.Handlers;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.Common.Time;
using ChartJs.Blazor.ChartJS.LineChart;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Covid19Dashboard.Components
{
	public partial class RippleLineChartText
	{
		[Parameter]
		public string Title { get; set; } = "Data";
		[Parameter]
		public string? TextStyle { get; set; }
		[Parameter]
		public IList<TimeTuple<double>> Data { get; set; } = default!;
		[Parameter]
		public bool ShowTrendIcon { get; set; }
		[Parameter]
		public bool ShowTrendNumber { get; set; }

		private double dataToday;
		private double dataYesterday;
		private bool showTrend = false;


		private readonly LineConfig _chartConfig;
		public RippleLineChartText()
		{
			_chartConfig = CreateNewChartConfig();
		}
		protected override void OnParametersSet()
		{
			if (Data == null || Data.Count < 1)
				throw new ArgumentNullException(nameof(Data));
			if (ShowTrendIcon && ShowTrendNumber)
				throw new ArgumentException($"You have to choose between {nameof(ShowTrendIcon)} and {nameof(ShowTrendNumber)}");
			showTrend = ShowTrendNumber || ShowTrendIcon;
			dataToday = Math.Round(Data[Data.Count - 1].YValue, 1);
			if (Data.Count > 1)
			{
				dataYesterday = Math.Round(Data[Data.Count - 2].YValue, 1);
			}
			else
			{
				showTrend = false;
			}
			double min = Data.Min(x => x.YValue);
			double max = Data.Max(x => x.YValue);
			double den = 10;
			if (min > 1000)
				den = 100;
			min = Math.Floor(min / den) * den;
			max = Math.Ceiling(max / den) * den;

			_chartConfig.Data.Datasets.Clear();

			foreach (LinearCartesianAxis yAxis in _chartConfig.Options.Scales.yAxes)
			{
				yAxis.Ticks.Min = min;
				yAxis.Ticks.Max = max;
			}

			var data = new LineDataset<TimeTuple<double>>(Data)
			{
				BorderColor = "#666",
				BackgroundColor = "#555",
				Fill = true,
				BorderWidth = 2,
				PointRadius = 0,
				LineTension = 0.1
			};
			_chartConfig.Data.Datasets.Add(data);
		}

		private (string icon, string style) GetIconAndStyle(double a, double b)
		{
			string style = "width:100%;color:";
			if (a > b)
				return ("arrow_upward", style + "#f00");
			else if (a == b)
				return ("indeterminate_check_box", style + "#fff");
			return ("arrow_downward", style + "#0f0");
		}


		LineConfig CreateNewChartConfig()
	 => new LineConfig
	 {

		 Options = new LineOptions
		 {
			 Title = new OptionsTitle { Text = " - 7 Tage - ", Display = true, Position = Position.Bottom },
			 Scales = new Scales
			 {
				 xAxes = new List<CartesianAxis>
				 {
							new TimeAxis
							{
								Display = AxisDisplay.False,
								ScaleLabel = new ScaleLabel{LabelString = "Time"}
							}
				 },
				 yAxes = new List<CartesianAxis> {
							new LinearCartesianAxis {
								Position = Position.Left,
								Ticks = new LinearCartesianTicks{ MaxTicksLimit = 2, FontColor = "#aaa" },
								GridLines = new GridLines{
									Display = false
								}
							}
							,new LinearCartesianAxis {
								Position = Position.Right,
								Ticks = new LinearCartesianTicks{ MaxTicksLimit = 2, FontColor = "#aaa" },
								GridLines = new GridLines{
									Display = false
								}
							}
				 }
			 },
			 Responsive = true,
			 Hover = new LineOptionsHover { Enabled = false },
			 Legend = new Legend
			 {

				 Display = false
			 },
		 }
	 };


	}
}
