namespace SampleBarChart
{
	using System;
	using System.Windows;

	using StockSharp.Algo;
	using StockSharp.BarChart;
	using StockSharp.BusinessEntities;
	using StockSharp.Xaml.Charting;
	using StockSharp.Localization;
	using StockSharp.Messages;

	public partial class HistoryCandlesWindow
	{
		private readonly Security _security;
		private readonly ChartCandleElement _candlesElem;

		public HistoryCandlesWindow(Security security)
		{
			if (security == null)
				throw new ArgumentNullException(nameof(security));

			_security = security;

			InitializeComponent();
			Title = _security.Code + LocalizedStrings.Str3747;

			TimeFramePicker.ItemsSource = BarChartMessageAdapter.TimeFrames;
			TimeFramePicker.SelectedIndex = 0;

			DateFromPicker.Value = DateTime.Today.AddDays(-7);
			DateToPicker.Value = DateTime.Today;

			var area = new ChartArea();
			_candlesElem = new ChartCandleElement();
			area.Elements.Add(_candlesElem);

			Chart.Areas.Add(area);
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			if (DateFromPicker.Value == null || DateToPicker.Value == null)
			{
				MessageBox.Show(LocalizedStrings.Str3748, Title, MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			bool isSuccess;
			var messages = MainWindow.Instance.Trader.GetHistoricalCandles(_security, typeof(TimeFrameCandleMessage), (TimeSpan)TimeFramePicker.SelectedValue, (DateTime)DateFromPicker.Value, (DateTime)DateToPicker.Value, out isSuccess);

			Chart.Reset(new[] { _candlesElem });

			foreach (var message in messages)
				Chart.Draw(_candlesElem, message.ToCandle(_security));
		}
	}
}
