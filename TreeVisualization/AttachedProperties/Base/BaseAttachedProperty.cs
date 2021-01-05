using System;
using System.Windows;

namespace TreeVisualization
{
    /// <summary>
    /// A base attached property to replace the vanilla WPF attached property.
    /// </summary>
    /// <typeparam name="Parent">The parent class to be the attached property.</typeparam>
    /// <typeparam name="Property">The type of this attached property.</typeparam>
    public abstract class BaseAttachedProperty<Parent, Property> where Parent : new()
    {
        #region Events
        /// <summary>
        /// Fired when the value changes.
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged;

        /// <summary>
        /// Fired when the value changes, even when the value is the same.
        /// </summary>
        public event Action<DependencyObject, object> ValueUpdated;
        #endregion

        #region Public Properties
        /// <summary>
        /// A singleton of the parent class.
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();
        #endregion

        #region Attached Property Definitions
        /// <summary>
        /// The attached property for this class.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(
            "Value",
            typeof(Property),
            typeof(BaseAttachedProperty<Parent, Property>),
            new UIPropertyMetadata(
                default(Property),
                new PropertyChangedCallback(OnValuePropertyChanged),
                new CoerceValueCallback(OnValuePropertyUpdated)
                )
            );

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed.
        /// </summary>
        /// <param name="d">The UI element that had it's property changed.</param>
        /// <param name="e">The arguments of the event.</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Call the parent function
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueChanged(d, e);

            // Call event Listeners
            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueChanged?.Invoke(d, e);
        }

        /// <summary>
        /// The callback event when the <see cref="ValueProperty"/> is changed, event if it is the same value.
        /// </summary>
        /// <param name="d">The UI element that had it's property changed.</param>
        /// <param name="e">The arguments of the event.</param>
        private static object OnValuePropertyUpdated(DependencyObject d, object value)
        {
            // Call the parent function
            (Instance as BaseAttachedProperty<Parent, Property>)?.OnValueUpdated(d, value);

            // Call event Listeners
            (Instance as BaseAttachedProperty<Parent, Property>)?.ValueUpdated?.Invoke(d, value);

            return value;
        }

        /// <summary>
        /// Get the attached property of type <see cref="Property"/>.
        /// </summary>
        /// <param name="d">The element to get the property from.</param>
        /// <returns>Return the attached property value of type <see cref="Property"/>.</returns>
        public static Property GetValue(DependencyObject d)
        {
            return (Property)d.GetValue(ValueProperty);
        }

        /// <summary>
        /// Set the attached property of type <see cref="Property"/>.
        /// </summary>
        /// <param name="d">The element to set the property from.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetValue(DependencyObject d, Property value)
        {
            d.SetValue(ValueProperty, value);
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// The method that is called when any attached property of this type is changed.
        /// </summary>
        /// <param name="sender">The UI element that had it's property changed.</param>
        /// <param name="e">The arguments for this event.</param>
        public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// The method that is called when any attached property of this type is changed, event if it is the same value.
        /// </summary>
        /// <param name="sender">The UI element that had it's property changed.</param>
        /// <param name="e">The arguments for this event.</param>
        public virtual void OnValueUpdated(DependencyObject sender, object value) { }
        #endregion

        #region Helper Methods
        public static string GetParentType()
        {
            return typeof(Parent).ToString();
        }

        public static string GetPropertyType()
        {
            return typeof(Property).ToString();
        }
        #endregion
    }
}