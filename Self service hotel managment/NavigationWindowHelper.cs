using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;

namespace Self_service_hotel_managment
{
   
    public class NavigationWindowHelper
    {
        public static readonly DependencyProperty FrameNextNavigationStotryboardProperty = DependencyProperty.RegisterAttached("FrameNextNavigationStotryboard", typeof(Storyboard), typeof(NavigationWindowHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, FrameNextNavigationStotryboardProprtyChanged));
        private static void FrameNextNavigationStotryboardProprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MetroNavigationWindow)
            {

                Storyboard st = GetFrameNextNavigationStotryboard(d);
                if (st != null)
                {
                    (d as MetroNavigationWindow).Navigating += (sm, ar) =>
                    {
                        if (ar.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
                        {
                            st.Begin((d as MetroNavigationWindow));
                        }
                    };
                }
            }
        }
        public static void SetFrameNextNavigationStotryboard(DependencyObject control, Storyboard st)
        {
            control.SetValue(FrameNextNavigationStotryboardProperty, st);
        }
        public static Storyboard GetFrameNextNavigationStotryboard(DependencyObject control)
        {
            var val = control.GetValue(FrameNextNavigationStotryboardProperty);
            if (val is Storyboard)
                return (Storyboard)val;
            return null;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>

        public static readonly DependencyProperty FrameBackNavigationStotryboardProperty = DependencyProperty.RegisterAttached("FrameBackNavigationStotryboard", typeof(Storyboard), typeof(NavigationWindowHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, FrameBackNavigationStotryboardProprtyChanged));
        private static void FrameBackNavigationStotryboardProprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MetroNavigationWindow)
            {
                Storyboard st = GetFrameBackNavigationStotryboard(d);
                if (st != null)
                {
                    (d as MetroNavigationWindow).Navigating += (sm, ar) =>
                    {
                        if (ar.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
                        {
                            st.Begin((d as MetroNavigationWindow));
                        }
                    };
                }
            }
        }
        public static void SetFrameBackNavigationStotryboard(DependencyObject control, Storyboard st)
        {
            control.SetValue(FrameBackNavigationStotryboardProperty, st);
        }
        public static Storyboard GetFrameBackNavigationStotryboard(DependencyObject control)
        {
            var val = control.GetValue(FrameBackNavigationStotryboardProperty);
            if (val is Storyboard)
                return (Storyboard)val;
            return null;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>



        public static readonly DependencyProperty FrameNavigationProgressProperty = DependencyProperty.RegisterAttached("FrameNavigationProgress", typeof(double), typeof(NavigationWindowHelper), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, FrameNavigationProgressProprtyChanged));
        private static void FrameNavigationProgressProprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MetroNavigationWindow)
            {
                (d as MetroNavigationWindow).NavigationProgress += (sm, ar) =>
                {
                    SetFrameNavigationProgress(d, ToPercentage(ar.BytesRead, ar.MaxBytes));
                };

            }
        }
        public static void SetFrameNavigationProgress(DependencyObject control, double st)
        {
            control.SetValue(FrameNavigationProgressProperty, st);
        }
        public static double GetFrameNavigationProgress(DependencyObject control)
        {
            var val = control.GetValue(FrameNavigationProgressProperty);
            if (val is double)
                return (double)val;
            return 0;
        }


        /// <summary>
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>

        public static readonly DependencyProperty FrameNavigationErrorMessageProperty = DependencyProperty.RegisterAttached("FrameNavigationErrorMessage", typeof(string), typeof(NavigationWindowHelper), new PropertyMetadata(null, FrameNavigationErrorMessageProprtyChanged));
        private static void FrameNavigationErrorMessageProprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MetroNavigationWindow)
            {
                (d as MetroNavigationWindow).NavigationFailed += (sm, ar) =>
                {

                    //MessageBox.Show(ar.Exception.Message);
                    SetFrameNavigationErrorMessage(d, ar.Exception.Message + "\nFull Error:\n" + ar.Exception.ToString());
                };
                (d as MetroNavigationWindow).Navigating += (sm, ar) =>
                {
                    SetFrameNavigationErrorMessage(d, null);
                };

            }
        }
        public static void SetFrameNavigationErrorMessage(DependencyObject control, string st)
        {
            control.SetValue(FrameNavigationErrorMessageProperty, st);
        }
        public static string GetFrameNavigationErrorMessage(DependencyObject control)
        {
            var val = control.GetValue(FrameNavigationErrorMessageProperty);
            if (val is string)
                return (string)val;
            return null;
        }

        public static double ToPercentage(long value, long maximume)
        {
            try
            {
                if (value != 0 && maximume != 0)
                {
                    return Math.Round((value * 100.0) / maximume, 2);
                }
            }
            catch (Exception) { }
            return 0;
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////
        /// </summary>

        public static readonly DependencyProperty FrameNavigationRemoveNextEntryProperty = DependencyProperty.RegisterAttached("FrameNavigationRemoveNextEntry", typeof(bool), typeof(NavigationWindowHelper), new PropertyMetadata(false, FrameNavigationRemoveNextEntryProprtyChanged));
        private static void FrameNavigationRemoveNextEntryProprtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MetroNavigationWindow)
            {
                if (GetFrameNavigationRemoveNextEntry(d))
                {
                    (d as MetroNavigationWindow).Navigating += (sm, ar) =>
                    {
                        if (ar.NavigationMode == System.Windows.Navigation.NavigationMode.Back)
                        {
                            (d as MetroNavigationWindow).RemoveBackEntry();
                        }


                    };
                }
            }
        }
        public static void SetFrameNavigationRemoveNextEntry(DependencyObject control, bool st)
        {
            control.SetValue(FrameNavigationRemoveNextEntryProperty, st);
        }
        public static bool GetFrameNavigationRemoveNextEntry(DependencyObject control)
        {
            var val = control.GetValue(FrameNavigationRemoveNextEntryProperty);
            if (val is bool)
                return (bool)val;
            return false;
        }



    }
}
