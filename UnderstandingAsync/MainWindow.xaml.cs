﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UnderstandingAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            timer.Elapsed += timer_Elapsed;
        }

        private const int DefaultSleeptTime = 1000;
        System.Timers.Timer timer = new System.Timers.Timer(10);

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            Application.Current.Dispatcher.Invoke(() => TextBlock_Timer.Text = $"{now.Second}.{now.Millisecond}");
        }

        private async void String_WithAsynLabel_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            button.IsEnabled = !(CheckBox_DisableButton.IsChecked).GetValueOrDefault(false);
            ListBoxResults.Items.Add(await TaskExamples.String_WithAsynLabel());
            button.IsEnabled = true;
        }

        private async void String_WithTaskRun_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            button.IsEnabled = !(CheckBox_DisableButton.IsChecked).GetValueOrDefault(false);
            ListBoxResults.Items.Add(await TaskExamples.String_WithTaskRun());
            button.IsEnabled = true;
        }    

        private async void String_WithTaskFromResultAsWhole_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            button.IsEnabled = !(CheckBox_DisableButton.IsChecked).GetValueOrDefault(false);
            ListBoxResults.Items.Add(await TaskExamples.String_WithTaskFromResultAsWhole());
            button.IsEnabled = true;
        }

        private async void RunAll_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            button.IsEnabled = !(CheckBox_DisableButton.IsChecked).GetValueOrDefault(false);

            var tasks = new List<Task<string>>()
            {
                TaskExamples.String_WithAsynLabel(),
                TaskExamples.String_WithTaskRun(),
                TaskExamples.String_WithTaskFromResultAsWhole()
            };

            foreach (var task in tasks)
                ListBoxResults.Items.Add(await task);

            /* The ForEach lambda expression required it's own async / await as shown below.
            * If you uncomment the line below and comment out the foreach (var task...) above then you'll notice the button
            * is re-enabled before all tasks are complete.  Therefore we can see that the ForEach lambda expression is started and returned to this task
            * before it completes.  Both work but the button is re-enabled before completion. */

            //tasks.ForEach(async task => ListBoxResults.Items.Add(await task));

            button.IsEnabled = true;
        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
            => ListBoxResults.Items.Clear();

        private void Window_Loaded(object sender, RoutedEventArgs e)
            => timer.Start();

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            Thread.Sleep(200);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int? sleepTime;

            try
            {
                sleepTime = Convert.ToInt32(TextBox_SleepTime.Text);
            }
            catch
            {
                TextBox_SleepTime.Text = DefaultSleeptTime.ToString();
                sleepTime = null;
            }

            TaskExamples.SleepTime = sleepTime ?? DefaultSleeptTime;
        }
    }
}
