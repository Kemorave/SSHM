using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Self_service_hotel_managment.Common
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Self_service_hotel_managment.Common"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Self_service_hotel_managment.Common;assembly=Self_service_hotel_managment.Common"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2) 
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:FlowButton/>
    ///
    /// </summary>
    public class FlowButton : System.Windows.Controls.Primitives.ButtonBase
    {
        public static System.Windows.Media.Animation.RepeatBehavior RepeatBehaviorTwice { get; } = new System.Windows.Media.Animation.RepeatBehavior(2);
        static FlowButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowButton), new FrameworkPropertyMetadata(typeof(FlowButton)));
        }
        public FlowButton()
        {
        }
        protected override void OnClick()
        {
            if (IsBusy)
            {
                if (IsCancalable)
                {
                    CancelCommand?.Execute(null);
                    RaiseEvent(new RoutedEventArgs(ClickEvent, this));
                }
                return;
            }
            
            base.OnClick();
        }

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }


        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }


        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        public DataTemplate IsBusyTemplate
        {
            get => (DataTemplate)GetValue(IsBusyTemplateProperty);
            set => SetValue(IsBusyTemplateProperty, value);
        }
        public Brush BackBrush
        {
            get => (Brush)GetValue(BackBrushProperty);
            set => SetValue(BackBrushProperty, value);
        }

        public bool IsCancalable
        {
            get { return (bool)GetValue(IsCancalableProperty); }
            set { SetValue(IsCancalableProperty, value); }
        }

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(FlowButton), new PropertyMetadata(null));


        // Using a DependencyProperty as the backing store for IsCancalable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCancalableProperty =
            DependencyProperty.Register("IsCancalable", typeof(bool), typeof(FlowButton), new PropertyMetadata(false));


        // Using a DependencyProperty as the backing store for IsBusyTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyTemplateProperty =
            DependencyProperty.Register("IsBusyTemplate", typeof(DataTemplate), typeof(FlowButton), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(FlowButton), new PropertyMetadata(0.0));

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(FlowButton), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for HasShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasShadowProperty =
            DependencyProperty.Register("HasShadow", typeof(bool), typeof(FlowButton), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for BackBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(Brush), typeof(FlowButton), new PropertyMetadata(Brushes.Black));

    }
}