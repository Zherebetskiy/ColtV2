using System.Text.RegularExpressions;

namespace Colt.UI.Desktop.Behaviors
{
    public class DoubleValidationBehavior : Behavior<Entry>
    {
        private const string DoubleRegex = @"^[0-9]*(?:\,[0-9]*)?$";

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                return;
            }

            if (sender is Entry entry && !Regex.IsMatch(e.NewTextValue, DoubleRegex))
            {
                entry.Text = e.OldTextValue;
            }
        }
    }
}
