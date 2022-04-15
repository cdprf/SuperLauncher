﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.IO;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace SuperLauncher
{
    public partial class ModernLauncherIcon : UserControl
    {
        public string rFilePath;
        public ModernLauncherBadge Badge;
        public bool MouseOver = false;
        public string FileName;
        public string FilePath { 
            get
            {
                return rFilePath;
            } 
            set
            { 
                rFilePath = value;
                FileInfo fi = new(rFilePath);
                Icon icon = Icon.ExtractAssociatedIcon(rFilePath);
                FileName = fi.Name;
                NameText.Text = ExtRemover(FileName);
                LIcon.Source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }
        public ModernLauncherIcon()
        {
            InitializeComponent();
        }
        public ModernLauncherIcon(string FilePath)
        {
            InitializeComponent();
            this.FilePath = FilePath;
        }
        private DoubleAnimation To1 = new()
        {
            To = 1,
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
        };
        private DoubleAnimation To0 = new()
        {
            To = 0,
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
        };
        private DoubleAnimation To0_5 = new()
        {
            To = 0.5,
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
        };
        private DoubleAnimation To0_9 = new()
        {
            To = 0.9,
            Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
        };
        private void UserControl_FadeInHighlight(object sender, object e)
        {
            MouseOver = true;
            _ = StartBadgeTimer();
            Highlight.BeginAnimation(OpacityProperty, To1);
        }
        private void UserControl_FadeOutHightlight(object sender, object e)
        {
            MouseOver = false;
            if (Badge != null) Badge.Close();
            Highlight.BeginAnimation(OpacityProperty, To0);
            IconScale.BeginAnimation(ScaleTransform.ScaleXProperty, To1);
            IconScale.BeginAnimation(ScaleTransform.ScaleYProperty, To1);
        }
        private void UserControl_FadeOutHightlightSlight(object sender, object e)
        {
            Highlight.BeginAnimation(OpacityProperty, To0_5);
            IconScale.BeginAnimation(ScaleTransform.ScaleXProperty, To0_9);
            IconScale.BeginAnimation(ScaleTransform.ScaleYProperty, To0_9);
        }
        private void UserControl_FadeInHightlightSlight(object sender, object e)
        {
            Highlight.BeginAnimation(OpacityProperty, To1);
            IconScale.BeginAnimation(ScaleTransform.ScaleXProperty, To1);
            IconScale.BeginAnimation(ScaleTransform.ScaleYProperty, To1);
        }
        private string ExtRemover(string FileName)
        {
            string[] parts = FileName.Split('.');
            if (parts.Length > 1)
            {
                string[] newParts = new string[parts.Length - 1];
                for (int i = 0; i < newParts.Length; i++) newParts[i] = parts[i];
                return string.Join('.', newParts);
            }
            return FileName;
        }
        public async Task StartBadgeTimer()
        {
            await Task.Delay(1000);
            if (!MouseOver) return;
            if (Badge != null) Badge.Close();
            
            Badge = new(FileName);
            Badge.Show();
        }
    }
}
