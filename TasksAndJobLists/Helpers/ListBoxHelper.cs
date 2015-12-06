using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TasksAndJobLists
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty ScaleYAnimationProperty =
            DependencyProperty.RegisterAttached("ScaleYAnimation", typeof(double), typeof(ListBoxHelper), new FrameworkPropertyMetadata(0d));

        public static double GetScaleYAnimation(UIElement element)
        {
            return (double)element.GetValue(ScaleYAnimationProperty);
        }

        public static void SetScaleYAnimation(UIElement element, double value)
        {
            element.SetValue(ScaleYAnimationProperty, value);
        }
    }
}
