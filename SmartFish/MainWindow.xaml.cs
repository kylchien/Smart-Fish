using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Timers;


namespace SmartFish
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		World world;
		DispatcherTimer dt = new DispatcherTimer();

		public MainWindow()
		{
			InitializeComponent(); //auto-generated
			
			world = new World(ref mainCanvas);
	
			// 60 FPS = 1/60 sec =0.01667 millisec
			// Nontheless, this will be slower than real time
			// for model/GUI updates
			//dt.Interval = new TimeSpan(0, 0, 0, 0, 16); 

			

			dt.Interval =TimeSpan.FromMilliseconds(10);
			dt.Tick += RenderFrame;
			dt.Start();
			
			
			//Application.Current.Shutdown();
			
			
			
			//ReadXML.Read("SmartFish.txt");
			/*
			WriteXML.Write();
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
			Application.Current.Shutdown();
			*/
		}

		
		
		
		
		
		private void RenderFrame(object sender, EventArgs e)
		{
			world.Update();
			string generation = Convert.ToString(world.Generation);
			//textBlock.Text = generation;
			//Text(50,50,generation,Colors.Black, ref mainCanvas);
		}

		
		private void Text(double x, double y, string text, Color color, ref Canvas aCanvas) 
		{
			TextBlock textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.Foreground = new SolidColorBrush(color);
			Canvas.SetLeft(textBlock, x);
			Canvas.SetTop(textBlock, y);
			aCanvas.Children.Add(textBlock);
			
			textBlock.Background  = Brushes.AntiqueWhite;
			textBlock.Foreground  = Brushes.Navy;

			textBlock.FontFamily    = new FontFamily("Century Gothic");
			textBlock.FontSize      = 16;
			textBlock.FontStretch   = FontStretches.UltraExpanded;
			textBlock.FontStyle     = FontStyles.Italic;
			textBlock.FontWeight    = FontWeights.UltraBold;

			textBlock.LineHeight    = Double.NaN;
			textBlock.Padding       = new Thickness(10, 10, 10, 10);
			textBlock.TextAlignment = TextAlignment.Center;
			textBlock.TextWrapping  = TextWrapping.Wrap;

			textBlock.Typography.NumeralStyle = FontNumeralStyle.OldStyle;
			textBlock.Typography.SlashedZero  = true;

		}//Text
		
		
		void ButtonPause_Click(object sender, RoutedEventArgs e)
		{
			dt.Stop();
		}
		
		void ButtonGo_Click(object sender, RoutedEventArgs e)
		{
			dt.Start();
		}
		
		void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			dt.Stop();
			world.SaveToFile();
		}

		private void MainWin_Loaded(object sender, RoutedEventArgs e)
		{
			Console.Out.WriteLine("{0}, {1}", MainWin.ActualWidth, MainWin.ActualHeight);
			Console.Out.WriteLine("{0}, {1}", mainCanvas.ActualWidth, mainCanvas.ActualHeight);
		}

		private void backwardButton_Click(object sender, RoutedEventArgs e)
		{
			Fish.updateSwimSpeed('-');
			
			if (Fish.reachMinSwimSpeed())
				backwardButton.ClickMode = ClickMode.Hover;
			else if (Fish.reachMaxSwimSpeed())
				forwardButton.ClickMode = ClickMode.Release;
			 

		}

		private void goButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void forwardButton_Click(object sender, RoutedEventArgs e)
		{
			Fish.updateSwimSpeed('+');
			
			if (Fish.reachMinSwimSpeed())
				backwardButton.ClickMode = ClickMode.Release;
			else if (Fish.reachMaxSwimSpeed())
				forwardButton.ClickMode = ClickMode.Hover; 
		}
	}

	

}

